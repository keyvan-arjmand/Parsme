using System.Collections.Specialized;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Application.Common.Utilities;
using Application.Interfaces;
using Domain.Entity.Factor;
using Domain.Entity.IndexPage;
using Domain.Entity.Product;
using Domain.Entity.User;
using Domain.Enums;
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
    public async Task<string> Index(long Price, int factorId)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
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
                var redirect = Url.Action("MellatBack", "Bank");
                var payment = new WebApp.PaymentGatewayImplService.PaymentGatewayClient();

                var result = await payment.bpPayRequestAsync(int.Parse(BpmConfig.TerminalId),
                    BpmConfig.UserName,
                    BpmConfig.Password,
                    p.Id,
                    long.Parse($"{Price}0"),
                    date,
                    time,
                    "خرید از فروشگاه اینترنتی پارس می",
                    redirect,
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

    public async Task<IActionResult> MellatBack(string RefId, string ResCode, string SaleOrderId,
        string SaleReferenceId, string orderid)
    {
        ViewBag.Message = null;
        ViewBag.IsSuccess = false;
        ViewBag.RefId = RefId;
        ViewBag.ResCode = ResCode;
        ViewBag.SaleReferenceId = SaleReferenceId;
        //Order Id Back
        ViewBag.SaleOrderId = SaleOrderId;

        try
        {
            if (ResCode == "0")
            {
                MelatResult melatResult = new MelatResult()
                {
                    RefId = RefId,
                    ResCode = ResCode,
                    SaleOrderId = SaleOrderId,
                    SaleReferenceId = SaleReferenceId
                };

                HttpClient httpClient = new HttpClient();
                var apiSerlize = JsonConvert.SerializeObject(melatResult);
                var content = new StringContent(apiSerlize, Encoding.UTF8, "application/json");
                var result = await httpClient.PostAsync($"https://front.parsme.com/Bank/MellatResponse", content);

                if (result.IsSuccessStatusCode)
                {
                    var _value = await result.Content.ReadAsStringAsync();
                    ApiResult.MelatResultResponse melatMessage =
                        JsonConvert.DeserializeObject<ApiResult.MelatResultResponse>(_value);

                    if (melatMessage.data.isError)
                    {
                        ViewBag.Message = melatMessage.data.errorMsg;
                    }
                    else
                    {
                        ViewBag.Message = melatMessage.data.succeedMsg;
                        ViewBag.IsSuccess = true;
                        HttpContext.Session.SetString("basket", JsonConvert.SerializeObject(new List<int>()));
                    }
                }
            }
            else
            {
                ViewBag.Message = "پرداخت آنلاین با خطا مواجه شده است.";
            }
        }
        catch
        {
            ViewBag.Message = "خطا در برقراری ارتباط با درگاه بانک ملت";
        }

        ViewBag.Factor = await _work.GenericRepository<Factor>().TableNoTracking
            .Include(x => x.PostMethod)
            .Include(x => x.UserAddress)
            .Include(x => x.Products).ThenInclude(x => x.FactorProductColor).ThenInclude(x => x.FactorProduct)
            .Include(x => x.Products).ThenInclude(x => x.FactorProductColor).ThenInclude(x => x!.FactorProduct)
            .FirstOrDefaultAsync(x => x.Id == SaleOrderId.ToInt());
        var basketProducts = new List<Product>();
        if (HttpContext.Session.GetString("basket") != null)
        {
            var basketList = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("basket"))
                .ToList();
            foreach (var i in basketList)
            {
                var prodColor = await _work.GenericRepository<ProductColor>().TableNoTracking.Include(x => x.Color)
                    .FirstOrDefaultAsync(x => x.Id == i);

                var prod = await _work.GenericRepository<Product>().TableNoTracking.Include(x => x.ProductColors)
                    .ThenInclude(x => x.Color)
                    .Include(x => x.Offer)
                    .FirstOrDefaultAsync(x => x.Id == prodColor.ProductId);

                prod.ProductColors = new List<ProductColor>() { prodColor };
                basketProducts.Add(prod!);
            }
        }

        ViewBag.BasketProd = basketProducts;
        ViewBag.Search = await _work.GenericRepository<SearchResult>().TableNoTracking.Take(6).ToListAsync();
        var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
            .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Categories = cats;
        ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                             new FooterLink();
      
        ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                          new SeoPage();
        return View();
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
                    order.SaleReferenceId = melatResult.SaleReferenceId.ToInt();
                    order.PaymentFinished = true;
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