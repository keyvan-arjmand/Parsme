using System.Collections.Specialized;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Application.Common.Utilities;
using Application.Constants.Kavenegar;
using Application.Interfaces;
using Domain.Entity.Factor;
using Domain.Entity.IndexPage;
using Domain.Entity.Product;
using Domain.Entity.User;
using Domain.Enums;
using Kavenegar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApp.Models;

namespace WebApp.Controllers;

public class BankController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _work;

    public BankController(UserManager<User> userManager, IUnitOfWork work)
    {
        _userManager = userManager;
        _work = work;
    }

    // GET
    public async Task<string> Index(long Price, int factorId, int userId)
    {
        try
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null) throw new Exception();
            BankTransaction p = new BankTransaction
            {
                Amount = Price,
                StatusPayment = "-100",
                BankName = "به پرداخت ملت",
                SaleReferenceId = 0,
                BuyDatetime = System.DateTime.Now,
                UserId = user.Id,
                PaymentFinished = false,
                FactorId = factorId
            };
            await _work.GenericRepository<BankTransaction>().AddAsync(p, CancellationToken.None);
            int paymentid = p.Id;
            if (paymentid > 0)
            {
                ServicePointManager.ServerCertificateValidationCallback +=
                    delegate(
                        Object sender1,
                        X509Certificate certificate,
                        X509Chain chain,
                        SslPolicyErrors sslPolicyErrors)
                    {
                        return true;
                    };
                string date = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') +
                              DateTime.Now.Day.ToString().PadLeft(2, '0');
                string time = DateTime.Now.Hour.ToString().PadLeft(2, '0') +
                              DateTime.Now.Minute.ToString().PadLeft(2, '0') +
                              DateTime.Now.Second.ToString().PadLeft(2, '0');
                var redirect = Url.Action("MellatBack", "Payment");
                var payment = new WebApp.PaymentGatewayImplService.PaymentGatewayClient();

                var result = await payment.bpPayRequestAsync(int.Parse(BpmConfig.TerminalId),
                    BpmConfig.UserName,
                    BpmConfig.Password,
                    p.Id,
                    long.Parse($"{Price}0"),
                    date,
                    time,
                    "خرید از فروشگاه اینترنتی پارس می",
                    $"https://front.parsme.com{redirect}",
                    "0",
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty);

                string[] res = result.Body.@return.Split(',');

                if (res[0] == "0")
                {
                    ViewBag.SaleId = res[1];
                    ViewBag.Url = BpmConfig.PostUrl + res[1];
                    return res[1];
                    //mellatSelectDto.IsError = false;
                    //mellatSelectDto.RefId = res[1];
                    //mellatSelectDto.ErrorMessage = null;
                    //mellatSelectDto.PostUrl = BpmConfig.PostUrl;
                    //mellatSelectDto.RedirectUrl = BpmConfig.RedirectUrl;
                    //mellatSelectDto.BankOrderId = transAction.BankOrderId;
                }
                else
                {
                    return string.Empty;
                    //mellatSelectDto.IsError = true;
                    //mellatSelectDto.RefId = null;
                    //mellatSelectDto.ErrorMessage = "خطای " + res[0] + " در ارتباط با بانک";
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return string.Empty;
    }

    public class MelatResult
    {
        public string RefId { get; set; }
        public string ResCode { get; set; }
        public string SaleOrderId { get; set; }
        public string SaleReferenceId { get; set; }
    }

   

    [AllowAnonymous]
    [HttpPost("MellatResponse")]
    public async Task<IActionResult> MellatResponse(MelatResult melatResult)
    {
        bool isError = false;
        string errorMsg = "";
        string succeedMsg = "";

        try
        {
            BankTransaction order = await _work.GenericRepository<BankTransaction>().Table
                .FirstOrDefaultAsync(x => x.Id == melatResult.SaleOrderId.ToInt());
            Factor factor = await _work.GenericRepository<Factor>().Table
                .FirstOrDefaultAsync(x => x.Id == order.FactorId);

            if (order != null)
            {
                ServicePointManager.ServerCertificateValidationCallback +=
                    delegate(
                        Object sender1,
                        X509Certificate certificate,
                        X509Chain chain,
                        SslPolicyErrors sslPolicyErrors)
                    {
                        return true;
                    };
                var payment = new WebApp.PaymentGatewayImplService.PaymentGatewayClient();

                var result = await payment.bpVerifyRequestAsync(Convert.ToInt64(BpmConfig.TerminalId),
                    BpmConfig.UserName,
                    BpmConfig.Password,
                    Convert.ToInt64(order.Id.ToString()),
                    Convert.ToInt64(melatResult.SaleOrderId),
                    Convert.ToInt64(melatResult.SaleReferenceId));

                if (result.Body.@return == "0")
                {
                    order.SaleReferenceId = melatResult.SaleReferenceId.ToInt();
                    order.PaymentFinished = true;
                    order.StatusPayment = melatResult.ResCode;
                    factor.Status = Status.Pending;
                    factor.StatusPayment = melatResult.ResCode;
                    await _work.GenericRepository<Factor>().UpdateAsync(factor, CancellationToken.None);
                    await _work.GenericRepository<BankTransaction>().UpdateAsync(order, CancellationToken.None);
                    succeedMsg = "پرداخت شما با موفقیت انجام شد";
                }
                else
                {
                    factor.Status = Status.Field;
                    factor.StatusPayment = melatResult.ResCode;
                    order.StatusPayment = melatResult.ResCode;
                    order.SaleReferenceId = melatResult.SaleReferenceId.ToInt();
                    order.PaymentFinished = false;
                    await _work.GenericRepository<Factor>().UpdateAsync(factor, CancellationToken.None);
                    await _work.GenericRepository<BankTransaction>().UpdateAsync(order, CancellationToken.None);
                    isError = true;
                    succeedMsg = $"پرداخت آنلاین با خطا {result.Body.@return} مواجه شده است.";
                }
            }
        }
        catch (Exception ex)
        {
            isError = true;
            errorMsg = ex.Message;
        }

        ApiResult.MelatMessage melatMessage = new ApiResult.MelatMessage
        {
            isError = isError,
            errorMsg = errorMsg,
            succeedMsg = succeedMsg
        };
        return Ok(melatMessage);
    }
}