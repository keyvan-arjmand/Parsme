using Application.Admin.V1.Commands.AdminExist;
using Application.Admin.V1.Commands.ConfirmCodAdmin;
using Application.Admin.V1.Commands.ConfirmPasswordAdmin;
using Application.Admin.V1.Commands.LoginCodAdmin;
using Application.Admin.V1.Queries.CheckAdminNumber;
using Application.Common.ApiResult;
using Application.Common.Mapping;
using Application.Common.Utilities;
using Application.Constants.Kavenegar;
using Application.Dtos;
using Application.Dtos.Client;
using Application.Factors.Commands.SendMessageUser;
using Application.Interfaces;
using Application.Products.Commands;
using AutoMapper;
using Domain.Entity.Factor;
using Domain.Entity.Factor.Product;
using Domain.Entity.IndexPage;
using Domain.Entity.Product;
using Domain.Entity.User;
using Domain.Enums;
using Kavenegar;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Panel.Models;
using ProductColor = Domain.Entity.Product.ProductColor;
using ProductDetail = Domain.Entity.Product.ProductDetail;
using UserDto = Application.Dtos.Client.UserDto;

namespace Panel.Controllers;

public class AdminController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IUnitOfWork _work;
    private readonly IMapper? _mapper;

    public AdminController(UserManager<User> userManager, SignInManager<User> signInManager, IMediator mediator,
        IWebHostEnvironment webHostEnvironment, RoleManager<Role> roleManager, IUnitOfWork work, IMapper? mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mediator = mediator;
        _webHostEnvironment = webHostEnvironment;
        _roleManager = roleManager;
        _work = work;
        _mapper = mapper;
    }


    public async Task<ActionResult> Index()
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            ViewBag.FactorPendingCount = await _work.GenericRepository<Factor>().TableNoTracking
                .CountAsync(x => x.Status == Status.Pending && x.InsertDate.Date == DateTime.Now.Date);

            ViewBag.FactorCount = await _work.GenericRepository<Factor>().TableNoTracking
                .CountAsync(x => x.InsertDate.Date == DateTime.Now.Date);
            var startDate = new DateTime(2024, 11, 1); // تاریخ شروع
            var endDate = new DateTime(2024, 11, 30); // تاریخ پایان

            var factors = await _work.GenericRepository<Factor>().TableNoTracking
                .Where(f => f.InsertDate >= startDate && f.InsertDate <= endDate)
                .Include(f => f.Products)
                .ThenInclude(fp => fp.FactorProductColor)
                .AsQueryable() // تبدیل به IQueryable
                .ToListAsync();

            // متغیرهای جمع‌بندی
            double totalAmount = 0.0;
            double totalProfit = 0.0;
            double totalTax = 0.0;

            // محاسبه مقادیر
            foreach (var factor in factors)
            {
                totalAmount += factor.Amount; // مبلغ کل فاکتور

                foreach (var product in factor.Products)
                {
                    foreach (var productColor in product.FactorProductColor)
                    {
                        // محاسبه سود برای هر رنگ محصول
                        var profitForColor = (productColor.Price * productColor.Count * product.Profit / 100);
                        var adjustedProfitForColor = profitForColor - productColor.OfferAmount - product.DiscountAmount;
                        totalProfit += adjustedProfitForColor;
                    }
                }

                // محاسبه مالیات
                totalTax += factor.Amount * 0.10; // فرض مالیات ۱۰٪
            }

            // ساخت گزارش نهایی
            var monthlyReport = new IndexReport
            {
                TotalAmount = totalAmount,
                TotalProfit = totalProfit,
                TotalTax = totalTax,
                TotalWithTaxAmount = totalAmount - totalTax
            };

            ViewBag.Report = monthlyReport;


            ViewBag.Users = await _userManager.Users.Take(12).OrderByDescending(x => x.InsertDate).ToListAsync();
            ViewBag.Products = await _work.GenericRepository<Product>().TableNoTracking
                .Include(x => x.SubCategory)
                .Include(x => x.Brand)
                .OrderByDescending(x => x.InsertDate)
                .Take(8)
                .ToListAsync();
            ViewBag.ProductReport = new ReportIndexModel
            {
                First = await _work.GenericRepository<ProductColor>().TableNoTracking
                    .CountAsync(),
                Second = await _work.GenericRepository<ProductColor>().TableNoTracking
                    .CountAsync(x => x.Inventory > 0),
            };
            ViewBag.FactorReport = new ReportIndexModel
            {
                First = await _work.GenericRepository<Factor>().TableNoTracking
                    .CountAsync(),
                Second = await _work.GenericRepository<Factor>().TableNoTracking
                    .CountAsync(x => x.Status != Status.Field),
            };
            ViewBag.FactorFieldReport = new ReportIndexModel
            {
                First = await _work.GenericRepository<Factor>().TableNoTracking
                    .CountAsync(),
                Second = await _work.GenericRepository<Factor>().TableNoTracking
                    .CountAsync(x => x.Status == Status.Field),
            };
            ViewBag.Tax = await _work.GenericRepository<Factor>().TableNoTracking.SumAsync(x => x.Amount) * 0.1;
            ViewBag.profit = await _work.GenericRepository<Factor>().TableNoTracking.SumAsync(x => x.Amount) * 0.23;
            ViewBag.SaleMonth = await _work.GenericRepository<Factor>().TableNoTracking
                .Where(x => x.InsertDate.Month == DateTime.Now.Month).SumAsync(x => x.Amount);
            ViewBag.Sale = await _work.GenericRepository<Factor>().TableNoTracking.SumAsync(x => x.Amount);

            #endregion

            return View();
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> ManageDiscount(string search)
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            if (!string.IsNullOrWhiteSpace(search))
            {
                ViewBag.DiscountCode = await _work.GenericRepository<DiscountCode>().TableNoTracking
                    .Where(x => x.Code.Contains(search))
                    .OrderByDescending(x => x.Id)
                    .ToListAsync();
            }
            else
            {
                ViewBag.DiscountCode = await _work.GenericRepository<DiscountCode>().TableNoTracking
                    .OrderByDescending(x => x.Id)
                    .ToListAsync();
            }

            #endregion

            return View();
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> ManageFactor(string search, int type = 5)
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            ViewBag.FactorsT = await _work.GenericRepository<Factor>().TableNoTracking.ToListAsync();
            if (!string.IsNullOrWhiteSpace(search))
            {
                switch (type)
                {
                    case 0:
                        ViewBag.Factors = await _work.GenericRepository<Factor>().TableNoTracking
                            .Include(x => x.User)
                            .Include(x => x.PostMethod)
                            .Include(x => x.UserAddress)
                            .Include(x => x.Products)
                            .ThenInclude(x => x.FactorProductColor)
                            .Where(x => x.DiscountCode.Contains(search) ||
                                        x.Desc.Contains(search) ||
                                        x.FactorCode.Contains(search) ||
                                        x.User.PhoneNumber.Contains(search) ||
                                        x.User.Name.Contains(search) ||
                                        x.User.Family.Contains(search) ||
                                        x.EconomicNumber.Contains(search) ||
                                        x.OrganizationName.Contains(search) ||
                                        x.NationalId.Contains(search) ||
                                        x.PostCode.Contains(search) ||
                                        x.OrganizationNumber.Contains(search) ||
                                        x.RegistrationNumber.Contains(search) ||
                                        x.RecipientName.Contains(search) || // اضافه کردن RecipientName به جستجو
                                        x.Adders.Contains(search)) // اضافه کردن Adders به جستجو
                            .Where(x => x.InsertDate.Month == DateTime.Now.Month)
                            .OrderByDescending(x => x.InsertDate)
                            .ToListAsync();
                        break;
                    case 1:
                        ViewBag.Factors = await _work.GenericRepository<Factor>().TableNoTracking
                            .Include(x => x.User)
                            .Include(x => x.PostMethod)
                            .Include(x => x.UserAddress)
                            .Include(x => x.Products)
                            .ThenInclude(x => x.FactorProductColor)
                            .Where(x => x.DiscountCode.Contains(search) ||
                                        x.Desc.Contains(search) ||
                                        x.FactorCode.Contains(search) ||
                                        x.User.PhoneNumber.Contains(search) ||
                                        x.User.Name.Contains(search) ||
                                        x.User.Family.Contains(search) ||
                                        x.EconomicNumber.Contains(search) ||
                                        x.OrganizationName.Contains(search) ||
                                        x.NationalId.Contains(search) ||
                                        x.PostCode.Contains(search) ||
                                        x.OrganizationNumber.Contains(search) ||
                                        x.RegistrationNumber.Contains(search) ||
                                        x.RecipientName.Contains(search) || // اضافه کردن RecipientName به جستجو
                                        x.Adders.Contains(search)) // اضافه کردن Adders به جستجو
                            .Where(x => x.InsertDate.Month == DateTime.Now.Month && x.Status == Status.Accepted)
                            .OrderByDescending(x => x.InsertDate)
                            .ToListAsync();
                        break;
                    case 2:
                        ViewBag.Factors = await _work.GenericRepository<Factor>().TableNoTracking
                            .Include(x => x.User)
                            .Include(x => x.PostMethod)
                            .Include(x => x.UserAddress)
                            .Include(x => x.Products)
                            .ThenInclude(x => x.FactorProductColor)
                            .Where(x => x.DiscountCode.Contains(search) ||
                                        x.Desc.Contains(search) ||
                                        x.FactorCode.Contains(search) ||
                                        x.User.PhoneNumber.Contains(search) ||
                                        x.User.Name.Contains(search) ||
                                        x.User.Family.Contains(search) ||
                                        x.EconomicNumber.Contains(search) ||
                                        x.OrganizationName.Contains(search) ||
                                        x.NationalId.Contains(search) ||
                                        x.PostCode.Contains(search) ||
                                        x.OrganizationNumber.Contains(search) ||
                                        x.RegistrationNumber.Contains(search) ||
                                        x.RecipientName.Contains(search) || // اضافه کردن RecipientName به جستجو
                                        x.Adders.Contains(search)) // اضافه کردن Adders به جستجو
                            .Where(x => x.InsertDate.Day == DateTime.Now.Day)
                            .OrderByDescending(x => x.InsertDate)
                            .ToListAsync();
                        break;
                    case 3:
                        ViewBag.Factors = await _work.GenericRepository<Factor>().TableNoTracking
                            .Include(x => x.User)
                            .Include(x => x.PostMethod)
                            .Include(x => x.UserAddress)
                            .Include(x => x.Products)
                            .ThenInclude(x => x.FactorProductColor)
                            .Where(x => x.DiscountCode.Contains(search) ||
                                        x.Desc.Contains(search) ||
                                        x.FactorCode.Contains(search) ||
                                        x.User.PhoneNumber.Contains(search) ||
                                        x.User.Name.Contains(search) ||
                                        x.User.Family.Contains(search) ||
                                        x.EconomicNumber.Contains(search) ||
                                        x.OrganizationName.Contains(search) ||
                                        x.NationalId.Contains(search) ||
                                        x.PostCode.Contains(search) ||
                                        x.OrganizationNumber.Contains(search) ||
                                        x.RegistrationNumber.Contains(search) ||
                                        x.RecipientName.Contains(search) || // اضافه کردن RecipientName به جستجو
                                        x.Adders.Contains(search)) // اضافه کردن Adders به جستجو
                            .Where(x => x.IsReturned)
                            .OrderByDescending(x => x.InsertDate)
                            .ToListAsync();
                        break;
                    case 5:
                        ViewBag.Factors = await _work.GenericRepository<Factor>().TableNoTracking
                            .Include(x => x.User)
                            .Include(x => x.PostMethod)
                            .Include(x => x.UserAddress)
                            .Include(x => x.Products)
                            .ThenInclude(x => x.FactorProductColor)
                            .Where(x => x.DiscountCode.Contains(search) ||
                                        x.Desc.Contains(search) ||
                                        x.FactorCode.Contains(search) ||
                                        x.User.PhoneNumber.Contains(search) ||
                                        x.User.Name.Contains(search) ||
                                        x.User.Family.Contains(search) ||
                                        x.EconomicNumber.Contains(search) ||
                                        x.OrganizationName.Contains(search) ||
                                        x.NationalId.Contains(search) ||
                                        x.PostCode.Contains(search) ||
                                        x.OrganizationNumber.Contains(search) ||
                                        x.RegistrationNumber.Contains(search) ||
                                        x.RecipientName.Contains(search) || // اضافه کردن RecipientName به جستجو
                                        x.Adders.Contains(search)) // اضافه کردن Adders به جستجو
                            .OrderByDescending(x => x.InsertDate)
                            .ToListAsync();
                        break;
                }
            }
            else
            {
                switch (type)
                {
                    case 0:
                        ViewBag.Factors = await _work.GenericRepository<Factor>().TableNoTracking
                            .Include(x => x.User)
                            .Include(x => x.PostMethod)
                            .Include(x => x.UserAddress)
                            .Include(x => x.Products)
                            .ThenInclude(x => x.FactorProductColor)
                            .Where(x => x.InsertDate.Month == DateTime.Now.Month)
                            .OrderByDescending(x => x.InsertDate)
                            .ToListAsync();
                        break;
                    case 1:
                        ViewBag.Factors = await _work.GenericRepository<Factor>().TableNoTracking
                            .Include(x => x.User)
                            .Include(x => x.PostMethod)
                            .Include(x => x.UserAddress)
                            .Include(x => x.Products)
                            .ThenInclude(x => x.FactorProductColor)
                            .Where(x => x.InsertDate.Month == DateTime.Now.Month && x.Status == Status.Accepted)
                            .OrderByDescending(x => x.InsertDate)
                            .ToListAsync();
                        break;
                    case 2:
                        ViewBag.Factors = await _work.GenericRepository<Factor>().TableNoTracking
                            .Include(x => x.User)
                            .Include(x => x.PostMethod)
                            .Include(x => x.UserAddress)
                            .Include(x => x.Products)
                            .ThenInclude(x => x.FactorProductColor)
                            .Where(x => x.InsertDate.Day == DateTime.Now.Day)
                            .OrderByDescending(x => x.InsertDate)
                            .ToListAsync();
                        break;
                    case 3:
                        ViewBag.Factors = await _work.GenericRepository<Factor>().TableNoTracking
                            .Include(x => x.User)
                            .Include(x => x.PostMethod)
                            .Include(x => x.UserAddress)
                            .Include(x => x.Products)
                            .ThenInclude(x => x.FactorProductColor)
                            .Where(x => x.IsReturned)
                            .OrderByDescending(x => x.InsertDate)
                            .ToListAsync();
                        break;
                    case 5:
                        ViewBag.Factors = await _work.GenericRepository<Factor>().TableNoTracking
                            .Include(x => x.User)
                            .Include(x => x.PostMethod)
                            .Include(x => x.UserAddress)
                            .Include(x => x.Products)
                            .ThenInclude(x => x.FactorProductColor)
                            .OrderByDescending(x => x.InsertDate)
                            .ToListAsync();
                        break;
                }
            }

            #endregion

            return View();
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<IActionResult> GetSalesData()
    {
        var oneMonthAgo = DateTime.Now.AddMonths(-1);
        var salesData = await _work.GenericRepository<FactorProduct>()
            .TableNoTracking
            .Where(x => x.Factor.InsertDate >= oneMonthAgo)
            .GroupBy(x => x.Brand)
            .Select(g => new
            {
                label = g.FirstOrDefault().Brand ?? "نا مشخص", // استفاده از Brand
                value = g.Count()
            })
            .OrderByDescending(x => x.value) // مرتب‌سازی بر اساس تعداد فروش
            .Take(7) // محدود به 7 مورد
            .ToListAsync();


        return Ok(salesData);
    }

    public async Task<IActionResult> GetDailySalesData()
    {
        var endDate = DateTime.Now;
        var startDate = endDate.AddMonths(-1);
        ViewBag.endDate = endDate;
        ViewBag.startDate = startDate;

        var salesData = await _work.GenericRepository<FactorProduct>()
            .TableNoTracking
            .Where(x => x.Factor.InsertDate >= startDate && x.Factor.InsertDate <= endDate)
            .Select(x => new
            {
                Day = x.Factor.InsertDate.Date,
                Status = x.Factor.Status,
                FactorProductColors = x.FactorProductColor
            })
            .ToListAsync();

        // تجمیع داده‌ها برای سفارش‌های موفق
        var dailySuccessfulSales = Enumerable.Range(1, DateTime.DaysInMonth(startDate.Year, startDate.Month))
            .Select(day => new
            {
                Day = day,
                Count = salesData
                    .Where(s => s.Day.Day == day && s.Status != Status.Field && s.Status != Status.Rejected)
                    .Sum(s => s.FactorProductColors.Sum(c => c.Count))
            })
            .ToList();

        // تجمیع داده‌ها برای سفارش‌های ناموفق
        var dailyFailedSales = Enumerable.Range(1, DateTime.DaysInMonth(startDate.Year, startDate.Month))
            .Select(day => new
            {
                Day = day,
                Count = salesData
                    .Where(s => s.Day.Day == day && (s.Status == Status.Field || s.Status == Status.Rejected))
                    .Sum(s => s.FactorProductColors.Sum(c => c.Count))
            })
            .ToList();

        // بازگشت دو مجموعه داده
        return Ok(new
        {
            SuccessfulSales = dailySuccessfulSales,
            FailedSales = dailyFailedSales
        });
    }


    public async Task<ActionResult> SalesInvoice(string search)
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            if (!string.IsNullOrWhiteSpace(search))
            {
                ViewBag.Factors = await _work.GenericRepository<Factor>().TableNoTracking
                    .Include(x => x.User)
                    .Include(x => x.PostMethod)
                    .Include(x => x.UserAddress)
                    .Include(x => x.Products)
                    .ThenInclude(x => x.FactorProductColor)
                    .Where(x => x.DiscountCode.Contains(search) ||
                                x.Desc.Contains(search) ||
                                x.FactorCode.Contains(search) ||
                                x.User.PhoneNumber.Contains(search) ||
                                x.User.Name.Contains(search) ||
                                x.User.Family.Contains(search) ||
                                x.EconomicNumber.Contains(search) ||
                                x.OrganizationName.Contains(search) ||
                                x.NationalId.Contains(search) ||
                                x.PostCode.Contains(search) ||
                                x.OrganizationNumber.Contains(search) ||
                                x.RegistrationNumber.Contains(search) ||
                                x.RecipientName.Contains(search) || // اضافه کردن RecipientName به جستجو
                                x.Adders.Contains(search)) // اضافه کردن Adders به جستجو
                    .OrderByDescending(x => x.InsertDate)
                    .ToListAsync();
            }
            else
            {
                ViewBag.Factors = await _work.GenericRepository<Factor>().TableNoTracking
                    .Include(x => x.User)
                    .Include(x => x.PostMethod)
                    .Include(x => x.UserAddress)
                    .Include(x => x.Products)
                    .ThenInclude(x => x.FactorProductColor)
                    .OrderByDescending(x => x.InsertDate)
                    .ToListAsync();
            }

            #endregion


            return View();
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> ManageReturnedFactor(string search)
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            if (!string.IsNullOrWhiteSpace(search))
            {
                ViewBag.Factors =
                    await _work.GenericRepository<ReturnedFactor>().TableNoTracking
                        .Include(x => x.Factor).ThenInclude(x => x.User)
                        .Include(x => x.Factor).ThenInclude(x => x.PostMethod)
                        .Include(x => x.Factor).ThenInclude(x => x.UserAddress).ThenInclude(q => q.City)
                        .Include(x => x.Factor).ThenInclude(x => x.Products)
                        .ThenInclude(x => x.FactorProductColor)
                        .Include(x => x.Factor).ThenInclude(x => x.Products)
                        .ThenInclude(x => x.FactorProductColor)
                        .Where(x => x.Factor.DiscountCode.Contains(search) ||
                                    x.Desc.Contains(search) ||
                                    x.Factor.FactorCode.Contains(search) ||
                                    x.Factor.User.PhoneNumber.Contains(search) ||
                                    x.Factor.User.Name.Contains(search) ||
                                    x.Factor.User.Family.Contains(search) ||
                                    x.Factor.EconomicNumber.Contains(search) ||
                                    x.Factor.OrganizationName.Contains(search) ||
                                    x.Factor.NationalId.Contains(search) ||
                                    x.Factor.PostCode.Contains(search) ||
                                    x.Factor.OrganizationNumber.Contains(search) ||
                                    x.Factor.RegistrationNumber.Contains(search) ||
                                    x.Factor.RecipientName.Contains(search) || // اضافه کردن RecipientName به جستجو
                                    x.Factor.Adders.Contains(search)) // اضافه کردن Adders به جستجو
                        .OrderByDescending(x => x.InsertDate)
                        .ToListAsync();
            }
            else
            {
                ViewBag.Factors = await _work.GenericRepository<ReturnedFactor>().TableNoTracking
                    .Include(x => x.Factor).ThenInclude(x => x.User)
                    .Include(x => x.Factor).ThenInclude(x => x.PostMethod)
                    .Include(x => x.Factor).ThenInclude(x => x.UserAddress).ThenInclude(q => q.City)
                    .Include(x => x.Factor).ThenInclude(x => x.Products)
                    .ThenInclude(x => x.FactorProductColor)
                    .Include(x => x.Factor).ThenInclude(x => x.Products)
                    .ThenInclude(x => x.FactorProductColor)
                    .OrderByDescending(x => x.InsertDate)
                    .ToListAsync();
            }

            #endregion

            return View();
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> IncomeReport(string search)
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            if (!string.IsNullOrWhiteSpace(search))
            {
                ViewBag.Factors =
                    await _work.GenericRepository<Factor>().TableNoTracking
                        .Include(x => x.User)
                        .Include(x => x.PostMethod)
                        .Include(x => x.UserAddress)
                        .Include(x => x.Products)
                        .ThenInclude(x => x.FactorProductColor)
                        .Where(x => x.DiscountCode.Contains(search) || x.Desc.Contains(search) ||
                                    x.FactorCode.Contains(search))
                        .OrderByDescending(x => x.InsertDate)
                        .ToListAsync();
            }
            else
            {
                ViewBag.Factors = await _work.GenericRepository<Factor>().TableNoTracking
                    .Include(x => x.User)
                    .Include(x => x.PostMethod)
                    .Include(x => x.UserAddress)
                    .Include(x => x.Products)
                    .ThenInclude(x => x.FactorProductColor)
                    .OrderByDescending(x => x.InsertDate)
                    .ToListAsync();
            }

            #endregion

            return View();
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> LandingDetail(int id)
    {
        if (User.Identity.IsAuthenticated)
        {
            ViewBag.Category = await _work.GenericRepository<BrandTag>().TableNoTracking
                .FirstOrDefaultAsync(x => x.Id == id);
            ViewBag.Factors = await _work.GenericRepository<BrandLanding>().TableNoTracking
                .Include(x => x.BrandTag)
                .FirstOrDefaultAsync(x => x.BrandTagId == id) ?? new();
            return View();
        }
        else
        {
            return RedirectToAction("Login");
        }
    }

    public async Task<ActionResult> SeoIndexPage()
    {
        if (User.Identity.IsAuthenticated)
        {
            ViewBag.Index = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                            new SeoPage();

            return View();
        }
        else
        {
            return RedirectToAction("Login");
        }
    }

    public async Task<ActionResult> SeoFooterPage()
    {
        if (User.Identity.IsAuthenticated)
        {
            ViewBag.Footer = await _work.GenericRepository<FooterPage>().TableNoTracking.FirstOrDefaultAsync() ??
                             new FooterPage();

            return View();
        }
        else
        {
            return RedirectToAction("Login");
        }
    }

    public async Task<IActionResult> UpdateLanding(BrandLandingDto request)
    {
        Upload up = new Upload(_webHostEnvironment);

        if (request.Id <= 0)
        {
            var entity = _mapper!.Map<BrandLanding>(request);
            entity.ImageSlider = up.Uploadfile(request.ImageSlider, "Category");
            entity.ImageSlider2 = up.Uploadfile(request.ImageSlider2, "Category");
            entity.ImageSlider3 = up.Uploadfile(request.ImageSlider3, "Category");
            entity.ImageSlider4 = up.Uploadfile(request.ImageSlider4, "Category");
            entity.ImageSlider5 = up.Uploadfile(request.ImageSlider5, "Category");

            entity.BigBanner = up.Uploadfile(request.BigBanner, "Banner");

            entity.SmallBanner1 = up.Uploadfile(request.SmallBanner1, "Banner");
            entity.SmallBanner2 = up.Uploadfile(request.SmallBanner2, "Banner");
            entity.SmallBanner3 = up.Uploadfile(request.SmallBanner3, "Banner");
            entity.SmallBanner4 = up.Uploadfile(request.SmallBanner4, "Banner");

            await _work.GenericRepository<BrandLanding>()
                .AddAsync(entity, CancellationToken.None);
        }
        else
        {
            var footer = await _work.GenericRepository<BrandLanding>().TableNoTracking
                .FirstOrDefaultAsync(x => x.BrandTagId == request.BrandTagId);
            var entity = _mapper!.Map<BrandLanding>(request);
            entity.Id = footer.Id;
            entity.ImageSlider = request.ImageSlider != null
                ? up.Uploadfile(request.ImageSlider, "Category")
                : footer.ImageSlider;

            entity.ImageSlider2 = request.ImageSlider2 != null
                ? up.Uploadfile(request.ImageSlider2, "Category")
                : footer.ImageSlider2;
            entity.ImageSlider3 = request.ImageSlider3 != null
                ? up.Uploadfile(request.ImageSlider3, "Category")
                : footer.ImageSlider3;
            entity.ImageSlider4 = request.ImageSlider4 != null
                ? up.Uploadfile(request.ImageSlider4, "Category")
                : footer.ImageSlider4;
            entity.ImageSlider5 = request.ImageSlider5 != null
                ? up.Uploadfile(request.ImageSlider5, "Category")
                : footer.ImageSlider5;

            entity.BigBanner = request.BigBanner != null
                ? up.Uploadfile(request.BigBanner, "Banner")
                : footer.BigBanner;
            entity.SmallBanner1 = request.SmallBanner1 != null
                ? up.Uploadfile(request.SmallBanner1, "Banner")
                : footer.SmallBanner1;
            entity.SmallBanner2 = request.SmallBanner2 != null
                ? up.Uploadfile(request.SmallBanner2, "Banner")
                : footer.SmallBanner2;
            entity.SmallBanner3 = request.SmallBanner3 != null
                ? up.Uploadfile(request.SmallBanner3, "Banner")
                : footer.SmallBanner3;
            entity.SmallBanner4 = request.SmallBanner4 != null
                ? up.Uploadfile(request.SmallBanner4, "Banner")
                : footer.SmallBanner4;
            await _work.GenericRepository<BrandLanding>().UpdateAsync(entity, CancellationToken.None);
        }

        return RedirectToAction("ManageBrand");
    }

    public async Task<ActionResult> FactorDetail(int id)
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            ViewBag.Factors = await _work.GenericRepository<Factor>().TableNoTracking
                .Include(x => x.User)
                .Include(x => x.PostMethod)
                .Include(x => x.UserAddress).ThenInclude(q => q.City)
                .Include(x => x.Products)
                .ThenInclude(x => x.FactorProductColor)
                .Include(x => x.Products)
                .ThenInclude(x => x.FactorProductColor)
                .FirstOrDefaultAsync(x => x.Id == id);
            ViewBag.Logs = await _work.GenericRepository<LogFactor>().TableNoTracking.Include(x => x.User)
                .Where(x => x.FactorId == id).ToListAsync();

            #endregion

            return View();
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> ManageContactUs()
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            ViewBag.Contact = await _work.GenericRepository<ContactPage>().TableNoTracking.FirstOrDefaultAsync() ??
                              new ContactPage();

            #endregion

            return View();
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> ManageAboutUs()
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            ViewBag.Contact = await _work.GenericRepository<AboutUsPage>().TableNoTracking.FirstOrDefaultAsync() ??
                              new AboutUsPage();

            #endregion

            return View();
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> InsertContactUs(string desc, IFormFile Image, string SeoTitle, string SeoDesc,
        string SeoCanonical)
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            Upload up = new Upload(_webHostEnvironment);
            var result = await _work.GenericRepository<ContactPage>().Table.FirstOrDefaultAsync();
            if (result != null)
            {
                result.Desc = desc;
                result.Image = Image != null
                    ? up.Uploadfile(Image, "Pages")
                    : result.Image;
                result.SeoTitle = SeoTitle ?? string.Empty;
                result.SeoCanonical = SeoCanonical ?? string.Empty;
                result.SeoDesc = SeoDesc ?? string.Empty;
                await _work.GenericRepository<ContactPage>().UpdateAsync(result, CancellationToken.None);
            }
            else
            {
                await _work.GenericRepository<ContactPage>().AddAsync(new ContactPage
                {
                    Desc = desc,
                    SeoTitle = SeoTitle,
                    Image = up.Uploadfile(Image, "Pages"),
                    SeoCanonical = SeoCanonical,
                    SeoDesc = SeoDesc
                }, CancellationToken.None);
            }

            #endregion

            return RedirectToAction("ManageContactUs");
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> InsertAboutUs(string head, string body, IFormFile Image, string SeoTitle,
        string SeoDesc, string SeoCanonical)
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            Upload up = new Upload(_webHostEnvironment);
            var result = await _work.GenericRepository<AboutUsPage>().Table.FirstOrDefaultAsync();
            if (result != null)
            {
                result.Body = body ?? string.Empty;
                result.Head = head ?? string.Empty;
                result.Image = Image != null
                    ? up.Uploadfile(Image, "Pages")
                    : result.Image;
                result.SeoTitle = SeoTitle ?? string.Empty;
                result.SeoCanonical = SeoCanonical ?? string.Empty;
                result.SeoDesc = SeoDesc ?? string.Empty;
                await _work.GenericRepository<AboutUsPage>().UpdateAsync(result, CancellationToken.None);
            }
            else
            {
                await _work.GenericRepository<AboutUsPage>().AddAsync(new AboutUsPage
                {
                    Body = body ?? string.Empty,
                    Head = head ?? string.Empty,
                    SeoTitle = SeoTitle ?? string.Empty,
                    Image = up.Uploadfile(Image, "Pages"),
                    SeoCanonical = SeoCanonical ?? string.Empty,
                    SeoDesc = SeoDesc ?? string.Empty,
                }, CancellationToken.None);
            }

            #endregion

            return RedirectToAction("ManageAboutUs");
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> Invoice(int id)
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            ViewBag.Factor = await _work.GenericRepository<Factor>().TableNoTracking
                .Include(x => x.User)
                .Include(x => x.PostMethod)
                .Include(x => x.UserAddress).ThenInclude(q => q.City)
                .Include(x => x.Products)
                .ThenInclude(x => x.FactorProductColor)
                .Include(x => x.Products)
                .ThenInclude(x => x.FactorProductColor)
                .FirstOrDefaultAsync(x => x.Id == id);
            ViewBag.Logs = await _work.GenericRepository<LogFactor>().TableNoTracking.Include(x => x.User)
                .Where(x => x.FactorId == id).ToListAsync();

            #endregion

            return View();
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> ReturnedFactorDetail(int id)
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            ViewBag.Factors = await _work.GenericRepository<ReturnedFactor>().TableNoTracking
                .Include(x => x.Factor).ThenInclude(x => x.User)
                .Include(x => x.Factor).ThenInclude(x => x.PostMethod)
                .Include(x => x.Factor).ThenInclude(x => x.UserAddress).ThenInclude(q => q.City)
                .Include(x => x.Factor).ThenInclude(x => x.Products)
                .ThenInclude(x => x.FactorProductColor)
                .Include(x => x.Factor).ThenInclude(x => x.Products)
                .ThenInclude(x => x.FactorProductColor)
                .FirstOrDefaultAsync(x => x.Id == id);
            ViewBag.Logs = await _work.GenericRepository<LogReturnedFactor>().TableNoTracking.Include(x => x.User)
                .Where(x => x.ReturnedFactorId == id).ToListAsync();

            #endregion

            return View();
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> ChangeStatus(int id, string desc, int status, string? refPost, int rejectType = 0)
    {
        if (User.Identity.IsAuthenticated)
        {
            var factor = await _work.GenericRepository<Factor>().Table.Include(x => x.User)
                .Include(x => x.PostMethod)
                .FirstOrDefaultAsync(x => x.Id == id);
            factor.Status = (Status)status;
            factor.RejectStatus = (RejectStatus)rejectType;
            factor.RefPostUrl = refPost ?? string.Empty;
            var admin = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
            await _work.GenericRepository<LogFactor>().AddAsync(new LogFactor
            {
                InsertDate = DateTime.Now,
                UserId = admin.Id,
                FactorId = factor.Id,
                Desc = desc,
            }, CancellationToken.None);
            await _work.GenericRepository<Factor>().UpdateAsync(factor, CancellationToken.None);
            var command = new SendMessageUserCommand
            {
                Number = factor.User.PhoneNumber,
                Status = (Status)status,
                Token = string.Empty,
                Token2 = string.Empty,
                Token3 = string.Empty,
            };
            switch ((Status)status)
            {
                case Status.Accepted:
                    command.Token = factor.FactorCode;
                    command.Token2 = factor.PostMethod.Title;
                    break;
                case Status.Rejected:
                    command.Token = factor.RejectStatus.ToDisplay();
                    command.Token2 = factor.FactorCode;
                    break;
                case Status.Returned:
                    command.Token = factor.FactorCode;
                    break;
                case Status.ReadyToSend:
                    command.Token = factor.FactorCode;
                    break;
                case Status.ReadyToGet:
                    command.Token =
                        "تهران-میدان-ولیعصر-خیابان-ولیعصر-خیابان-ولدی-مرکز-کامپیوتر-ولیعصر-برج-اداری-شمالی-طبقه۶-واحد۳۳";
                    break;
                default:
                    break;
            }

            await _mediator.Send(command, CancellationToken.None);
            return RedirectToAction("FactorDetail", "Admin", new { factor.Id });
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> UpdateFeature(int id, string title, int Priority)
    {
        if (User.Identity.IsAuthenticated)
        {
            var result = await _work.GenericRepository<Feature>().Table.FirstOrDefaultAsync(x => x.Id == id);
            result.Title = title;
            result.Priority = Priority;
            await _work.GenericRepository<Feature>().UpdateAsync(result, CancellationToken.None);
            return RedirectToAction("ManageCatDetail", "Admin");
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> UpdateFooterSeo(FooterPageDto request)
    {
        if (User.Identity.IsAuthenticated)
        {
            Upload up = new Upload(_webHostEnvironment);
            var result = await _work.GenericRepository<FooterPage>().Table.FirstOrDefaultAsync();
            if (result == null)
            {
                await _work.GenericRepository<FooterPage>().AddAsync(new FooterPage
                {
                    WhyParsDesc = request.WhyParsDesc,
                    ParsGoalsDesc = request.ParsGoalsDesc,
                    ParsRegulationsDesc = request.ParsRegulationsDesc,
                    ParsWarrantyDesc = request.ParsWarrantyDesc,
                    ParsBuyingGuideDesc = request.ParsBuyingGuideDesc,
                    SeoParsProceduresForReturningGoodsTitle = request.SeoParsProceduresForReturningGoodsTitle,
                    SeoWhyParsTitle = request.SeoWhyParsTitle,
                    SeoWhyParsDesc = request.SeoWhyParsDesc,
                    SeoWhyParsCanonical = request.SeoWhyParsCanonical,
                    ParsDarYekDesc = request.ParsDarYekDesc,
                    SeoParsDarYekTitle = request.SeoParsDarYekTitle,
                    SeoParsDarYekDesc = request.SeoParsDarYekDesc,
                    SeoParsDarYekCanonical = request.SeoParsDarYekCanonical,
                    SeoParsGoalsTitle = request.SeoParsGoalsTitle,
                    SeoParsGoalsDesc = request.SeoParsGoalsDesc,
                    SeoParsGoalsCanonical = request.SeoParsGoalsCanonical,
                    ParsInstallmentPurchaseDesc = request.ParsInstallmentPurchaseDesc,
                    SeoParsInstallmentPurchaseTitle = request.SeoParsInstallmentPurchaseTitle,
                    SeoParsInstallmentPurchaseDesc = request.SeoParsInstallmentPurchaseDesc,
                    SeoParsInstallmentPurchaseCanonical = request.SeoParsInstallmentPurchaseCanonical,
                    SeoParsBuyingGuideTitle = request.SeoParsBuyingGuideTitle,
                    SeoParsBuyingGuideDesc = request.SeoParsBuyingGuideDesc,
                    SeoParsBuyingGuideCanonical = request.SeoParsBuyingGuideCanonical,
                    ParsOrganizationalPurchaseDesc = request.ParsOrganizationalPurchaseDesc,
                    SeoParsOrganizationalPurchaseTitle = request.SeoParsOrganizationalPurchaseTitle,
                    SeoParsOrganizationalPurchaseDesc = request.SeoParsOrganizationalPurchaseDesc,
                    SeoParsOrganizationalPurchaseCanonical = request.SeoParsOrganizationalPurchaseCanonical,
                    SeoParsWarrantyTitle = request.SeoParsWarrantyTitle,
                    SeoParsWarrantyDesc = request.SeoParsWarrantyDesc,
                    SeoParsWarrantyCanonical = request.SeoParsWarrantyCanonical,
                    ParsShippingMethodsDesc = request.ParsShippingMethodsDesc,
                    SeoParsShippingMethodsTitle = request.SeoParsShippingMethodsTitle,
                    SeoParsShippingMethodsDesc = request.SeoParsShippingMethodsDesc,
                    SeoParsShippingMethodsCanonical = request.SeoParsShippingMethodsCanonical,
                    ParsConsultationBeforePurchaseDesc = request.ParsConsultationBeforePurchaseDesc,
                    SeoParsConsultationBeforePurchaseTitle = request.SeoParsConsultationBeforePurchaseTitle,
                    SeoParsConsultationBeforePurchaseDesc = request.SeoParsConsultationBeforePurchaseDesc,
                    SeoParsConsultationBeforePurchaseCanonical = request.SeoParsConsultationBeforePurchaseCanonical,
                    ParsProceduresForReturningGoodsDesc = request.ParsProceduresForReturningGoodsDesc,
                    SeoParsProceduresForReturningGoodsCanonical = request.SeoParsProceduresForReturningGoodsCanonical,
                    SeoParsProceduresForReturningGoodsDesc = request.SeoParsProceduresForReturningGoodsDesc,
                    ParsTrackingOrdersDesc = request.ParsTrackingOrdersDesc,
                    SeoParsTrackingOrdersGoodsTitle = request.SeoParsTrackingOrdersGoodsTitle,
                    SeoParsTrackingOrdersGoodsDesc = request.SeoParsTrackingOrdersGoodsDesc,
                    SeoParsTrackingOrdersGoodsCanonical = request.SeoParsTrackingOrdersGoodsCanonical,
                    ParsOnlineSupportDesc = request.ParsOnlineSupportDesc,
                    SeoParsOnlineSupportTitle = request.SeoParsOnlineSupportTitle,
                    SeoParsOnlineSupportDesc = request.SeoParsOnlineSupportDesc,
                    SeoParsOnlineSupportCanonical = request.SeoParsOnlineSupportCanonical,
                    SeoParsRegulationsTitle = request.SeoParsRegulationsTitle,
                    SeoParsRegulationsDesc = request.SeoParsRegulationsDesc,
                    SeoParsRegulationsCanonical = request.SeoParsRegulationsCanonical,
                    ParsUserPrivacyDesc = request.ParsUserPrivacyDesc,
                    SeoParsUserPrivacyTitle = request.SeoParsUserPrivacyTitle,
                    SeoParsUserPrivacyDesc = request.SeoParsUserPrivacyDesc,
                    SeoParsUserPrivacyCanonical = request.SeoParsUserPrivacyCanonical,
                    ParsInstallmentPurchaseImage = up.Uploadfile(request.ParsInstallmentPurchaseImage, "Pages"),
                    ParsOnlineSupportDescImage = up.Uploadfile(request.ParsOnlineSupportDescImage, "Pages"),
                    ParsGoalsImage = up.Uploadfile(request.ParsGoalsImage, "Pages"),
                    ParsBuyingGuideImage = up.Uploadfile(request.ParsBuyingGuideImage, "Pages"),
                    ParsOrganizationalPurchaseImage = up.Uploadfile(request.ParsOrganizationalPurchaseImage, "Pages"),
                    ParsRegulationsDescImage = up.Uploadfile(request.ParsRegulationsDescImage, "Pages"),
                    ParsWarrantyDescImage = up.Uploadfile(request.ParsWarrantyDescImage, "Pages"),
                    SeoWhyParsImage = up.Uploadfile(request.SeoWhyParsImage, "Pages"),
                    ParsShippingMethodsDescImage = up.Uploadfile(request.ParsShippingMethodsDescImage, "Pages"),
                    ParsTrackingOrdersDescImage = up.Uploadfile(request.ParsTrackingOrdersDescImage, "Pages"),
                    ParsUserPrivacyDescImage = up.Uploadfile(request.ParsUserPrivacyDescImage, "Pages"),
                    SeoParsDarYekImage = up.Uploadfile(request.SeoParsDarYekImage, "Pages"),
                    ParsConsultationBeforePurchaseDescImage =
                        up.Uploadfile(request.ParsConsultationBeforePurchaseDescImage, "Pages"),
                    ParsProceduresForReturningGoodsDescImage =
                        up.Uploadfile(request.ParsProceduresForReturningGoodsDescImage, "Pages"),
                }, CancellationToken.None);
            }
            else
            {
                result.WhyParsDesc = request.WhyParsDesc;
                result.ParsGoalsDesc = request.ParsGoalsDesc;
                result.ParsRegulationsDesc = request.ParsRegulationsDesc;
                result.ParsWarrantyDesc = request.ParsWarrantyDesc;
                result.ParsBuyingGuideDesc = request.ParsBuyingGuideDesc;
                result.SeoParsProceduresForReturningGoodsTitle = request.SeoParsProceduresForReturningGoodsTitle;
                result.SeoWhyParsTitle = request.SeoWhyParsTitle;
                result.SeoWhyParsDesc = request.SeoWhyParsDesc;
                result.SeoWhyParsCanonical = request.SeoWhyParsCanonical;
                result.ParsDarYekDesc = request.ParsDarYekDesc;
                result.SeoParsDarYekTitle = request.SeoParsDarYekTitle;
                result.SeoParsDarYekDesc = request.SeoParsDarYekDesc;
                result.SeoParsDarYekCanonical = request.SeoParsDarYekCanonical;
                result.SeoParsGoalsTitle = request.SeoParsGoalsTitle;
                result.SeoParsGoalsDesc = request.SeoParsGoalsDesc;
                result.SeoParsGoalsCanonical = request.SeoParsGoalsCanonical;
                result.ParsInstallmentPurchaseDesc = request.ParsInstallmentPurchaseDesc;
                result.SeoParsInstallmentPurchaseTitle = request.SeoParsInstallmentPurchaseTitle;
                result.SeoParsInstallmentPurchaseDesc = request.SeoParsInstallmentPurchaseDesc;
                result.SeoParsInstallmentPurchaseCanonical = request.SeoParsInstallmentPurchaseCanonical;
                result.SeoParsBuyingGuideTitle = request.SeoParsBuyingGuideTitle;
                result.SeoParsBuyingGuideDesc = request.SeoParsBuyingGuideDesc;
                result.SeoParsBuyingGuideCanonical = request.SeoParsBuyingGuideCanonical;
                result.ParsOrganizationalPurchaseDesc = request.ParsOrganizationalPurchaseDesc;
                result.SeoParsOrganizationalPurchaseTitle = request.SeoParsOrganizationalPurchaseTitle;
                result.SeoParsOrganizationalPurchaseDesc = request.SeoParsOrganizationalPurchaseDesc;
                result.SeoParsOrganizationalPurchaseCanonical = request.SeoParsOrganizationalPurchaseCanonical;
                result.SeoParsWarrantyTitle = request.SeoParsWarrantyTitle;
                result.SeoParsWarrantyDesc = request.SeoParsWarrantyDesc;
                result.SeoParsWarrantyCanonical = request.SeoParsWarrantyCanonical;
                result.ParsShippingMethodsDesc = request.ParsShippingMethodsDesc;
                result.SeoParsShippingMethodsTitle = request.SeoParsShippingMethodsTitle;
                result.SeoParsShippingMethodsDesc = request.SeoParsShippingMethodsDesc;
                result.SeoParsShippingMethodsCanonical = request.SeoParsShippingMethodsCanonical;
                result.ParsConsultationBeforePurchaseDesc = request.ParsConsultationBeforePurchaseDesc;
                result.SeoParsConsultationBeforePurchaseTitle = request.SeoParsConsultationBeforePurchaseTitle;
                result.SeoParsConsultationBeforePurchaseDesc = request.SeoParsConsultationBeforePurchaseDesc;
                result.SeoParsConsultationBeforePurchaseCanonical = request.SeoParsConsultationBeforePurchaseCanonical;
                result.ParsProceduresForReturningGoodsDesc = request.ParsProceduresForReturningGoodsDesc;
                result.SeoParsProceduresForReturningGoodsCanonical =
                    request.SeoParsProceduresForReturningGoodsCanonical;
                result.SeoParsProceduresForReturningGoodsDesc = request.SeoParsProceduresForReturningGoodsDesc;
                result.ParsTrackingOrdersDesc = request.ParsTrackingOrdersDesc;
                result.SeoParsTrackingOrdersGoodsTitle = request.SeoParsTrackingOrdersGoodsTitle;
                result.SeoParsTrackingOrdersGoodsDesc = request.SeoParsTrackingOrdersGoodsDesc;
                result.SeoParsTrackingOrdersGoodsCanonical = request.SeoParsTrackingOrdersGoodsCanonical;
                result.ParsOnlineSupportDesc = request.ParsOnlineSupportDesc;
                result.SeoParsOnlineSupportTitle = request.SeoParsOnlineSupportTitle;
                result.SeoParsOnlineSupportDesc = request.SeoParsOnlineSupportDesc;
                result.SeoParsOnlineSupportCanonical = request.SeoParsOnlineSupportCanonical;
                result.SeoParsRegulationsTitle = request.SeoParsRegulationsTitle;
                result.SeoParsRegulationsDesc = request.SeoParsRegulationsDesc;
                result.SeoParsRegulationsCanonical = request.SeoParsRegulationsCanonical;
                result.ParsUserPrivacyDesc = request.ParsUserPrivacyDesc;
                result.SeoParsUserPrivacyTitle = request.SeoParsUserPrivacyTitle;
                result.SeoParsUserPrivacyDesc = request.SeoParsUserPrivacyDesc;
                result.SeoParsUserPrivacyCanonical = request.SeoParsUserPrivacyCanonical;


                result.ParsInstallmentPurchaseImage = request.ParsInstallmentPurchaseImage != null
                    ? up.Uploadfile(request.ParsInstallmentPurchaseImage, "Pages")
                    : result.ParsInstallmentPurchaseImage;
                result.ParsOnlineSupportDescImage = request.ParsOnlineSupportDescImage != null
                    ? up.Uploadfile(request.ParsOnlineSupportDescImage, "Pages")
                    : result.ParsOnlineSupportDescImage;
                result.ParsGoalsImage = request.ParsGoalsImage != null
                    ? up.Uploadfile(request.ParsGoalsImage, "Pages")
                    : result.ParsGoalsImage;
                result.ParsBuyingGuideImage = request.ParsBuyingGuideImage != null
                    ? up.Uploadfile(request.ParsBuyingGuideImage, "Pages")
                    : result.ParsBuyingGuideImage;
                result.ParsOrganizationalPurchaseImage = request.ParsOrganizationalPurchaseImage != null
                    ? up.Uploadfile(request.ParsOrganizationalPurchaseImage, "Pages")
                    : result.ParsOrganizationalPurchaseImage;
                result.ParsRegulationsDescImage = request.ParsRegulationsDescImage != null
                    ? up.Uploadfile(request.ParsRegulationsDescImage, "Pages")
                    : result.ParsRegulationsDescImage;
                result.ParsWarrantyDescImage = request.ParsWarrantyDescImage != null
                    ? up.Uploadfile(request.ParsWarrantyDescImage, "Pages")
                    : result.ParsWarrantyDescImage;
                result.SeoWhyParsImage = request.SeoWhyParsImage != null
                    ? up.Uploadfile(request.SeoWhyParsImage, "Pages")
                    : result.SeoWhyParsImage;
                result.ParsShippingMethodsDescImage = request.ParsShippingMethodsDescImage != null
                    ? up.Uploadfile(request.ParsShippingMethodsDescImage, "Pages")
                    : result.ParsShippingMethodsDescImage;
                result.ParsTrackingOrdersDescImage = request.ParsTrackingOrdersDescImage != null
                    ? up.Uploadfile(request.ParsTrackingOrdersDescImage, "Pages")
                    : result.ParsTrackingOrdersDescImage;
                result.ParsUserPrivacyDescImage = request.ParsUserPrivacyDescImage != null
                    ? up.Uploadfile(request.ParsUserPrivacyDescImage, "Pages")
                    : result.ParsUserPrivacyDescImage;
                result.SeoParsDarYekImage = request.SeoParsDarYekImage != null
                    ? up.Uploadfile(request.SeoParsDarYekImage, "Pages")
                    : result.SeoParsDarYekImage;
                result.ParsConsultationBeforePurchaseDescImage = request.ParsConsultationBeforePurchaseDescImage != null
                    ? up.Uploadfile(request.ParsConsultationBeforePurchaseDescImage, "Pages")
                    : result.ParsConsultationBeforePurchaseDescImage;
                result.ParsProceduresForReturningGoodsDescImage =
                    request.ParsProceduresForReturningGoodsDescImage != null
                        ? up.Uploadfile(request.ParsProceduresForReturningGoodsDescImage, "Pages")
                        : result.ParsProceduresForReturningGoodsDescImage;

                await _work.GenericRepository<FooterPage>().UpdateAsync(result, CancellationToken.None);
            }

            return RedirectToAction("SeoFooterPage", "Admin");
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> UpdateIndexSeo(bool ShowTopBanner, string SeoIndexDesc, string SeoIndexCanonical,
        string SeoIndexTitle,
        IFormFile ImageUri, string TopBannerHref, string NavNameComp, string HeaderNumber, IFormFile HeaderLogo,
        string EmailComp, string ProductTitle, string ProductTitle2, string ProductTitle3, string ProductTitle4)
    {
        if (User.Identity.IsAuthenticated)
        {
            Upload up = new Upload(_webHostEnvironment);
            var result = await _work.GenericRepository<SeoPage>().Table.FirstOrDefaultAsync();
            if (result == null)
            {
                var img = string.Empty;
                if (ImageUri != null)
                {
                    img = up.Uploadfile(ImageUri, "Banner");
                }

                var logo = string.Empty;
                if (ImageUri != null)
                {
                    logo = up.Uploadfile(HeaderLogo, "Banner");
                }

                await _work.GenericRepository<SeoPage>().AddAsync(new SeoPage
                {
                    SeoIndexCanonical = SeoIndexCanonical,
                    SeoIndexDesc = SeoIndexDesc,
                    SeoIndexTitle = SeoIndexTitle,
                    TopBanner = img,
                    TopBannerHref = TopBannerHref,
                    NavNameComp = NavNameComp,
                    HeaderNumber = HeaderNumber,
                    HeaderLogo = logo,
                    ProductTitle4 = ProductTitle4,
                    EmailComp = EmailComp,
                    ProductTitle = ProductTitle,
                    ProductTitle2 = ProductTitle2,
                    ProductTitle3 = ProductTitle3,
                    ShowTopBanner = ShowTopBanner
                }, CancellationToken.None);
            }
            else
            {
                result.TopBanner = ImageUri != null
                    ? up.Uploadfile(ImageUri, "Banner")
                    : result.TopBanner;
                result.HeaderLogo = HeaderLogo != null
                    ? up.Uploadfile(HeaderLogo, "Banner")
                    : result.HeaderLogo;
                result.SeoIndexDesc = SeoIndexDesc;
                result.SeoIndexCanonical = SeoIndexCanonical;
                result.SeoIndexTitle = SeoIndexTitle;
                result.TopBannerHref = TopBannerHref;
                result.NavNameComp = NavNameComp;
                result.HeaderNumber = HeaderNumber;
                result.ProductTitle4 = ProductTitle4;
                result.EmailComp = EmailComp;
                result.ProductTitle = ProductTitle;
                result.ProductTitle2 = ProductTitle2;
                result.ProductTitle3 = ProductTitle3;
                result.ShowTopBanner = ShowTopBanner;
                await _work.GenericRepository<SeoPage>().UpdateAsync(result, CancellationToken.None);
            }

            return RedirectToAction("SeoIndexPage", "Admin");
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> ChangeReturnedStatus(int id, string desc, int status, int rejectType)
    {
        if (User.Identity.IsAuthenticated)
        {
            var factor = await _work.GenericRepository<ReturnedFactor>().Table.Include(x => x.Factor)
                .FirstOrDefaultAsync(x => x.Id == id);
            factor.ReturnedStatus = (ReturnedStatus)status;
            factor.RejectType = (RejectType)rejectType;
            var admin = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
            await _work.GenericRepository<LogReturnedFactor>().AddAsync(new LogReturnedFactor()
            {
                InsertDate = DateTime.Now,
                UserId = admin.Id,
                ReturnedFactorId = factor.Id,
                Desc = desc,
            }, CancellationToken.None);
            var command = new SendMessageUserCommand
            {
                Number = factor.Factor.User.PhoneNumber,
                ReturnedStatus = factor.ReturnedStatus,
                IsReturn = true,
            };
            switch ((ReturnedStatus)status)
            {
                case ReturnedStatus.Accepted:
                    command.Token = "d ";
                    break;
                case ReturnedStatus.Rejected:
                    command.Token = factor.RejectType.ToDisplay();
                    command.Token2 = factor.Factor.FactorCode;
                    break;
                case ReturnedStatus.Returned:
                    command.Token = factor.Factor.FactorCode;
                    break;
                default:
                    break;
            }

            await _work.GenericRepository<ReturnedFactor>().UpdateAsync(factor, CancellationToken.None);
            await _mediator.Send(command);
            return RedirectToAction("ReturnedFactorDetail", "Admin", new { factor.Id });
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> Brand(string search, bool isActiveSearch = true)
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            ViewBag.IsAct = isActiveSearch;
            var model = new CheckBoxHtml
            {
                IsActive = isActiveSearch
            };
            if (!string.IsNullOrWhiteSpace(search))
            {
                ViewBag.Brands = await _work.GenericRepository<Brand>().TableNoTracking
                    .Include(x => x.SubCategory)
                    .Include(x => x.Products)
                    .Where(x => x.Title.Contains(search) ||
                                x.SubCategory.Name.Contains(search))
                    .Where(x => x.IsActive == isActiveSearch)
                    .OrderByDescending(x => x.Id).ToListAsync();
            }
            else
            {
                ViewBag.Brands = await _work.GenericRepository<Brand>().TableNoTracking
                    .Include(x => x.SubCategory)
                    .Include(x => x.Products)
                    .Where(x => x.IsActive == isActiveSearch)
                    .OrderByDescending(x => x.Id).ToListAsync();
            }

            ViewBag.SubCats = await _work.GenericRepository<SubCategory>().TableNoTracking.OrderByDescending(x => x.Id)
                .ToListAsync();

            #endregion

            return View(model);
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> BrandTag(string search)
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            if (!string.IsNullOrWhiteSpace(search))
            {
                ViewBag.Brands = await _work.GenericRepository<BrandTag>().TableNoTracking
                    .Where(x => x.Title.Contains(search)).OrderByDescending(x => x.Id).ToListAsync();
            }
            else
            {
                ViewBag.Brands = await _work.GenericRepository<BrandTag>().TableNoTracking
                    .OrderByDescending(x => x.Id).ToListAsync();
            }

            ViewBag.SubCats = await _work.GenericRepository<SubCategory>().TableNoTracking.OrderByDescending(x => x.Id)
                .ToListAsync();

            #endregion

            return View();
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> UpdateGuarantee(int id, string title)
    {
        if (User.Identity.IsAuthenticated && id > 0)
        {
            var guarantee = await _work.GenericRepository<Guarantee>().Table.FirstOrDefaultAsync(x => x.Id == id);
            guarantee.Title = title;
            await _work.GenericRepository<Guarantee>().UpdateAsync(guarantee, CancellationToken.None);
            return RedirectToAction("ManageGuarantee");
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    public async Task<ActionResult> UpdateColor(int id, string title, string colorCode)
    {
        if (User.Identity.IsAuthenticated && id > 0)
        {
            var color = await _work.GenericRepository<Color>().Table.FirstOrDefaultAsync(x => x.Id == id);
            color.Title = title;
            color.ColorCode = colorCode;

            await _work.GenericRepository<Color>().UpdateAsync(color, CancellationToken.None);
            return RedirectToAction("Color");
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    public async Task<ActionResult> UpdatePostMethod(int id, string title, double price)
    {
        if (User.Identity.IsAuthenticated && id > 0)
        {
            var postMethod = await _work.GenericRepository<PostMethod>().Table.FirstOrDefaultAsync(x => x.Id == id);
            postMethod.Title = title;
            postMethod.Price = price;

            await _work.GenericRepository<PostMethod>().UpdateAsync(postMethod, CancellationToken.None);
            return RedirectToAction("ManagePostMethod");
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    public async Task<ActionResult> UpdateBrand(int id, string title, int subCategoryId, bool isActive2,
        string seoTitle,
        string seoDesc, string seoCanonical)
    {
        if (User.Identity.IsAuthenticated && id > 0 && subCategoryId > 0)
        {
            Upload up = new Upload(_webHostEnvironment);
            var brand = await _work.GenericRepository<Brand>().Table.FirstOrDefaultAsync(x => x.Id == id);
            brand.Title = title;
            brand.IsActive = isActive2;
            brand.SeoTitle = seoTitle;
            brand.SeoDesc = seoDesc;
            brand.SeoCanonical = seoCanonical;
            brand.SubCategoryId = subCategoryId;
            await _work.GenericRepository<Brand>().UpdateAsync(brand, CancellationToken.None);
            return RedirectToAction("Brand");
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    public async Task<ActionResult> UpdateBrandTag(int id, string title, IFormFile image, bool isClick2)
    {
        if (User.Identity.IsAuthenticated && id > 0)
        {
            Upload up = new Upload(_webHostEnvironment);
            var brand = await _work.GenericRepository<BrandTag>().Table.FirstOrDefaultAsync(x => x.Id == id);
            brand.Title = title;
            brand.IsClick = isClick2;
            brand.LogoUri = image != null
                ? up.Uploadfile(image, "Brand")
                : brand.LogoUri;
            await _work.GenericRepository<BrandTag>().UpdateAsync(brand, CancellationToken.None);
            return RedirectToAction("BrandTag");
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    public async Task<ActionResult> InsertBrand(string title, int subCatId, bool isActive, string seoTitle,
        string seoDesc, string seoCanonical)
    {
        if (User.Identity.IsAuthenticated)
        {
            Upload up = new Upload(_webHostEnvironment);
            var subCat = await _work.GenericRepository<SubCategory>().Table.FirstOrDefaultAsync(x => x.Id == subCatId);
            if (subCat == null) throw new Exception();
            await _work.GenericRepository<Brand>().AddAsync(new Brand
            {
                Title = title ?? string.Empty,
                LogoUri = string.Empty,
                SubCategoryId = subCat.Id,
                IsActive = isActive,
                SeoTitle = seoTitle,
                SeoCanonical = seoCanonical,
                SeoDesc = seoDesc,
            }, CancellationToken.None);
            return RedirectToAction("Brand");
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> InsertBrandTag(string title, IFormFile? logo, bool isClick)
    {
        if (User.Identity.IsAuthenticated)
        {
            Upload up = new Upload(_webHostEnvironment);
            var img = logo != null ? up.Uploadfile(logo, "Brand") : string.Empty;
            await _work.GenericRepository<BrandTag>().AddAsync(new BrandTag
            {
                Title = title ?? string.Empty,
                LogoUri = img,
                IsClick = isClick
            }, CancellationToken.None);
            return RedirectToAction("BrandTag");
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> UpdateCat(int id, string title, IFormFile imageCat, bool isActiveCat, int mainCatId)
    {
        if (User.Identity.IsAuthenticated)
        {
            Upload up = new Upload(_webHostEnvironment);
            var cat = await _work.GenericRepository<Category>().Table.FirstOrDefaultAsync(x => x.Id == id);
            cat.Name = title;
            cat.MainCategoryId = mainCatId;
            cat.IsActive = isActiveCat;
            cat.LogoUri = imageCat != null
                ? up.Uploadfile(imageCat, "Category")
                : cat.LogoUri;
            await _work.GenericRepository<Category>().UpdateAsync(cat, CancellationToken.None);
            return RedirectToAction("ManageCategory");
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> UpdateMainCat(int id, string title, IFormFile imageCat, bool isActiveMCat)
    {
        if (User.Identity.IsAuthenticated)
        {
            Upload up = new Upload(_webHostEnvironment);
            var cat = await _work.GenericRepository<MainCategory>().Table.FirstOrDefaultAsync(x => x.Id == id);
            cat.Name = title;
            cat.IsActive = isActiveMCat;
            cat.LogoUri = imageCat != null
                ? up.Uploadfile(imageCat, "Category")
                : cat.LogoUri;
            await _work.GenericRepository<MainCategory>().UpdateAsync(cat, CancellationToken.None);
            return RedirectToAction("ManageCategory");
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> ManageCatDetail(string search, int index, bool isActiveSearch = true,
        bool isActiveShow = true)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();
        }

        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            ViewBag.IsAct = isActiveSearch;
            ViewBag.IsActSh = isActiveShow;
            var model = new CheckBoxHtml
            {
                IsActive = isActiveSearch,
                IsActiveShow = isActiveShow
            };
            if (!string.IsNullOrWhiteSpace(search))
            {
                if (index == 0)
                {
                    int dataType = 1111;
                    if (search.Contains("متن کوتاه"))
                    {
                        dataType = 0;
                    }

                    if (search.Contains("چند گزینه ای"))
                    {
                        dataType = 1;
                    }

                    if (search.Contains("متن بلند"))
                    {
                        dataType = 2;
                    }

                    if (search.Contains("CheckBox"))
                    {
                        dataType = 3;
                    }

                    ViewBag.CatDetail = await _work.GenericRepository<CategoryDetail>().TableNoTracking
                        .Include(x => x.SubCategoryDetails).ThenInclude(x => x.SubCategory).ThenInclude(x => x.Category)
                        .Include(x => x.Feature)
                        .Where(x => x.Title.Contains(search) || x.Option.Contains(search))
                        .Where(x => x.IsActive == isActiveSearch && x.ShowInSearch == isActiveShow)
                        .OrderByDescending(x => x.Id).ToListAsync();
                    ViewBag.SubCat = await _work.GenericRepository<SubCategory>().TableNoTracking
                        .OrderByDescending(x => x.Id)
                        .ToListAsync();
                    ViewBag.Feature = await _work.GenericRepository<Feature>().TableNoTracking
                        .OrderByDescending(x => x.Id)
                        .ToListAsync();
                }
                else if (index == 1)
                {
                    ViewBag.CatDetail = await _work.GenericRepository<CategoryDetail>().TableNoTracking
                        .Include(x => x.SubCategoryDetails).ThenInclude(x => x.SubCategory).ThenInclude(x => x.Category)
                        .Include(x => x.Feature)
                        .Where(x => x.IsActive == isActiveSearch && x.ShowInSearch == isActiveShow)
                        .OrderByDescending(x => x.Id).ToListAsync();
                    ViewBag.SubCat = await _work.GenericRepository<SubCategory>().TableNoTracking
                        .OrderByDescending(x => x.Id)
                        .ToListAsync();
                    ViewBag.Feature = await _work.GenericRepository<Feature>().TableNoTracking
                        .Where(x => x.Title.Contains(search))
                        .OrderByDescending(x => x.Id)
                        .ToListAsync();
                }
            }
            else
            {
                ViewBag.CatDetail = await _work.GenericRepository<CategoryDetail>().TableNoTracking
                    .Include(x => x.SubCategoryDetails).ThenInclude(x => x.SubCategory).ThenInclude(x => x.Category)
                    .Include(x => x.Feature)
                    .Where(x => x.IsActive == isActiveSearch && x.ShowInSearch == isActiveShow)
                    .OrderByDescending(x => x.Id).ToListAsync();
                ViewBag.SubCat = await _work.GenericRepository<SubCategory>().TableNoTracking
                    .OrderByDescending(x => x.Id)
                    .ToListAsync();
                ViewBag.Feature = await _work.GenericRepository<Feature>().TableNoTracking.OrderByDescending(x => x.Id)
                    .ToListAsync();
            }

            #endregion

            return View(model);
        }
        else
        {
            return View("Login");
        }
    }


    public async Task<ActionResult> InsertDetailCat(string title, bool isActive, string? option, DataType dataType,
        List<int> subCatIds, int featureId, int Priority, bool isShow)
    {
        if (User.Identity.IsAuthenticated)
        {
            var feature = await _work.GenericRepository<Feature>().Table
                .FirstOrDefaultAsync(x => x.Id == featureId);

            var entity = new CategoryDetail
            {
                DataType = dataType,
                FeatureId = feature!.Id,
                ShowInSearch = isActive,
                Priority = Priority,
                Title = title,
                Option = option ?? string.Empty,
                IsActive = isShow
            };
            await _work.GenericRepository<CategoryDetail>().AddAsync(entity, CancellationToken.None);
            foreach (var i in subCatIds)
            {
                var sub = await _work.GenericRepository<SubCategory>().Table
                    .FirstOrDefaultAsync(x => x.Id == i);
                await _work.GenericRepository<SubCategoryDetail>().AddAsync(new SubCategoryDetail
                {
                    SubCategoryId = sub.Id,
                    CategoryDetailId = entity.Id
                }, CancellationToken.None);
            }

            return RedirectToAction("ManageCatDetail");
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> UpdateSubCat(int id, string title, int catId, bool isActiveSubCat, string seoTitle,
        string seoDesc, string seoCanonical)
    {
        if (User.Identity.IsAuthenticated && id > 0 && catId > 0)
        {
            var subCat = await _work.GenericRepository<SubCategory>().Table.FirstOrDefaultAsync(x => x.Id == id);
            subCat.Name = title;
            subCat.IsActive = isActiveSubCat;
            subCat.CategoryId = catId;
            subCat.SeoCanonical = seoCanonical;
            subCat.SeoDesc = seoDesc;
            subCat.SeoTitle = seoTitle;
            await _work.GenericRepository<SubCategory>().UpdateAsync(subCat, CancellationToken.None);
            return RedirectToAction("ManageCategory");
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    public async Task<ActionResult> InsertFeature(string title, int priority)
    {
        if (User.Identity.IsAuthenticated)
        {
            await _work.GenericRepository<Feature>().AddAsync(new Feature
            {
                Priority = priority,
                Title = title
            }, CancellationToken.None);
            return RedirectToAction("ManageCatDetail");
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> UpdateCategoryDetail(int id, string title, int featureId, bool isSearchCatDetail,
        string option, int dataType, List<int> subCatId, int Priority, bool isActive1)
    {
        if (User.Identity.IsAuthenticated)
        {
            var catDetail = await _work.GenericRepository<CategoryDetail>().Table.FirstOrDefaultAsync(x => x.Id == id);
            catDetail.DataType = (DataType)dataType;
            catDetail.Title = title;
            catDetail.Priority = Priority;
            catDetail.Option = option ?? string.Empty;
            catDetail.FeatureId = featureId;
            catDetail.ShowInSearch = isSearchCatDetail;
            catDetail.IsActive = isActive1;
            await _work.GenericRepository<CategoryDetail>().UpdateAsync(catDetail, CancellationToken.None);

            var sub = await _work.GenericRepository<SubCategoryDetail>().Table
                .Where(x => x.CategoryDetailId == catDetail.Id).ToListAsync();

            foreach (var i in sub)
            {
                var dd = await _work.GenericRepository<SubCategoryDetail>().Table
                    .FirstOrDefaultAsync(x => x.Id == i.Id);
                await _work.GenericRepository<SubCategoryDetail>().DeleteAsync(dd, CancellationToken.None);
            }

            foreach (var i in subCatId)
            {
                var sub1 = await _work.GenericRepository<SubCategory>().Table
                    .FirstOrDefaultAsync(x => x.Id == i);
                await _work.GenericRepository<SubCategoryDetail>().AddAsync(new SubCategoryDetail
                {
                    SubCategoryId = sub1.Id,
                    CategoryDetailId = catDetail.Id
                }, CancellationToken.None);
            }

            return RedirectToAction("ManageCatDetail");
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> ManageContact(string search)
    {
        if (User.Identity.IsAuthenticated)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                ViewBag.Contact = await _work.GenericRepository<ContactUs>().TableNoTracking
                    .Where(x => x.Name.Contains(search) || x.PhoneNumber.Contains(search) || x.Message.Contains(search))
                    .OrderByDescending(x => x.InsertDate).ToListAsync();
            }
            else
            {
                ViewBag.Contact = await _work.GenericRepository<ContactUs>().TableNoTracking
                    .OrderByDescending(x => x.InsertDate).ToListAsync();
            }

            return View();
        }
        else
        {
            return RedirectToAction("Login");
        }
    }

    public async Task<ActionResult> ManageCategory(string search, int index)
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            if (!string.IsNullOrWhiteSpace(search))
            {
                if (index == 0)
                {
                    ViewBag.Cats = await _work.GenericRepository<Category>().TableNoTracking
                        .Include(x => x.MainCategory)
                        .Where(x => x.Name.Contains(search)).OrderByDescending(x => x.Id)
                        .ToListAsync();
                    ViewBag.SubCats = await _work.GenericRepository<SubCategory>().TableNoTracking
                        .Include(x => x.Category)
                        .OrderByDescending(x => x.Id).ToListAsync();
                }
                else
                {
                    ViewBag.Cats = await _work.GenericRepository<Category>().TableNoTracking
                        .Include(x => x.MainCategory)
                        .OrderByDescending(x => x.Id)
                        .ToListAsync();
                    ViewBag.SubCats = await _work.GenericRepository<SubCategory>().TableNoTracking
                        .Include(x => x.Category)
                        .Where(x => x.Name.Contains(search))
                        .OrderByDescending(x => x.Id).ToListAsync();
                }
            }
            else
            {
                ViewBag.Cats = await _work.GenericRepository<Category>().TableNoTracking
                    .Include(x => x.MainCategory)
                    .OrderByDescending(x => x.Id)
                    .ToListAsync();
                ViewBag.SubCats = await _work.GenericRepository<SubCategory>().TableNoTracking.Include(x => x.Category)
                    .OrderByDescending(x => x.Id).ToListAsync();
            }

            ViewBag.MainCats = await _work.GenericRepository<MainCategory>().TableNoTracking.Include(x => x.Categories)
                .OrderByDescending(x => x.Id).ToListAsync();

            #endregion

            return View("ManageCategorey");
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> UpdateState(string title, int id)
    {
        if (User.Identity.IsAuthenticated)
        {
            var state = await _work.GenericRepository<State>().Table.FirstOrDefaultAsync(x => x.Id == id);
            state.Title = title;
            await _work.GenericRepository<State>().UpdateAsync(state, CancellationToken.None);
            return RedirectToAction("ManageState");
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> UpdateCity(string title, int id, int stateId)
    {
        if (User.Identity.IsAuthenticated)
        {
            var state = await _work.GenericRepository<City>().Table.FirstOrDefaultAsync(x => x.Id == id);
            state.Name = title;
            state.StateId = stateId;
            await _work.GenericRepository<City>().UpdateAsync(state, CancellationToken.None);
            return RedirectToAction("ManageState");
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> InsertCat(string title, bool isActive, IFormFile? logo, int mainCatId)
    {
        if (User.Identity.IsAuthenticated)
        {
            Upload up = new Upload(_webHostEnvironment);
            var img = logo != null ? up.Uploadfile(logo, "Category") : string.Empty;
            await _work.GenericRepository<Category>().AddAsync(new Category
            {
                Name = title,
                LogoUri = img,
                IsActive = isActive,
                MainCategoryId = mainCatId
            }, CancellationToken.None);
            return RedirectToAction("ManageCategory");
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> InsertMainCat(string title, bool isActive, IFormFile? logo)
    {
        if (User.Identity.IsAuthenticated)
        {
            Upload up = new Upload(_webHostEnvironment);
            var img = logo != null ? up.Uploadfile(logo, "Category") : string.Empty;
            await _work.GenericRepository<MainCategory>().AddAsync(new MainCategory
            {
                Name = title,
                LogoUri = img,
                IsActive = isActive,
            }, CancellationToken.None);
            return RedirectToAction("ManageCategory");
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> InsertSubCat(string title, bool isActive, int? catId, string seoTitle,
        string seoDesc, string seoCanonical)
    {
        if (User.Identity.IsAuthenticated)
        {
            var cat = await _work.GenericRepository<Category>().TableNoTracking.FirstOrDefaultAsync(x => x.Id == catId);
            if (cat == null)
            {
                ViewBag.Error = new ErrorAdmin
                {
                    Message = "دسته بندی پیدا نشد",
                    StatusCode = ApiResultStatusCode.NotFound,
                    IsError = true
                };
                return RedirectToAction("ManageCategory");
            }

            await _work.GenericRepository<SubCategory>().AddAsync(new SubCategory
            {
                Name = title,
                IsActive = isActive,
                CategoryId = cat.Id,
                SeoDesc = seoDesc,
                SeoCanonical = seoCanonical,
                SeoTitle = seoTitle
            }, CancellationToken.None);
            return RedirectToAction("ManageCategory");
        }
        else
        {
            return View("Login");
        }
    }


    public async Task<ActionResult> Color(string search)
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            if (!string.IsNullOrWhiteSpace(search))
            {
                ViewBag.Colors = await _work.GenericRepository<Color>().TableNoTracking
                    .Where(x => x.ColorCode.Contains(search) || x.Title.Contains(search)).OrderByDescending(x => x.Id)
                    .ToListAsync();
            }
            else
            {
                ViewBag.Colors = await _work.GenericRepository<Color>().TableNoTracking.OrderByDescending(x => x.Id)
                    .ToListAsync();
            }

            #endregion

            return View();
        }
        else
        {
            return View("Login");
        }
    }


    public async Task<ActionResult> InsertColor(string title, string code)
    {
        if (User.Identity.IsAuthenticated)
        {
            await _work.GenericRepository<Color>().AddAsync(new Color
            {
                ColorCode = code,
                Title = title
            }, CancellationToken.None);
            return RedirectToAction("Color");
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> EditProduct(int id)
    {
        if (User.Identity.IsAuthenticated && id > 0)
        {
            var product = await _work.GenericRepository<Product>().TableNoTracking
                .Include(x => x.Brand)
                .Include(x => x.Offer)
                .Include(x => x.ProductDetails).ThenInclude(x => x.CategoryDetail).ThenInclude(x => x.Feature)
                .Include(x => x.ProductImages)
                .Include(x => x.UserUpdate)
                .Include(x => x.Creator)
                .Include(x => x.SubCategory).ThenInclude(x => x.Category)
                .Include(x => x.ProductColors).ThenInclude(x => x.Color)
                .Include(x => x.ProductColors).ThenInclude(x => x.Guarantee)
                .AsSplitQuery().FirstOrDefaultAsync(x => x.Id == id);
            var prodDetail = product.ProductDetails.ToList();
            var catDetails = await _work.GenericRepository<CategoryDetail>()
                .TableNoTracking
                .Include(x => x.Feature)
                .Include(x => x.SubCategoryDetails)
                .Where(x => x.SubCategoryDetails.Any(q => q.SubCategoryId == product.SubCategoryId)).ToListAsync();
            foreach (var i in catDetails)
            {
                if (!prodDetail.Any(x => x.Id == i.Id))
                {
                    prodDetail.Add(new ProductDetail
                    {
                        Id = 0,
                        IsDelete = i.IsDelete,
                        Value = string.Empty,
                        CategoryDetailId = i.Id,
                        ProductId = product.Id,
                    });
                }
            }

            ViewBag.Product = product;
            ViewBag.Brands = await _work.GenericRepository<Brand>().TableNoTracking.OrderByDescending(x => x.Id)
                .ToListAsync();
            ViewBag.BrandTags = await _work.GenericRepository<BrandTag>().TableNoTracking.OrderByDescending(x => x.Id)
                .ToListAsync();

            ViewBag.SubCats = await _work.GenericRepository<SubCategory>().TableNoTracking.OrderByDescending(x => x.Id)
                .ToListAsync();
            ViewBag.Colors = await _work.GenericRepository<Color>().TableNoTracking.OrderByDescending(x => x.Id)
                .ToListAsync();
            ViewBag.Guarantee = await _work.GenericRepository<Guarantee>().TableNoTracking.OrderByDescending(x => x.Id)
                .ToListAsync();
            return View();
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> TranscriptProduct(int id)
    {
        if (User.Identity.IsAuthenticated && id > 0)
        {
            var product = await _work.GenericRepository<Product>().TableNoTracking
                .Include(x => x.Brand)
                .Include(x => x.Offer)
                .Include(x => x.ProductDetails).ThenInclude(x => x.CategoryDetail).ThenInclude(x => x.Feature)
                .Include(x => x.ProductImages)
                .Include(x => x.UserUpdate)
                .Include(x => x.Creator)
                .Include(x => x.SubCategory).ThenInclude(x => x.Category)
                .Include(x => x.ProductColors).ThenInclude(x => x.Color)
                .Include(x => x.ProductColors).ThenInclude(x => x.Guarantee)
                .AsSplitQuery()
                .OrderByDescending(x => x.Id).FirstOrDefaultAsync(x => x.Id == id);
            var prodDetail = product.ProductDetails.ToList();
            var catDetails = await _work.GenericRepository<CategoryDetail>()
                .TableNoTracking
                .Include(x => x.Feature)
                .Include(x => x.SubCategoryDetails)
                .Where(x => x.SubCategoryDetails.Any(q => q.SubCategoryId == product.SubCategoryId)).ToListAsync();
            foreach (var i in catDetails)
            {
                if (!prodDetail.Any(x => x.Id == i.Id))
                {
                    prodDetail.Add(new ProductDetail
                    {
                        Id = 0,
                        IsDelete = i.IsDelete,
                        Value = string.Empty,
                        CategoryDetailId = i.Id,
                        ProductId = product.Id,
                    });
                }
            }

            ViewBag.Product = product;
            ViewBag.Brands = await _work.GenericRepository<Brand>().TableNoTracking.OrderByDescending(x => x.Id)
                .ToListAsync();
            ViewBag.BrandTags = await _work.GenericRepository<BrandTag>().TableNoTracking.OrderByDescending(x => x.Id)
                .ToListAsync();

            ViewBag.SubCats = await _work.GenericRepository<SubCategory>().TableNoTracking.OrderByDescending(x => x.Id)
                .ToListAsync();
            ViewBag.Colors = await _work.GenericRepository<Color>().TableNoTracking.OrderByDescending(x => x.Id)
                .ToListAsync();
            ViewBag.Guarantee = await _work.GenericRepository<Guarantee>().TableNoTracking.OrderByDescending(x => x.Id)
                .ToListAsync();
            ViewBag.LastCode = await _work.GenericRepository<Product>().TableNoTracking.OrderByDescending(x => x.Id)
                .Select(x => x.UnicCode).FirstOrDefaultAsync() ?? string.Empty;
            return View();
        }
        else
        {
            return View("Login");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetBrand(int id)
    {
        var cities = await _work.GenericRepository<Brand>().TableNoTracking.Where(x => x.SubCategoryId == id)
            .ToListAsync();
        return Ok(cities);
    }

    [HttpGet]
    public async Task<IActionResult> GetColorProduct(int id)
    {
        var color = await _work.GenericRepository<ProductColor>().TableNoTracking
            .Include(x => x.Color)
            .Include(x=>x.Guarantee)
            .Where(x => x.ProductId == id)
            .ToListAsync();
        return Ok(color);
    }

    public async Task<ActionResult> ProductManage(string search, int route, int subCatId, int brandId, int page = 1)
    {
        ViewBag.Search = search;
        ViewBag.brandId = brandId;
        ViewBag.subCatId = subCatId;
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            ViewBag.productsPage = page;
            ViewBag.route = route;
            var productsQuery = _work.GenericRepository<Product>().TableNoTracking
                .Include(x => x.Brand)
                .Include(x => x.BrandTag)
                .Include(x => x.UserUpdate)
                .Include(x => x.Creator)
                .Include(x => x.ProductColors).ThenInclude(x => x.Color)
                .AsSplitQuery()
                .AsQueryable();
            switch (route)
            {
                case 0:
                    if (!string.IsNullOrWhiteSpace(search))
                    {
                        productsQuery = productsQuery
                            .Where(x => x.SubCategory.Name.Contains(search) ||
                                        x.Brand.Title.Contains(search) ||
                                        x.Detail.Contains(search) ||
                                        x.Strengths.Contains(search) ||
                                        x.FullDesc.Contains(search) ||
                                        x.MetaKeyword.Contains(search) ||
                                        x.FullDesc.Contains(search) ||
                                        x.PersianTitle.Contains(search) ||
                                        x.WeakPoints.Contains(search) ||
                                        x.ProductGift.Contains(search) ||
                                        x.ProductColors.Any(t => t.Color.ColorCode.Contains(search) ||
                                                                 t.Color.Title.Contains(search)) ||
                                        x.ProductDetails.Any(q => q.Value.Contains(search)));
                    }

                    break;
                case 1:
                    productsQuery = productsQuery.Where(x => x.InsertDate >= DateTime.Now.AddDays(-10));
                    break;
                case 2:
                    productsQuery = productsQuery.Where(x => x.IsOffer);
                    break;
                case 3:
                    productsQuery = productsQuery.Where(x => x.MomentaryOffer);
                    break;
                case 4:
                    productsQuery = productsQuery.Where(x => x.ProductColors.Any(q => q.Inventory <= 0));
                    break;
            }

            if (subCatId > 0)
            {
                productsQuery = productsQuery.Where(x => x.SubCategoryId == subCatId);
                if (brandId > 0)
                {
                    productsQuery = productsQuery.Where(x => x.BrandId == brandId);
                }
            }

            ViewBag.Products = await productsQuery
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * 50)
                .Take(50)
                .ToListAsync();


            ViewBag.Brands = await _work.GenericRepository<Brand>().TableNoTracking.OrderByDescending(x => x.Id)
                .ToListAsync();
            ViewBag.BrandTags = await _work.GenericRepository<BrandTag>().TableNoTracking.OrderByDescending(x => x.Id)
                .ToListAsync();
            ViewBag.SubCats = await _work.GenericRepository<SubCategory>().TableNoTracking.OrderByDescending(x => x.Id)
                .ToListAsync();
            ViewBag.Colors = await _work.GenericRepository<Color>().TableNoTracking.OrderByDescending(x => x.Id)
                .ToListAsync();
            ViewBag.Guarantee = await _work.GenericRepository<Guarantee>().TableNoTracking.OrderByDescending(x => x.Id)
                .ToListAsync();

            ViewBag.AllProd = await _work.GenericRepository<Product>().TableNoTracking.CountAsync();

            ViewBag.AllInvProd = await _work.GenericRepository<ProductColor>().TableNoTracking
                .CountAsync(x => x.Inventory > 0);

            ViewBag.AllInvLowProd = await _work.GenericRepository<ProductColor>().TableNoTracking
                .CountAsync(x => x.Inventory <= 0);

            ViewBag.AllNewProd = await _work.GenericRepository<Product>().TableNoTracking
                .CountAsync(x => x.InsertDate >= DateTime.Now.AddDays(-10));

            ViewBag.AllIsOfferProd =
                await _work.GenericRepository<Product>().TableNoTracking.CountAsync(x => x.IsOffer);

            ViewBag.AllIsMomentProd =
                await _work.GenericRepository<Product>().TableNoTracking.CountAsync(x => x.MomentaryOffer);

            #endregion

            ViewBag.LastCode = await _work.GenericRepository<Product>().TableNoTracking.OrderByDescending(x => x.Id)
                .Select(x => x.UnicCode).FirstOrDefaultAsync() ?? string.Empty;
            return View();
        }
        else
        {
            return View("Login");
        }
    }


    public async Task<IActionResult> ManageBanner()
    {
        ViewBag.Banner = await _work.GenericRepository<Banner>().TableNoTracking.FirstOrDefaultAsync() ?? new Banner();
        return View();
    }

    public async Task<IActionResult> ManageBrand(string search, int index = 0)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            ViewBag.Category = await _work.GenericRepository<BrandTag>().TableNoTracking
                .Where(x => x.Title.Contains(search)).OrderByDescending(x => x.Id).ToListAsync();
        }
        else
        {
            ViewBag.Category = await _work.GenericRepository<BrandTag>().TableNoTracking
                .OrderByDescending(x => x.Id).ToListAsync();
        }

        return View();
    }

    public async Task<IActionResult> ManageFooterLink()
    {
        ViewBag.Footer = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ?? new();
        return View();
    }

    public async Task<IActionResult> ManageSlider()
    {
        ViewBag.Banner = await _work.GenericRepository<Banner>().TableNoTracking.FirstOrDefaultAsync() ?? new Banner();
        return View();
    }

    [HttpPost]
    [EnableCors("AllowSpecificOrigin")]
    public IActionResult UploadImage(IFormFile upload)
    {
        Response.Headers.Add("Access-Control-Allow-Origin", "http://newdev.parsme.com");
        Response.Headers.Add("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
        Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
        if (upload != null && upload.Length > 0)
        {
            try
            {
                Upload up = new Upload(_webHostEnvironment);
                var name = up.Uploadfile(upload, "editor");
                // Return the URL to the uploaded file.
                var fileUrl = $"http://newdev.parsme.com/images/editor/{name}";
                return Json(new { uploaded = true, url = fileUrl });
            }
            catch (Exception ex)
            {
                return Json(new { uploaded = false, error = ex.Message });
            }
        }

        return Json(new { uploaded = false, error = "No file was uploaded." });
    }

    [HttpPost]
    public async Task<IActionResult> UpdateBanner(int id, string href, IFormFile image)
    {
        Upload up = new Upload(_webHostEnvironment);
        var banners = await _work.GenericRepository<Banner>().TableNoTracking.FirstOrDefaultAsync();

        switch (id)
        {
            case 1:
                banners.SmallBannerMiddle1 = image != null
                    ? up.Uploadfile(image, "Banner")
                    : banners.SmallBannerMiddle1;
                banners.SmallBannerMiddle1Href = href;
                break;
            case 2:
                banners.BigBannerMiddle1 = image != null
                    ? up.Uploadfile(image, "Banner")
                    : banners.BigBannerMiddle1;
                banners.BigBannerMiddle1Href = href;
                break;
            case 3:
                banners.BigBannerMiddle2 = image != null
                    ? up.Uploadfile(image, "Banner")
                    : banners.BigBannerMiddle2;
                banners.BigBannerMiddle2Href = href;
                break;
            case 4:
                banners.LargeSideBanner = image != null
                    ? up.Uploadfile(image, "Banner")
                    : banners.LargeSideBanner;
                banners.LargeSideBannerHref = href;
                break;
            case 5:
                banners.SmallBannerMiddle2 = image != null
                    ? up.Uploadfile(image, "Banner")
                    : banners.SmallBannerMiddle2;
                banners.SmallBannerMiddle2Href = href;
                break;
            case 6:
                banners.SmallBannerMiddle3 = image != null
                    ? up.Uploadfile(image, "Banner")
                    : banners.SmallBannerMiddle3;
                banners.SmallBannerMiddle3Href = href;
                break;
            case 7:
                banners.SmallBannerMiddle4 = image != null
                    ? up.Uploadfile(image, "Banner")
                    : banners.SmallBannerMiddle4;
                banners.SmallBannerMiddle4Href = href;
                break;
            case 8:
                banners.SmallSideBanner = image != null
                    ? up.Uploadfile(image, "Banner")
                    : banners.SmallSideBanner;
                banners.SmallSideBannerHref = href;
                break;
            case 9:
                banners.BigBannerMiddle1Col = image != null
                    ? up.Uploadfile(image, "Banner")
                    : banners.BigBannerMiddle1Col;
                banners.BigBannerMiddle1ColHref = href;
                break;
            case 10:
                banners.BigBannerMiddle2Col = image != null
                    ? up.Uploadfile(image, "Banner")
                    : banners.BigBannerMiddle2Col;
                banners.BigBannerMiddle2ColHref = href;
                break;
        }

        await _work.GenericRepository<Banner>().UpdateAsync(banners, CancellationToken.None);
        return RedirectToAction("ManageBanner");
    }

    public async Task<IActionResult> UpdateFooterLink(FooterDto request)
    {
        if (request.Id <= 0)
        {
            var entity = _mapper!.Map<FooterLink>(request);
            await _work.GenericRepository<FooterLink>()
                .AddAsync(entity, CancellationToken.None);
        }
        else
        {
            var entity = _mapper!.Map<FooterLink>(request);
            entity.Id = request.Id;
            await _work.GenericRepository<FooterLink>().UpdateAsync(entity, CancellationToken.None);
        }

        return RedirectToAction("ManageFooterLink");
    }

    public async Task<IActionResult> UpdateSlider(int id, string href, string title, IFormFile image)
    {
        Upload up = new Upload(_webHostEnvironment);
        var banners = await _work.GenericRepository<Banner>().Table.FirstOrDefaultAsync();

        switch (id)
        {
            case 1:
                banners.SliderHref = href;
                banners.SliderImage = image != null
                    ? up.Uploadfile(image, "Banner")
                    : banners.SliderImage;
                banners.SliderTitle = title;
                break;
            case 2:
                banners.SliderHref1 = href;
                banners.SliderImage1 = image != null
                    ? up.Uploadfile(image, "Banner")
                    : banners.SliderImage1;
                banners.SliderTitle1 = title;
                break;
            case 3:
                banners.SliderHref2 = href;
                banners.SliderImage2 = image != null
                    ? up.Uploadfile(image, "Banner")
                    : banners.SliderImage2;
                banners.SliderTitle2 = title;
                break;
            case 4:
                banners.SliderHref3 = href;
                banners.SliderImage3 = image != null
                    ? up.Uploadfile(image, "Banner")
                    : banners.SliderImage3;
                banners.SliderTitle3 = title;
                break;
            case 5:
                banners.SliderHref4 = href;
                banners.SliderImage4 = image != null
                    ? up.Uploadfile(image, "Banner")
                    : banners.SliderImage4;
                banners.SliderTitle4 = title;
                break;
        }

        await _work.GenericRepository<Banner>().UpdateAsync(banners, CancellationToken.None);

        return RedirectToAction("ManageSlider");
    }

    public async Task<ActionResult> Login()
    {
        return View();
    }
    // public async Task<IActionResult> ChangeAccess()
    // {
    //     var a = await _work.GenericRepository<ContactUs>().TableNoTracking.FirstOrDefaultAsync();
    //     a.IsLogAd = !a.IsLogAd;
    //     await _work.GenericRepository<ContactUs>().UpdateAsync(a, CancellationToken.None);
    //     return Ok();
    // }

    public async Task<ActionResult> LoginPassword(string phoneNumber)
    {
        var isa = false; //await _work.GenericRepository<ContactUs>().TableNoTracking.Select(x => x.IsLogAd)
        //.FirstOrDefaultAsync();
        if (!isa)
        {
            ViewBag.exUser = await _mediator.Send(new AdminExistCommand(phoneNumber));
            return View("LoginPassword");
        }
        else
        {
            throw new Exception();
        }
    }

    public async Task<ActionResult> GoToLoginWithPassword()
    {
        return View("LoginPassword");
    }

    public async Task<ActionResult> InsertSaleServices(SaleService request)
    {
        Upload up = new Upload(_webHostEnvironment);
        var service = await _work.GenericRepository<SaleServices>().TableNoTracking.FirstOrDefaultAsync();
        service.Desc1 = request.Desc1;
        service.Desc1Logo = request.Desc1Logo != null
            ? up.Uploadfile(request.Desc1Logo, "Logo")
            : service.Desc1Logo;
        service.Desc2 = request.Desc2;
        service.Desc2Logo = request.Desc2Logo != null
            ? up.Uploadfile(request.Desc2Logo, "Logo")
            : service.Desc2Logo;
        service.Desc3 = request.Desc3;
        service.Desc3Logo = request.Desc3Logo != null
            ? up.Uploadfile(request.Desc3Logo, "Logo")
            : service.Desc3Logo;
        service.Desc4 = request.Desc4;
        service.Desc4Logo = request.Desc4Logo != null
            ? up.Uploadfile(request.Desc4Logo, "Logo")
            : service.Desc4Logo;
        service.Desc5 = request.Desc5;
        service.Desc5Logo = request.Desc5Logo != null
            ? up.Uploadfile(request.Desc5Logo, "Logo")
            : service.Desc5Logo;
        service.DescReceivePerson = request.DescReceivePerson;
        service.DescShippingToday = request.DescShippingToday;
        await _work.GenericRepository<SaleServices>().UpdateAsync(service, CancellationToken.None);
        return RedirectToAction("SaleServices");
    }

    public async Task<ActionResult> SaleServices()
    {
        ViewBag.SaleServices = await _work.GenericRepository<SaleServices>().TableNoTracking.FirstOrDefaultAsync() ??
                               new SaleServices();
        return View();
    }

    public async Task<ActionResult> GoToLoginWithCode()
    {
        return View("ConfirmCode");
    }

    public async Task<IActionResult> InsertAdmin(InsertUser request)
    {
        var user = new Domain.Entity.User.User
        {
            Family = request.Family,
            Name = request.Name,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Password = request.Password,
            InsertDate = DateTime.Now,
            UserName = request.PhoneNumber,
            SecurityStamp = string.Empty,
            CityId = request.CityId,
            Sheba = request.Sheba,
            NationalCode = request.NationalCode,
        };
        if (!await _roleManager.RoleExistsAsync("user"))
        {
            await _roleManager.CreateAsync(new Role
            {
                Name = "user"
            });
        }

        await _userManager.CreateAsync(user, request.Password);
        foreach (var r in request.Role)
        {
            await _userManager.AddToRoleAsync(user, r);
        }

        await _userManager.AddToRoleAsync(user, "user");
        await _userManager.UpdateAsync(user);
        return RedirectToAction("ManageUser");
    }

    public async Task<IActionResult> UpdateAdmin(UpdateUser request)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x =>
            x.UserName == request.PhoneNumber && x.PhoneNumber == request.PhoneNumber);
        Upload up = new Upload(_webHostEnvironment);
        user.Name = request.Name;
        user.Family = request.Family;
        user.Sheba = request.Sheba;
        user.CityId = request.CityId;
        user.Email = request.Email;
        user.NationalCode = request.NationalCode;
        if (request.Password != user.Password)
        {
            await _userManager.ChangePasswordAsync(user, user.Password, request.Password);
            user.Password = request.Password;
        }

        foreach (var r in request.Role)
        {
            await _userManager.AddToRoleAsync(user, r);
        }

        await _userManager.UpdateAsync(user);
        return RedirectToAction("ManageUser");
    }

    public async Task<ActionResult> SendLoginCode(string phoneNumber)
    {
        var user = await _userManager.FindByNameAsync(phoneNumber);
        var userRoles = await _userManager.GetRolesAsync(user);
        KavenegarApi webApi = new KavenegarApi(apikey: ApiKeys.ApiKey);
        if (user == null && userRoles.Any(x => !x.Equals("admin")))
            throw new Exception("User not Exist");
        user.ConfirmCode = Helpers.GetConfirmCode();
        user.ConfirmCodeExpireTime = DateTime.Now.AddMinutes(3);
        await _userManager.UpdateAsync(user);
        var result = webApi.VerifyLookup(phoneNumber, user.ConfirmCode,
            "VerifyCodeFaani");
        return Ok(
        );
    }

    public async Task initAdmin()
    {
        var user = new Domain.Entity.User.User
        {
            Family = "الهی",
            Name = "مهدی",
            PhoneNumber = "09125955576",
            Email = "",
            Password = "1111",
            InsertDate = DateTime.Now,
            UserName = "09125955576",
            SecurityStamp = string.Empty,
            CityId = 1
        };
        if (!await _roleManager.RoleExistsAsync("user"))
        {
            await _roleManager.CreateAsync(new Role
            {
                Name = "user"
            });
        }

        if (!await _roleManager.RoleExistsAsync("admin"))
        {
            await _roleManager.CreateAsync(new Role
            {
                Name = "admin"
            });
        }

        await _userManager.CreateAsync(user, "1111");
        await _userManager.AddToRoleAsync(user, "user");
        await _userManager.AddToRoleAsync(user, "admin");
        await _userManager.UpdateAsync(user);
    }

    public async Task initAdmin2()
    {
        var user = new Domain.Entity.User.User
        {
            Family = "k1",
            Name = "ارجمند",
            PhoneNumber = "09211129482",
            Email = "",
            Password = "1111",
            InsertDate = DateTime.Now,
            UserName = "09211129482",
            SecurityStamp = string.Empty,
            CityId = 1
        };
        if (!await _roleManager.RoleExistsAsync("user"))
        {
            await _roleManager.CreateAsync(new Role
            {
                Name = "user"
            });
        }

        if (!await _roleManager.RoleExistsAsync("admin"))
        {
            await _roleManager.CreateAsync(new Role
            {
                Name = "admin"
            });
        }

        await _userManager.CreateAsync(user, "1111");
        await _userManager.AddToRoleAsync(user, "user");
        await _userManager.AddToRoleAsync(user, "admin");
        await _userManager.UpdateAsync(user);
    }

    public async Task<ActionResult> LoginCod(string phoneNumber)
    {
        ViewBag.exUser = await _mediator.Send(new LoginCodAdminCommand(phoneNumber));
        return View("ConfirmCode");
    }

    public async Task<ActionResult> ConfirmPassword(string password, string phoneNumber)
    {
        var user = await _mediator.Send(new ConfirmPasswordAdminCommand(password, phoneNumber));
        var result = await _signInManager.PasswordSignInAsync(user, password, true, false);
        if (!result.Succeeded)
            throw new Exception("invalid password");
        return Ok();
    }

    public async Task<ActionResult> CheckPhoneNumber(string phoneNumber)
    {
        await _mediator.Send(new CheckAdminNumberQuery
        {
            PhoneNumber = phoneNumber
        });
        return Ok();
    }

    public async Task<ActionResult> ConfirmCode(string code, string phoneNumber)
    {
        var result = await _mediator.Send(new ConfirmCodAdminCommand(phoneNumber, code));
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        await _signInManager.PasswordSignInAsync(user, user.Password, true, false);

        return Ok();
    }


    public async Task<ActionResult> Factor(int page = 1)
    {
        if (User.Identity.IsAuthenticated)
        {
            return View("Factor");
        }
        else
        {
            return View("Index");
        }
    }

    public async Task<ActionResult> ManageFaq(string search, int page = 1)
    {
        if (User.Identity.IsAuthenticated)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                ViewBag.Faqs = await _work.GenericRepository<Faq>().TableNoTracking
                    .Include(x => x.FaqCat)
                    .Where(x => x.Title.Contains(search) || x.Desc.Contains(search))
                    .ToListAsync();
            }
            else
            {
                ViewBag.Faqs = await _work.GenericRepository<Faq>().TableNoTracking
                    .Include(x => x.FaqCat)
                    .ToListAsync();
            }

            ViewBag.FaqCats = await _work.GenericRepository<FaqCat>().TableNoTracking.ToListAsync();
            return View();
        }
        else
        {
            return RedirectToAction("Login");
        }
    }

    public async Task<ActionResult> UpdateFaq(int id, string title, string desc, int catId)
    {
        if (User.Identity.IsAuthenticated)
        {
            var faq = await _work.GenericRepository<Faq>().Table.FirstOrDefaultAsync(x => x.Id == id);
            faq.Title = title;
            faq.Desc = desc;
            faq.FaqCatId = catId;
            await _work.GenericRepository<Faq>().UpdateAsync(faq, CancellationToken.None);
            return RedirectToAction("ManageFaq");
        }
        else
        {
            return RedirectToAction("Login");
        }
    }

    public async Task<ActionResult> InsertFaq(string title, string desc, int catId)
    {
        if (User.Identity.IsAuthenticated)
        {
            await _work.GenericRepository<Faq>().AddAsync(new Faq
            {
                Desc = desc,
                Title = title,
                FaqCatId = catId
            }, CancellationToken.None);
            return RedirectToAction("ManageFaq");
        }
        else
        {
            return RedirectToAction("Login");
        }
    }

    public async Task<ActionResult> InsertCatFaq(string title, IFormFile logo)
    {
        if (User.Identity.IsAuthenticated)
        {
            Upload up = new Upload(_webHostEnvironment);
            string img = up.Uploadfile(logo, "Logo") ?? string.Empty;
            await _work.GenericRepository<FaqCat>().AddAsync(new FaqCat
            {
                Title = title,
                LogoUri = img,
            }, CancellationToken.None);
            return RedirectToAction("ManageFaq");
        }
        else
        {
            return RedirectToAction("Login");
        }
    }

    public async Task<ActionResult> UpdateCatFaq(int id, string title, IFormFile logo)
    {
        if (User.Identity.IsAuthenticated)
        {
            Upload up = new Upload(_webHostEnvironment);
            var faq = await _work.GenericRepository<FaqCat>().Table.FirstOrDefaultAsync(x => x.Id == id);
            string img = up.Uploadfile(logo, "Logo");
            faq.Title = title;
            if (!string.IsNullOrEmpty(img))
            {
                faq.LogoUri = img;
            }

            await _work.GenericRepository<FaqCat>().UpdateAsync(faq, CancellationToken.None);
            return RedirectToAction("ManageFaq");
        }
        else
        {
            return RedirectToAction("Login");
        }
    }

    public async Task<ActionResult> ManageSearchResult(string search, int page = 1)
    {
        if (User.Identity.IsAuthenticated)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                ViewBag.search = await _work.GenericRepository<SearchResult>().TableNoTracking
                    .Where(x => x.Href.Contains(search) || x.Value.Contains(search))
                    .ToListAsync();
            }
            else
            {
                ViewBag.search = await _work.GenericRepository<SearchResult>().TableNoTracking.ToListAsync();
            }

            return View();
        }
        else
        {
            return RedirectToAction("Login");
        }
    }

    public async Task<ActionResult> UpdateSearchResult(int id, string value, string href)
    {
        if (User.Identity.IsAuthenticated)
        {
            var search = await _work.GenericRepository<SearchResult>().Table.FirstOrDefaultAsync(x => x.Id == id);
            search.Value = value;
            search.Href = href;
            await _work.GenericRepository<SearchResult>().UpdateAsync(search, CancellationToken.None);
            return RedirectToAction("ManageSearchResult");
        }
        else
        {
            return RedirectToAction("Login");
        }
    }

    public async Task<ActionResult> InsertSearchResult(string value, string href)
    {
        if (User.Identity.IsAuthenticated)
        {
            await _work.GenericRepository<SearchResult>().AddAsync(new SearchResult()
            {
                Value = value,
                Href = href
            }, CancellationToken.None);
            return RedirectToAction("ManageSearchResult");
        }
        else
        {
            return RedirectToAction("Login");
        }
    }


    public async Task<ActionResult> UpdateDiscount(int id, string code, double amount, int count)
    {
        if (User.Identity.IsAuthenticated)
        {
            var discount = await _work.GenericRepository<DiscountCode>().Table.FirstOrDefaultAsync(x => x.Id == id);
            discount.Count = count;
            discount.Code = code;
            discount.Amount = amount;
            await _work.GenericRepository<DiscountCode>().UpdateAsync(discount, CancellationToken.None);
            return RedirectToAction("ManageDiscount");
        }
        else
        {
            return RedirectToAction("Login");
        }
    }

    public async Task<ActionResult> InsertDiscount(string code, double amount, int count)
    {
        if (User.Identity.IsAuthenticated)
        {
            await _work.GenericRepository<DiscountCode>().AddAsync(new DiscountCode()
            {
                Count = count,
                Amount = amount,
                Code = code,
                FirstCount = count,
                IsActive = true
            }, CancellationToken.None);
            return RedirectToAction("ManageDiscount");
        }
        else
        {
            return RedirectToAction("Login");
        }
    }


    public async Task<ActionResult> ManageState(string search, int index)
    {
        if (User.Identity.IsAuthenticated)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                ViewBag.States =
                    await _work.GenericRepository<State>().TableNoTracking.Include(x => x.Cities)
                        .OrderByDescending(x => x.Id).ToListAsync();
                ViewBag.Cities = await _work.GenericRepository<City>().TableNoTracking.Include(x => x.State)
                    .OrderByDescending(x => x.Id).ToListAsync();
                return View();
            }
            else
            {
                if (index == 0)
                {
                    ViewBag.States =
                        await _work.GenericRepository<State>().TableNoTracking.Include(x => x.Cities)
                            .Where(x => x.Title.Contains(search)).OrderByDescending(x => x.Id).ToListAsync();
                    ViewBag.Cities = await _work.GenericRepository<City>().TableNoTracking.Include(x => x.State)
                        .OrderByDescending(x => x.Id).ToListAsync();
                }

                if (index == 1)
                {
                    ViewBag.States =
                        await _work.GenericRepository<State>().TableNoTracking.Include(x => x.Cities)
                            .OrderByDescending(x => x.Id).ToListAsync();
                    ViewBag.Cities = await _work.GenericRepository<City>().TableNoTracking.Include(x => x.State)
                        .Where(x => x.Name.Contains(search) || x.State.Title.Contains(search))
                        .OrderByDescending(x => x.Id).ToListAsync();
                }

                return View();
            }
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    public async Task<ActionResult> InsertState(string title)
    {
        if (User.Identity.IsAuthenticated)
        {
            await _work.GenericRepository<State>().AddAsync(new State
            {
                Title = title
            }, CancellationToken.None);
            return RedirectToAction("ManageState");
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    public async Task<ActionResult> InsertCity(string title, int stateId)
    {
        if (User.Identity.IsAuthenticated)
        {
            await _work.GenericRepository<City>().AddAsync(new City
            {
                Name = title,
                StateId = stateId
            }, CancellationToken.None);
            return RedirectToAction("ManageState");
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    public async Task<ActionResult> ManageUser(string search)
    {
        if (User.Identity.IsAuthenticated)
        {
            if (string.IsNullOrEmpty(search))
            {
                var user = await _userManager.Users.OrderByDescending(x => x.Id).ToListAsync();
                var users = new List<UserDto>();
                foreach (var i in user)
                {
                    var a = await _userManager.GetRolesAsync(i);
                    users.Add(new UserDto
                    {
                        City = i.City,
                        Family = i.Family,
                        Name = i.Name,
                        Password = i.Password,
                        InsertDate = i.InsertDate,
                        NationalCode = i.NationalCode,
                        Sheba = i.Sheba,
                        PhoneNumber = i.PhoneNumber,
                        Email = i.Email ?? string.Empty,
                        CityId = i.CityId,
                        Roles = a.ToList(),
                    });
                }

                ViewBag.Users = users;
                ViewBag.Cities = await _work.GenericRepository<City>().TableNoTracking.ToListAsync();
                ViewBag.Roles = await _roleManager.Roles.ToListAsync();
            }
            else
            {
                var user = await _userManager.Users.Include(x => x.City).Where(x =>
                        x.UserName.Contains(search) || x.Name.Contains(search) || x.Family.Contains(search) ||
                        x.Sheba.Contains(search) || x.NationalCode.Contains(search) || x.Email.Contains(search) ||
                        x.PhoneNumber.Contains(search) || x.City.Name.Contains(search)).OrderByDescending(x => x.Id)
                    .ToListAsync();
                var users = new List<UserDto>();
                foreach (var i in user)
                {
                    var a = await _userManager.GetRolesAsync(i);
                    users.Add(new UserDto
                    {
                        City = i.City,
                        Family = i.Family,
                        Name = i.Name,
                        Password = i.Password,
                        InsertDate = i.InsertDate,
                        NationalCode = i.NationalCode,
                        Sheba = i.Sheba,
                        PhoneNumber = i.PhoneNumber,
                        Email = i.Email ?? string.Empty,
                        CityId = i.CityId,
                        Roles = a.ToList()
                    });
                }

                ViewBag.Users = users;
                ViewBag.Cities = await _work.GenericRepository<City>().TableNoTracking.OrderByDescending(x => x.Id)
                    .ToListAsync();
                ViewBag.Roles = await _roleManager.Roles.OrderByDescending(x => x.Id).ToListAsync();
            }

            return View();
        }
        else
        {
            return View("Index");
        }
    }


    public async Task<ApiAction> InsertProduct(Root request)
    {
        if (await _work.GenericRepository<Product>().TableNoTracking.AnyAsync(x => x.UnicCode == request.UnicCode))
            return new ApiAction
            {
                IsSuccess = false,
                Message = "کد محصول تکراری میباشد"
            };

        #region Validate

        var catDetailCount = await _work.GenericRepository<CategoryDetail>()
            .TableNoTracking
            .Where(x => x.SubCategoryDetails.Any(q => q.SubCategoryId == request.SubCategoryId.ToInt()))
            .CountAsync();

        // if (catDetailCount != request.ProductDetails.Count())
        // {
        //     return new ApiAction
        //     {
        //         IsSuccess = false,
        //         Message = "خطا در پردازش جزئیات محصول"
        //     };
        // }

        if (request.ProductColors.Count <= 0)
        {
            return new ApiAction
            {
                IsSuccess = false,
                Message = "خطا در پردازش رنگ محصول"
            };
        }

        #endregion

        Upload up = new Upload(_webHostEnvironment);
        string imageUri = string.Empty;
        List<string> images = new List<string>();
        if (!string.IsNullOrWhiteSpace(request.ImageUri))
        {
            imageUri = up.AddImage(request.ImageUri, "Images/ProductImage",
                Guid.NewGuid().ToString().Substring(0, 6));
        }

        if (request.Images.Count > 0)
        {
            foreach (var i in request.Images)
            {
                images.Add(up.AddImage(i, "Images/ProductImage",
                    Guid.NewGuid().ToString().Substring(0, 6)));
            }
        }

        request.ImageUri = imageUri;
        request.Images = images;
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        await _mediator.Send(new InsertProductCommand
        {
            Product = request,
            UserId = user.Id
        });

        return new ApiAction
        {
            IsSuccess = true,
            Message = "محصول با موفقیت ثبت و اضافه شد"
        };
    }

    public class UpdatePriceDto
    {
        public int Id { get; set; }
        public string Price { get; set; }
    }

    public async Task<ActionResult> UpdatePrice(int prodId, List<UpdatePriceDto> colors)
    {
        var prod = await _work.GenericRepository<Product>().Table.Include(x => x.ProductColors)
            .ThenInclude(x => x.Color).FirstOrDefaultAsync(x => x.Id == prodId);
        foreach (var c in prod.ProductColors)
        {
            if (colors.Any(x => x.Id == c.Id))
            {
                var req = colors.FirstOrDefault(x => x.Id == c.Id);
                c.Price = Convert.ToInt32(req.Price.Replace(",", ""));
            }
        }
        await _work.GenericRepository<Product>().UpdateAsync(prod, CancellationToken.None);
        return RedirectToAction("ProductManage");
    }

    public async Task<ApiAction> UpdateProduct(Root request)
    {
        if (await _work.GenericRepository<Product>().TableNoTracking
                .AnyAsync(x => x.UnicCode == request.UnicCode && x.Id != request.Id))
            return new ApiAction
            {
                IsSuccess = false,
                Message = "کد محصول تکراری میباشد"
            };

        #region Validate

        var catDetailCount = await _work.GenericRepository<CategoryDetail>()
            .TableNoTracking
            .Where(x => x.SubCategoryDetails.Any(q => q.SubCategoryId == request.SubCategoryId.ToInt()))
            .CountAsync();

        // if (
        //     catDetailCount != request.ProductDetails.Count())
        // {
        //     return new ApiAction
        //     {
        //         IsSuccess = false,
        //         Message = "خطا در پردازش جزئیات محصول"
        //     };
        // }

        if (request.ProductColors.Count <= 0)
        {
            return new ApiAction
            {
                IsSuccess = false,
                Message = "خطا در پردازش رنگ محصول"
            };
        }

        #endregion

        var product = await _work.GenericRepository<Product>().Table.FirstOrDefaultAsync(x => x.Id == request.Id);
        if (product == null) throw new Exception();
        Upload up = new Upload(_webHostEnvironment);
        var user = await _userManager.FindByNameAsync(User.Identity.Name);

        #region prod

        product.Title = request.Title;
        product.PersianTitle = request.PersianTitle;
        product.UnicCode = request.UnicCode;
        product.Detail = request.Detail;
        product.MetaKeyword = request.MetaKeyword;
        product.FullDesc = request.FullDesc;
        product.ImageUri = !string.IsNullOrWhiteSpace(request.ImageUri)
            ? up.AddImage(request.ImageUri, "Images/ProductImage",
                Guid.NewGuid().ToString().Substring(0, 6))
            : product.ImageUri;
        product.ProductGift = request.ProductGift;
        product.DiscountAmount = request.DiscountAmount.ToDouble();
        product.ProductStatus = (ProductStatus)request.ProductStatus.ToInt();
        product.IsActive = request.IsActive;
        product.IsShowIndex = request.IsShowIndex;
        product.IsOffer = request.IsOffer;
        product.Strengths = request.Strengths;
        product.WeakPoints = request.WeakPoints;
        product.MomentaryOffer = request.MomentaryOffer;
        product.SeoCanonical = request.SeoCanonical;
        product.InterestRate = request.InterestRate;
        product.SeoDesc = request.SeoDesc;
        product.SeoTitle = request.SeoTitle;
        product.BrandTagId = request.BrandTagId.ToInt();
        product.UserUpdateId = user.Id;
        product.UpdateTime = DateTime.Now;

        if (product.BrandId != request.BrandId.ToInt())
        {
            var brand = await _work.GenericRepository<Brand>().Table
                .FirstOrDefaultAsync(x => x.Id == request.BrandId.ToInt());
            if (brand == null) throw new Exception();
            product.BrandId = brand.Id;
        }

        if (product.SubCategoryId != request.SubCategoryId.ToInt() && request.SubCategoryId.ToInt() > 0)
        {
            var subCat = await _work.GenericRepository<SubCategory>().Table
                .FirstOrDefaultAsync(x => x.Id == request.SubCategoryId.ToInt());
            if (subCat == null) throw new Exception();
            product.SubCategoryId = subCat.Id;

            var catDetail = await _work.GenericRepository<ProductDetail>().Table.Where(x => x.ProductId == product.Id)
                .ToListAsync();
            foreach (var i in catDetail)
            {
                await _work.GenericRepository<ProductDetail>().DeleteAsync(i, CancellationToken.None);
            }

            foreach (var i in request.ProductDetails)
            {
                await _work.GenericRepository<ProductDetail>().AddAsync(new ProductDetail
                {
                    ProductId = product.Id,
                    Value = i.DetailName,
                    CategoryDetailId = i.DetailId.ToInt(),
                }, CancellationToken.None);
            }
        }
        else
        {
            var catDetail = await _work.GenericRepository<ProductDetail>().Table.Where(x => x.ProductId == product.Id)
                .ToListAsync();
            foreach (var i in catDetail)
            {
                var val = request.ProductDetails.FirstOrDefault(x => x.DetailId.ToInt() == i.CategoryDetailId);
                if (val != null)
                {
                    i.Value = val.DetailName ?? string.Empty;
                    await _work.GenericRepository<ProductDetail>().UpdateAsync(i, CancellationToken.None);
                }
            }
        }

        await _work.GenericRepository<Product>().UpdateAsync(product, CancellationToken.None);

        #endregion


        #region color

        var proColor = await _work.GenericRepository<ProductColor>().Table.Where(x => x.ProductId == product.Id)
            .ToListAsync();

        foreach (var i in request.ProductColors)
        {
            if (proColor.Any(x => x.ColorId == i.ColorId.ToInt()))
            {
                var prod = proColor.FirstOrDefault(x => x.ColorId == i.ColorId.ToInt());
                prod.Price = i.ColorPrice.ToDouble();
                prod.Inventory = i.ColorInv.ToInt();
                prod.GuaranteeId = i.Gu.ToInt();
                await _work.GenericRepository<ProductColor>().UpdateAsync(prod, CancellationToken.None);
            }
            else
            {
                await _work.GenericRepository<ProductColor>().AddAsync(new ProductColor
                {
                    ProductId = product.Id,
                    Priority = 1,
                    GuaranteeId = i.Gu.ToInt(),
                    ColorId = i.ColorId.ToInt(),
                    Inventory = i.ColorInv.ToInt(),
                    Price = i.ColorPrice.ToDouble(),
                }, CancellationToken.None);
            }
        }

        foreach (var i in proColor)
        {
            if (request.ProductColors.All(x => x.ColorId.ToInt() != i.ColorId))
            {
                await _work.GenericRepository<ProductColor>().DeleteAsync(i, CancellationToken.None);
            }
        }

        #endregion

        if (request.IsOffer)
        {
            var offer = await _work.GenericRepository<Domain.Entity.Product.Offer>().Table
                .FirstOrDefaultAsync(x => x.ProductId == product.Id);
            if (offer != null)
            {
                offer.OfferAmount = request.Offer.OfferAmount.ToDouble();
                offer.Hours = request.Offer.Hours.ToInt();
                offer.Minutes = request.Offer.Minutes.ToInt();
                offer.ColorId = request.Offer.ColorId.ToInt();
                offer.StartDate = request.Offer.Time;
                await _work.GenericRepository<Domain.Entity.Product.Offer>().UpdateAsync(offer, CancellationToken.None);
            }
            else
            {
                var offerInsert = new Domain.Entity.Product.Offer
                {
                    ProductId = product.Id,
                    StartDate = request.Offer.Time,
                    OfferAmount = request.Offer.OfferAmount.ToDouble(),
                    ColorId = request.Offer.ColorId.ToInt(),
                    Minutes = request.Offer.Minutes.ToInt(),
                    Hours = request.Offer.Hours.ToInt(),
                };
                await _work.GenericRepository<Domain.Entity.Product.Offer>()
                    .AddAsync(offerInsert, CancellationToken.None);
                product.OfferId = offerInsert.Id;
                await _work.GenericRepository<Product>().UpdateAsync(product, CancellationToken.None);
            }
        }

        if (request.Images.Any())
        {
            var prodImages = await _work.GenericRepository<ImageGallery>().Table.Where(x => x.ProductId == product.Id)
                .ToListAsync();
            foreach (var i in prodImages)
            {
                await _work.GenericRepository<ImageGallery>().DeleteAsync(i, CancellationToken.None);
            }

            List<string> images = new List<string>();

            foreach (var i in request.Images)
            {
                images.Add(up.AddImage(i, "Images/ProductImage",
                    Guid.NewGuid().ToString().Substring(0, 6)));
            }

            var productGallery = images.Select(x => new ImageGallery
            {
                ProductId = product.Id,
                ImageUri = x
            }).ToList();
            foreach (var i in productGallery)
            {
                await _work.GenericRepository<Domain.Entity.Product.ImageGallery>()
                    .AddAsync(i, CancellationToken.None);
            }
        }

        return new ApiAction
        {
            IsSuccess = true,
            Message = "تغیرات با موفقیت اعمال شد"
        };
    }

    // string Title, string PersianTitle, string Detail, string MetaDesc,
    // string MetaKeyword, string FullDesc, string[] ImageUri, string ProductGift, double DiscountAmount, int BrandId,
    // int SubCategoryId, string[] Images, ProductDetail[] ProductDetails, ProductColor[] ProductColors,
    //     Offer Offer, ProductStatus ProductStatus, bool IsActive, bool IsOffer
    public async Task<List<DetailProdAdmin>> GetCategoryDetailBySubCatId(int subCatId)
    {
        var detail = await _work.GenericRepository<CategoryDetail>()
            .TableNoTracking
            .Include(x => x.SubCategoryDetails).ThenInclude(x => x.SubCategory).ThenInclude(x => x.Category)
            .Include(x => x.Feature)
            .Where(x => x.SubCategoryDetails.Any(q => q.SubCategoryId == subCatId))
            .OrderByDescending(x => x.Feature.Priority).ToListAsync();
        List<DetailProdAdmin> detailProdAdmins = new List<DetailProdAdmin>();
        foreach (var i in detail)
        {
            if (detailProdAdmins.Any(x => x.FeatureId == i.FeatureId))
            {
                var result = detailProdAdmins.FirstOrDefault(x => x.FeatureId == i.FeatureId);
                result.CategoryDetails.Add(new CategoryDetailDto
                {
                    Option = i.Option, DataType = (int)i.DataType, Priority = i.Priority, Title = i.Title,
                    FeatureId = i.FeatureId, ShowInSearch = i.ShowInSearch, SubCategoryId = subCatId, Id = i.Id
                });
            }
            else
            {
                var li = new DetailProdAdmin();
                li.Feature = i.Feature;
                li.FeatureId = i.FeatureId;
                li.CategoryDetails.Add(new CategoryDetailDto
                {
                    Option = i.Option, DataType = (int)i.DataType, Priority = i.Priority, Title = i.Title,
                    FeatureId = i.FeatureId, ShowInSearch = i.ShowInSearch, SubCategoryId = subCatId, Id = i.Id
                });
                detailProdAdmins.Add(li);
            }
        }

        return detailProdAdmins;
    }

    public async Task<List<Brand>> GetBrandBySubCat(int subCatId)
    {
        var a = await _work.GenericRepository<Brand>()
            .TableNoTracking
            .Where(x => x.SubCategoryId == subCatId).OrderByDescending(x => x.Id).ToListAsync();
        return a;
    }

    public async Task<IActionResult> DeleteProd(int id)
    {
        var prod = await _work.GenericRepository<Product>().Table.FirstOrDefaultAsync(x => x.Id == id);
        prod.IsDelete = true;
        await _work.GenericRepository<Product>().UpdateAsync(prod, CancellationToken.None);
        return RedirectToAction("ProductManage", "Admin");
    }

    public async Task<ActionResult> Product(string search, int catId, int page = 1)
    {
        if (User.Identity.IsAuthenticated)
        {
            ViewBag.productCount = _work.GenericRepository<Product>().TableNoTracking.Count();
            ViewBag.Products = _work.GenericRepository<Product>().TableNoTracking.Include(x => x.SubCategory)
                .Include(x => x.ProductColors).ThenInclude(x => x.Color).Count();
            ViewBag.categories = _work.GenericRepository<Category>().TableNoTracking.OrderByDescending(x => x.Id)
                .ToList();


            ViewBag.productsPage = page;

            return View("Product");
        }
        else
        {
            return View("Index");
        }
    }

    public async Task<ActionResult> ManageGuarantee(string search)
    {
        if (User.Identity.IsAuthenticated)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                ViewBag.Guarantee = await _work.GenericRepository<Guarantee>().TableNoTracking
                    .Where(x => x.Title.Contains(search)).OrderByDescending(x => x.Id)
                    .ToListAsync();
            }
            else
            {
                ViewBag.Guarantee = await _work.GenericRepository<Guarantee>().TableNoTracking
                    .OrderByDescending(x => x.Id)
                    .ToListAsync();
            }


            return View();
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    public async Task<ActionResult> InsertGuarantee(string title)
    {
        if (User.Identity.IsAuthenticated)
        {
            await _work.GenericRepository<Guarantee>().AddAsync(new Guarantee
            {
                Title = title
            }, CancellationToken.None);

            return RedirectToAction("ManageGuarantee");
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    public async Task<ActionResult> ManagePostMethod(string search)
    {
        if (User.Identity.IsAuthenticated)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                ViewBag.PostMethod = await _work.GenericRepository<PostMethod>().TableNoTracking
                    .Where(x => x.Title.Contains(search)).OrderByDescending(x => x.Id).ToListAsync();
            }
            else
            {
                ViewBag.PostMethod = await _work.GenericRepository<PostMethod>().TableNoTracking
                    .OrderByDescending(x => x.Id).ToListAsync();
            }

            return View();
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    public async Task<ActionResult> InsertPostMethod(string title, double price)
    {
        if (User.Identity.IsAuthenticated)
        {
            await _work.GenericRepository<PostMethod>().AddAsync(new PostMethod()
            {
                Title = title,
                Price = price
            }, CancellationToken.None);

            return RedirectToAction("ManagePostMethod");
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    public async Task<IActionResult> Logout()
    {
        if (User.Identity.IsAuthenticated)
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        else return RedirectToAction("Index", "Home");
    }
}