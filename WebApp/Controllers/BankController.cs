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
using Serilog;
using WebApp.Models;
using WebApp.PaymentGatewayImplService;

namespace WebApp.Controllers;

public class BankController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _work;
    private readonly ILogger<BankController> _logger;
    public BankController(UserManager<User> userManager, IUnitOfWork work, ILogger<BankController> logger)
    {
        _userManager = userManager;
        _work = work;
        _logger = logger;
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

    //
    // [AllowAnonymous]
    // [HttpPost("MellatResponse")]
    // public async Task<IActionResult> MellatResponse(MelatResult melatResult)
    // {
    //     bool isError = false;
    //     string errorMsg = "";
    //     string succeedMsg = "";
    //
    //     try
    //     {
    //         BankTransaction order = await _work.GenericRepository<BankTransaction>().Table
    //             .FirstOrDefaultAsync(x => x.Id == melatResult.SaleOrderId.ToInt());
    //         Factor factor = await _work.GenericRepository<Factor>().Table
    //             .FirstOrDefaultAsync(x => x.Id == order.FactorId);
    //
    //         if (order != null)
    //         {
    //             ServicePointManager.ServerCertificateValidationCallback +=
    //                 delegate(
    //                     Object sender1,
    //                     X509Certificate certificate,
    //                     X509Chain chain,
    //                     SslPolicyErrors sslPolicyErrors)
    //                 {
    //                     return true;
    //                 };
    //             var payment = new WebApp.PaymentGatewayImplService.PaymentGatewayClient();
    //
    //             var result = await payment.bpVerifyRequestAsync(Convert.ToInt64(BpmConfig.TerminalId),
    //                 BpmConfig.UserName,
    //                 BpmConfig.Password,
    //                 Convert.ToInt64(order.Id.ToString()),
    //                 Convert.ToInt64(melatResult.SaleOrderId),
    //                 Convert.ToInt64(melatResult.SaleReferenceId));
    //
    //             if (result.Body.@return == "0")
    //             {
    //                 order.SaleReferenceId = melatResult.SaleReferenceId.ToInt();
    //                 order.PaymentFinished = true;
    //                 order.StatusPayment = melatResult.ResCode;
    //                 factor.Status = Status.Pending;
    //                 factor.StatusPayment = melatResult.ResCode;
    //                 await _work.GenericRepository<Factor>().UpdateAsync(factor, CancellationToken.None);
    //                 await _work.GenericRepository<BankTransaction>().UpdateAsync(order, CancellationToken.None);
    //                 succeedMsg = "پرداخت شما با موفقیت انجام شد";
    //             }
    //             else
    //             {
    //                 factor.Status = Status.Field;
    //                 factor.StatusPayment = melatResult.ResCode;
    //                 order.StatusPayment = melatResult.ResCode;
    //                 order.SaleReferenceId = melatResult.SaleReferenceId.ToInt();
    //                 order.PaymentFinished = false;
    //                 await _work.GenericRepository<Factor>().UpdateAsync(factor, CancellationToken.None);
    //                 await _work.GenericRepository<BankTransaction>().UpdateAsync(order, CancellationToken.None);
    //                 isError = true;
    //                 succeedMsg = $"پرداخت آنلاین با خطا {result.Body.@return} مواجه شده است.";
    //             }
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         isError = true;
    //         errorMsg = ex.Message;
    //     }
    //
    //     ApiResult.MelatMessage melatMessage = new ApiResult.MelatMessage
    //     {
    //         isError = isError,
    //         errorMsg = errorMsg,
    //         succeedMsg = succeedMsg
    //     };
    //     return Ok(melatMessage);
    // }

    [AllowAnonymous]
    public async Task<IActionResult> MellatBack(string RefId, string ResCode, string SaleOrderId,
        string SaleReferenceId)
    {
        //iewBag.Message = null;
        //iewBag.IsSuccess = false;
        //iewBag.RefId = RefId;
        //iewBag.ResCode = ResCode;
        //iewBag.SaleReferenceId = SaleReferenceId;
        ///Order Id Back
        //iewBag.SaleOrderId = SaleOrderId;

        //ry
        //
        //   if (ResCode == "0")
        //   {
        //       MelatResult melatResult = new MelatResult()
        //       {
        //           RefId = RefId,
        //           ResCode = ResCode,
        //           SaleOrderId = SaleOrderId,
        //           SaleReferenceId = SaleReferenceId
        //       };
        await MellatReturn(RefId, ResCode, SaleOrderId, SaleReferenceId);
        //HttpClient httpClient = new HttpClient();
        //var apiSerlize = JsonConvert.SerializeObject(melatResult);
        //var content = new StringContent(apiSerlize, Encoding.UTF8, "application/json");
        //var result = await httpClient.PostAsync($"https://front.parsme.com/Bank/MellatResponse", content);

        //if (result.IsSuccessStatusCode)
        //{
        //    var _value = await result.Content.ReadAsStringAsync();
        //    ApiResult.MelatResultResponse melatMessage =
        //        JsonConvert.DeserializeObject<ApiResult.MelatResultResponse>(_value);

        //    if (melatMessage.data.isError)
        //    {
        //        ViewBag.Message = melatMessage.data.errorMsg;
        //    }
        //    else
        //    {
        //        KavenegarApi webApi = new KavenegarApi(apikey: ApiKeys.ApiKey);
        //        ViewBag.Message = melatMessage.data.succeedMsg;
        //        ViewBag.IsSuccess = true;
        //        HttpContext.Session.SetString("basket", JsonConvert.SerializeObject(new List<int>()));
        //    }
        //}
        //     }
        //     else
        //     {
        //         ViewBag.Message = "پرداخت آنلاین با خطا مواجه شده است.";
        //     }
        // }
        // catch
        // {
        //     ViewBag.Message = "خطا در برقراری ارتباط با درگاه بانک ملت";
        // }


        ViewBag.Factor = await _work.GenericRepository<Factor>().TableNoTracking
            .Include(x => x.PostMethod)
            .Include(x => x.UserAddress)
            .Include(x => x.Products).ThenInclude(x => x.FactorProductColor)
            .FirstOrDefaultAsync(x => x.Id == 87);
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


    private async Task MellatReturn(string RefId, string ResCode, string SaleOrderId, string SaleReferenceId)
    {
        Log.Information(
            "MellatReturn called with parameters: RefId={RefId}, ResCode={ResCode}, SaleOrderId={SaleOrderId}, SaleReferenceId={SaleReferenceId}");

        BypassCertificateError();
        var saleReferenceId = string.Empty;
        var resCode = string.Empty;

        try
        {
            // دریافت ResCode
            if (string.IsNullOrWhiteSpace(ResCode))
            {
                resCode = Request.Query["ResCode"].FirstOrDefault()
                          ?? Request.Form["ResCode"].FirstOrDefault()
                          ?? RouteData.Values["ResCode"]?.ToString();
                Log.Information(
                    "MellatReturn res1code={resCode}");

            }
            else
            {
                resCode = ResCode;
            }

            Log.Information("ResCode resolved to: {ResCode}", resCode);

            // دریافت SaleReferenceId
            if (string.IsNullOrWhiteSpace(SaleReferenceId))
            {
                saleReferenceId = Request.Query["SaleReferenceId"].FirstOrDefault()
                                  ?? Request.Form["SaleReferenceId"].FirstOrDefault()
                                  ?? RouteData.Values["SaleReferenceId"]?.ToString();
                Log.Information(
                    "MellatReturn saleReferenceId={saleReferenceId}");
            }
            else
            {
                saleReferenceId = SaleReferenceId;
            }

            Log.Information("SaleReferenceId resolved to: {SaleReferenceId}", saleReferenceId);

            if (string.IsNullOrEmpty(saleReferenceId))
            {
                if (!string.IsNullOrEmpty(resCode))
                {
                    ViewBag.Message = Helpers.MellatResult(resCode);
                    ViewBag.SaleReferenceId = "**************";
                }
                else
                {
                    ViewBag.Message = "رسید قابل قبول نیست";
                    ViewBag.SaleReferenceId = "**************";
                }
            }
            else
            {
                long saleOrderId = 0;
                long saleReferenceIdLong = 0;
                string refId = null;
                try
                {
                    // تلاش برای گرفتن SaleOrderId و تبدیل آن به نوع عددی
                    if (string.IsNullOrWhiteSpace(SaleOrderId))
                    {
                        saleOrderId = long.Parse(Request.Query["SaleOrderId"].FirstOrDefault()
                                                 ?? Request.Form["SaleOrderId"].FirstOrDefault()
                                                 ?? RouteData.Values["SaleOrderId"]?.ToString());
                        Log.Information(
                            "MellatReturn saleOrderId={saleOrderId}");
                    }
                    else
                    {
                        saleOrderId = SaleOrderId.ToInt();
                    }

                    saleReferenceIdLong = long.Parse(saleReferenceId.Trim());

                    if (string.IsNullOrWhiteSpace(RefId))
                    {
                        refId = Request.Query["RefId"].FirstOrDefault()
                                ?? Request.Form["RefId"].FirstOrDefault()
                                ?? RouteData.Values["RefId"]?.ToString();
                        Log.Information(
                            "MellatReturn refId={refId}");
                    }
                    else
                    {
                        refId = RefId;
                    }

                    Log.Information(
                        "Parsed SaleOrderId: {SaleOrderId}, SaleReferenceIdLong: {SaleReferenceIdLong}, RefId: {RefId}",
                        saleOrderId, saleReferenceIdLong, refId);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error occurred while parsing SaleOrderId and SaleReferenceId");
                    ViewBag.IsSuccess = false;
                    ViewBag.Message = ex + "<br/>" +
                                      " وضعیت:مشکلی در پرداخت بوجود آمده ، در صورتی که وجه پرداختی از حساب بانکی شما کسر شده است آن مبلغ به صورت خودکار برگشت داده خواهد شد ";
                    ViewBag.SaleReferenceId = "**************";
                    return;
                }

                try
                {
                    var payment = new PaymentGatewayClient();
                    var result = await payment.bpVerifyRequestAsync(Convert.ToInt64(BpmConfig.TerminalId),
                        BpmConfig.UserName,
                        BpmConfig.Password,
                        saleOrderId,
                        saleOrderId,
                        saleReferenceIdLong);

                    if (result.Body.@return == "0")
                    {
                        ViewBag.Message = "پرداخت شما با موفقیت انجام شد";
                        ViewBag.SaleReferenceId = saleReferenceIdLong;
                        ViewBag.IsSuccess = true;
                        Log.Information("Payment verified successfully for SaleReferenceId: {SaleReferenceId}",
                            saleReferenceIdLong);
                    }
                    else
                    {
                        ViewBag.IsSuccess = false;
                        ViewBag.Message = $"پرداخت آنلاین با خطا {result.Body.@return} مواجه شده است.";
                        Log.Warning("Payment failed with return code: {ReturnCode}", result.Body.@return);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "An error occurred during the payment verification process");
                    ViewBag.Message =
                        "مشکلی در پرداخت به وجود آمده است ، در صورتیکه وجه پرداختی از حساب بانکی شما کسر شده است آن مبلغ به صورت خودکار برگشت داده خواهد شد";
                    ViewBag.SaleReferenceId = "**************";
                }
            }
        }
        catch(Exception ex)
        {
            Log.Error(ex, "همون اول کار رید ");
        }
    }


    void BypassCertificateError()
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
    }
}