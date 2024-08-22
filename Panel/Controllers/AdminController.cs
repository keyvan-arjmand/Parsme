using Application.Admin.V1.Commands.AdminExist;
using Application.Admin.V1.Commands.ConfirmCodAdmin;
using Application.Admin.V1.Commands.ConfirmPasswordAdmin;
using Application.Admin.V1.Commands.LoginCodAdmin;
using Application.Admin.V1.Queries.CheckAdminNumber;
using Application.Common.ApiResult;
using Application.Common.Mapping;
using Application.Common.Utilities;
using Application.Constants.Kavenegar;
using Application.Dtos.Client;
using Application.Interfaces;
using Application.Products.Commands;
using AutoMapper;
using Domain.Entity.Factor;
using Domain.Entity.IndexPage;
using Domain.Entity.Product;
using Domain.Entity.User;
using Domain.Enums;
using Kavenegar;
using MediatR;
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
                .CountAsync(x => x.Status == Status.Pending);
            ViewBag.FactorCount = await _work.GenericRepository<Factor>().TableNoTracking
                .CountAsync();
            ViewBag.Users = await _userManager.Users.ToListAsync();
            ViewBag.Products = await _work.GenericRepository<Product>().TableNoTracking
                .Include(x => x.SubCategory)
                .Include(x => x.Brand)
                .OrderByDescending(x => x.InsertDate)
                .ToListAsync();

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

    public async Task<ActionResult> ManageFactor(string search)
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
                    .ThenInclude(x => x.ProductColor).ThenInclude(x => x!.Product)
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
                    .ThenInclude(x => x.ProductColor).ThenInclude(x => x!.Product)
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
                    .ThenInclude(x => x.ProductColor).ThenInclude(x => x!.Product)
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
                    .ThenInclude(x => x.ProductColor).ThenInclude(x => x!.Product)
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
                        .ThenInclude(x => x.ProductColor).ThenInclude(x => x!.Product).ThenInclude(x => x.Brand)
                        .Include(x => x.Factor).ThenInclude(x => x.Products)
                        .ThenInclude(x => x.ProductColor).ThenInclude(x => x!.Color)
                        .Where(x => x.Factor.DiscountCode.Contains(search) || x.Desc.Contains(search) ||
                                    x.Factor.FactorCode.Contains(search))
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
                    .ThenInclude(x => x.ProductColor).ThenInclude(x => x!.Product).ThenInclude(x => x.Brand)
                    .Include(x => x.Factor).ThenInclude(x => x.Products)
                    .ThenInclude(x => x.ProductColor).ThenInclude(x => x!.Color)
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
                        .ThenInclude(x => x.ProductColor).ThenInclude(x => x!.Product)
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
                    .ThenInclude(x => x.ProductColor).ThenInclude(x => x!.Product)
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
            ViewBag.Brand = await _work.GenericRepository<Brand>().TableNoTracking.FirstOrDefaultAsync(x => x.Id == id);
            ViewBag.Factors = await _work.GenericRepository<BrandLanding>().TableNoTracking
                .Include(x => x.Brand)
                .FirstOrDefaultAsync(x => x.BrandId == id) ?? new();
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
            entity.ImageSlider = up.Uploadfile(request.ImageSlider, "Brand");
            entity.ImageSlider2 = up.Uploadfile(request.ImageSlider2, "Brand");
            entity.ImageSlider3 = up.Uploadfile(request.ImageSlider3, "Brand");
            entity.ImageSlider4 = up.Uploadfile(request.ImageSlider4, "Brand");
            entity.ImageSlider5 = up.Uploadfile(request.ImageSlider5, "Brand");

            entity.BigBanner = up.Uploadfile(request.BigBanner, "Brand");

            entity.SmallBanner1 = up.Uploadfile(request.SmallBanner1, "Brand");
            entity.SmallBanner2 = up.Uploadfile(request.SmallBanner2, "Brand");
            entity.SmallBanner3 = up.Uploadfile(request.SmallBanner3, "Brand");
            entity.SmallBanner4 = up.Uploadfile(request.SmallBanner4, "Brand");

            await _work.GenericRepository<BrandLanding>()
                .AddAsync(entity, CancellationToken.None);
        }
        else
        {
            var footer = await _work.GenericRepository<BrandLanding>().TableNoTracking
                .FirstOrDefaultAsync(x => x.BrandId == request.BrandId);
            var entity = _mapper!.Map<BrandLanding>(request);
            entity.Id = footer.Id;
            entity.ImageSlider = request.ImageSlider != null
                ? up.Uploadfile(request.ImageSlider, "Brand")
                : footer.ImageSlider;

            entity.ImageSlider2 = request.ImageSlider2 != null
                ? up.Uploadfile(request.ImageSlider2, "Brand")
                : footer.ImageSlider2;
            entity.ImageSlider3 = request.ImageSlider3 != null
                ? up.Uploadfile(request.ImageSlider3, "Brand")
                : footer.ImageSlider3;
            entity.ImageSlider4 = request.ImageSlider4 != null
                ? up.Uploadfile(request.ImageSlider4, "Brand")
                : footer.ImageSlider4;
            entity.ImageSlider5 = request.ImageSlider5 != null
                ? up.Uploadfile(request.ImageSlider5, "Brand")
                : footer.ImageSlider5;

            entity.BigBanner = request.BigBanner != null
                ? up.Uploadfile(request.BigBanner, "Brand")
                : footer.BigBanner;
            entity.SmallBanner1 = request.SmallBanner1 != null
                ? up.Uploadfile(request.SmallBanner1, "Brand")
                : footer.SmallBanner1;
            entity.SmallBanner2 = request.SmallBanner2 != null
                ? up.Uploadfile(request.SmallBanner2, "Brand")
                : footer.SmallBanner2;
            entity.SmallBanner3 = request.SmallBanner3 != null
                ? up.Uploadfile(request.SmallBanner3, "Brand")
                : footer.SmallBanner3;
            entity.SmallBanner4 = request.SmallBanner4 != null
                ? up.Uploadfile(request.SmallBanner4, "Brand")
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
                .ThenInclude(x => x.ProductColor).ThenInclude(x => x!.Product).ThenInclude(x => x.Brand)
                .Include(x => x.Products)
                .ThenInclude(x => x.ProductColor).ThenInclude(x => x!.Color)
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

            ViewBag.Contact = await _work.GenericRepository<ContactPage>().TableNoTracking.FirstOrDefaultAsync();

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

            ViewBag.Contact = await _work.GenericRepository<AboutUsPage>().TableNoTracking.FirstOrDefaultAsync();

            #endregion

            return View();
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> InsertContactUs(string desc)
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            var result = await _work.GenericRepository<ContactPage>().Table.FirstOrDefaultAsync();
            if (result != null)
            {
                result.Desc = desc;
                await _work.GenericRepository<ContactPage>().UpdateAsync(result, CancellationToken.None);
            }
            else
            {
                await _work.GenericRepository<ContactPage>().AddAsync(new ContactPage
                {
                    Desc = desc
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

    public async Task<ActionResult> InsertAboutUs(string head, string body)
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            var result = await _work.GenericRepository<AboutUsPage>().Table.FirstOrDefaultAsync();
            if (result != null)
            {
                result.Body = body;
                result.Head = head;
                await _work.GenericRepository<AboutUsPage>().UpdateAsync(result, CancellationToken.None);
            }
            else
            {
                await _work.GenericRepository<AboutUsPage>().AddAsync(new AboutUsPage
                {
                    Body = body,
                    Head = head
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
                .ThenInclude(x => x.ProductColor).ThenInclude(x => x!.Product).ThenInclude(x => x.Brand)
                .Include(x => x.Products)
                .ThenInclude(x => x.ProductColor).ThenInclude(x => x!.Color)
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
                .ThenInclude(x => x.ProductColor).ThenInclude(x => x!.Product).ThenInclude(x => x.Brand)
                .Include(x => x.Factor).ThenInclude(x => x.Products)
                .ThenInclude(x => x.ProductColor).ThenInclude(x => x!.Color)
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

    public async Task<ActionResult> ChangeStatus(int id, string desc, int status)
    {
        if (User.Identity.IsAuthenticated)
        {
            var factor = await _work.GenericRepository<Factor>().Table.FirstOrDefaultAsync(x => x.Id == id);
            factor.Status = (Status)status;
            var admin = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
            await _work.GenericRepository<LogFactor>().AddAsync(new LogFactor
            {
                InsertDate = DateTime.Now,
                UserId = admin.Id,
                FactorId = factor.Id,
                Desc = desc,
            }, CancellationToken.None);
            await _work.GenericRepository<Factor>().UpdateAsync(factor, CancellationToken.None);
            return RedirectToAction("FactorDetail", "Admin", new { factor.Id });
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> ChangeReturnedStatus(int id, string desc, int status)
    {
        if (User.Identity.IsAuthenticated)
        {
            var factor = await _work.GenericRepository<ReturnedFactor>().Table.FirstOrDefaultAsync(x => x.Id == id);
            factor.ReturnedStatus = (ReturnedStatus)status;
            var admin = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
            await _work.GenericRepository<LogReturnedFactor>().AddAsync(new LogReturnedFactor()
            {
                InsertDate = DateTime.Now,
                UserId = admin.Id,
                ReturnedFactorId = factor.Id,
                Desc = desc,
            }, CancellationToken.None);
            await _work.GenericRepository<ReturnedFactor>().UpdateAsync(factor, CancellationToken.None);
            return RedirectToAction("ReturnedFactorDetail", "Admin", new { factor.Id });
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> Brand(string search)
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            if (!string.IsNullOrWhiteSpace(search))
            {
                ViewBag.Brands = await _work.GenericRepository<Brand>().TableNoTracking.Include(x => x.SubCategory)
                    .Where(x => x.Title.Contains(search) ||
                                x.SubCategory.Name.Contains(search)).OrderByDescending(x => x.Id).ToListAsync();
            }
            else
            {
                ViewBag.Brands = await _work.GenericRepository<Brand>().TableNoTracking.Include(x => x.SubCategory)
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

    public async Task<ActionResult> UpdateBrand(int id, string title, IFormFile image, int subCategoryId)
    {
        if (User.Identity.IsAuthenticated && id > 0 && subCategoryId > 0)
        {
            Upload up = new Upload(_webHostEnvironment);
            var brand = await _work.GenericRepository<Brand>().Table.FirstOrDefaultAsync(x => x.Id == id);
            brand.Title = title;
            brand.LogoUri = image != null
                ? up.Uploadfile(image, "Brand")
                : brand.LogoUri;
            brand.SubCategoryId = subCategoryId;
            await _work.GenericRepository<Brand>().UpdateAsync(brand, CancellationToken.None);
            return RedirectToAction("Brand");
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    public async Task<ActionResult> InsertBrand(string title, IFormFile? logo, int subCatId)
    {
        if (User.Identity.IsAuthenticated)
        {
            Upload up = new Upload(_webHostEnvironment);
            var img = logo != null ? up.Uploadfile(logo, "Brand") : string.Empty;
            var subCat = await _work.GenericRepository<SubCategory>().Table.FirstOrDefaultAsync(x => x.Id == subCatId);
            if (subCat == null) throw new Exception();
            await _work.GenericRepository<Brand>().AddAsync(new Brand
            {
                Title = title ?? string.Empty,
                LogoUri = img,
                SubCategoryId = subCat.Id
            }, CancellationToken.None);
            return RedirectToAction("Brand");
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> UpdateCat(int id, string title, IFormFile imageCat, bool isActiveCat)
    {
        if (User.Identity.IsAuthenticated)
        {
            Upload up = new Upload(_webHostEnvironment);
            var cat = await _work.GenericRepository<Category>().Table.FirstOrDefaultAsync(x => x.Id == id);
            cat.Name = title;
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

    public async Task<ActionResult> ManageCatDetail(string search, int index)
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

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
                        .Include(x => x.SubCategory)
                        .Include(x => x.Feature)
                        .Where(x => x.Title.Contains(search) || x.DataType == (DataType)dataType)
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
                        .Include(x => x.SubCategory)
                        .Include(x => x.Feature)
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
                    .Include(x => x.SubCategory)
                    .Include(x => x.Feature)
                    .OrderByDescending(x => x.Id).ToListAsync();
                ViewBag.SubCat = await _work.GenericRepository<SubCategory>().TableNoTracking
                    .OrderByDescending(x => x.Id)
                    .ToListAsync();
                ViewBag.Feature = await _work.GenericRepository<Feature>().TableNoTracking.OrderByDescending(x => x.Id)
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


    public async Task<ActionResult> InsertDetailCat(string title, bool isActive, string? option, DataType dataType,
        int subCatId, int featureId)
    {
        if (User.Identity.IsAuthenticated)
        {
            var feature = await _work.GenericRepository<Feature>().Table
                .FirstOrDefaultAsync(x => x.Id == featureId);
            var sub = await _work.GenericRepository<SubCategory>().Table
                .FirstOrDefaultAsync(x => x.Id == subCatId);
            await _work.GenericRepository<CategoryDetail>().AddAsync(new CategoryDetail
            {
                DataType = dataType,
                FeatureId = feature!.Id,
                ShowInSearch = isActive,
                Priority = 1,
                Title = title,
                SubCategoryId = sub!.Id,
                Option = option ?? string.Empty,
            }, CancellationToken.None);
            return RedirectToAction("ManageCatDetail");
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> UpdateSubCat(int id, string title, int catId, bool isActiveSubCat)
    {
        if (User.Identity.IsAuthenticated && id > 0 && catId > 0)
        {
            var subCat = await _work.GenericRepository<SubCategory>().Table.FirstOrDefaultAsync(x => x.Id == id);
            subCat.Name = title;
            subCat.IsActive = isActiveSubCat;
            subCat.CategoryId = catId;
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
                Priority = 1,
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
        string option, int dataType)
    {
        if (User.Identity.IsAuthenticated)
        {
            var catDetail = await _work.GenericRepository<CategoryDetail>().Table.FirstOrDefaultAsync(x => x.Id == id);
            catDetail.DataType = (DataType)dataType;
            catDetail.Title = title;
            catDetail.Option = option ?? string.Empty;
            catDetail.FeatureId = featureId;
            catDetail.ShowInSearch = isSearchCatDetail;
            await _work.GenericRepository<CategoryDetail>().UpdateAsync(catDetail, CancellationToken.None);
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
                        .Where(x => x.Name.Contains(search)).OrderByDescending(x => x.Id)
                        .ToListAsync();
                    ViewBag.SubCats = await _work.GenericRepository<SubCategory>().TableNoTracking
                        .Include(x => x.Category)
                        .OrderByDescending(x => x.Id).ToListAsync();
                }
                else
                {
                    ViewBag.Cats = await _work.GenericRepository<Category>().TableNoTracking
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
                ViewBag.Cats = await _work.GenericRepository<Category>().TableNoTracking.OrderByDescending(x => x.Id)
                    .ToListAsync();
                ViewBag.SubCats = await _work.GenericRepository<SubCategory>().TableNoTracking.Include(x => x.Category)
                    .OrderByDescending(x => x.Id).ToListAsync();
            }

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

    public async Task<ActionResult> InsertCat(string title, bool isActive, IFormFile? logo)
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
            }, CancellationToken.None);
            return RedirectToAction("ManageCategory");
        }
        else
        {
            return View("Login");
        }
    }


    public async Task<ActionResult> InsertSubCat(string title, bool isActive, int? catId)
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
                CategoryId = cat.Id
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
            ViewBag.Product = await _work.GenericRepository<Product>().TableNoTracking
                .Include(x => x.Brand)
                .Include(x => x.SubCategory)
                .Include(x => x.ProductColors).ThenInclude(x => x.Color)
                .Include(x => x.ProductColors).ThenInclude(x => x.Guarantee)
                .Include(x => x.ProductDetails).ThenInclude(x => x.CategoryDetail)
                .Include(x => x.ProductImages)
                .Include(x => x.Offer).ThenInclude(q => q.Color)
                .OrderByDescending(x => x.Id).FirstOrDefaultAsync(x => x.Id == id);
            ViewBag.Brands = await _work.GenericRepository<Brand>().TableNoTracking.OrderByDescending(x => x.Id)
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


    public async Task<ActionResult> ProductManage(string search)
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            if (!string.IsNullOrWhiteSpace(search))
            {
                ViewBag.Products = await _work.GenericRepository<Product>().TableNoTracking
                    .Include(x => x.Brand)
                    .Include(x => x.SubCategory)
                    .Include(x => x.ProductColors).ThenInclude(x => x.Color)
                    .Include(x => x.ProductDetails)
                    .Where(x => x.SubCategory.Name.Contains(search) || x.Brand.Title.Contains(search) ||
                                x.Code.Contains(search) || x.Detail.Contains(search) || x.Strengths.Contains(search) ||
                                x.FullDesc.Contains(search) || x.MetaDesc.Contains(search) ||
                                x.MetaKeyword.Contains(search) || x.FullDesc.Contains(search) ||
                                x.PersianTitle.Contains(search) || x.WeakPoints.Contains(search) ||
                                x.ProductGift.Contains(search) ||
                                x.ProductColors.Select(t => t.Color).FirstOrDefault().ColorCode.Contains(search) ||
                                x.ProductColors.Select(t => t.Color).FirstOrDefault().Title.Contains(search) ||
                                x.ProductDetails.Select(q => q).FirstOrDefault().Value.Contains(search))
                    .OrderByDescending(x => x.Id).ToListAsync();
            }
            else
            {
                ViewBag.Products = await _work.GenericRepository<Product>().TableNoTracking
                    .Include(x => x.Brand)
                    .Include(x => x.SubCategory)
                    .Include(x => x.ProductColors).ThenInclude(x => x.Color)
                    .OrderByDescending(x => x.Id).ToListAsync();
            }

            ViewBag.Brands = await _work.GenericRepository<Brand>().TableNoTracking.OrderByDescending(x => x.Id)
                .ToListAsync();

            ViewBag.SubCats = await _work.GenericRepository<SubCategory>().TableNoTracking.OrderByDescending(x => x.Id)
                .ToListAsync();
            ViewBag.Colors = await _work.GenericRepository<Color>().TableNoTracking.OrderByDescending(x => x.Id)
                .ToListAsync();
            ViewBag.Guarantee = await _work.GenericRepository<Guarantee>().TableNoTracking.OrderByDescending(x => x.Id)
                .ToListAsync();

            #endregion

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
            ViewBag.Brands = await _work.GenericRepository<Brand>().TableNoTracking.Include(x => x.SubCategory)
                .Where(x => x.Title.Contains(search) ||
                            x.SubCategory.Name.Contains(search)).OrderByDescending(x => x.Id).ToListAsync();
        }
        else
        {
            ViewBag.Brands = await _work.GenericRepository<Brand>().TableNoTracking.Include(x => x.SubCategory)
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


    public async Task<IActionResult> UpdateBanner(BannerDto request)
    {
        if (request.Id <= 0)
        {
            Upload up = new Upload(_webHostEnvironment);

            await _work.GenericRepository<Banner>().AddAsync(new Banner
            {
                SmallBannerMiddle1 = up.Uploadfile(request.SmallBannerMiddle1, "Banner"),
                BigBannerMiddle1 = up.Uploadfile(request.BigBannerMiddle1, "Banner"),
                BigBannerMiddle2 = up.Uploadfile(request.BigBannerMiddle2, "Banner"),
                LargeSideBanner = up.Uploadfile(request.LargeSideBanner, "Banner"),
                SmallBannerMiddle2 = up.Uploadfile(request.SmallBannerMiddle2, "Banner"),
                SmallBannerMiddle3 = up.Uploadfile(request.SmallBannerMiddle3, "Banner"),
                SmallBannerMiddle4 = up.Uploadfile(request.SmallBannerMiddle4, "Banner"),
                SmallSideBanner = up.Uploadfile(request.SmallSideBanner, "Banner"),
                BigBannerMiddle1Href = request.BigBannerMiddle1Href,
                BigBannerMiddle2Href = request.BigBannerMiddle2Href,
                LargeSideBannerHref = request.LargeSideBannerHref,
                SmallBannerMiddle1Href = request.SmallBannerMiddle1Href,
                SmallBannerMiddle2Href = request.SmallBannerMiddle2Href,
                SmallBannerMiddle3Href = request.SmallBannerMiddle3Href,
                SmallBannerMiddle4Href = request.SmallBannerMiddle4Href,
                SmallSideBannerHref = request.SmallSideBannerHref,
                BigBannerMiddle1Col = up.Uploadfile(request.BigBannerMiddle1Col, "Banner"),
                BigBannerMiddle2Col = up.Uploadfile(request.BigBannerMiddle2Col, "Banner"),
                BigBannerMiddle1ColHref = request.BigBannerMiddle1ColHref,
                BigBannerMiddle2ColHref = request.BigBannerMiddle2ColHref,
            }, CancellationToken.None);
        }
        else
        {
            Upload up = new Upload(_webHostEnvironment);
            var banners = await _work.GenericRepository<Banner>().GetByIdAsync(CancellationToken.None, request.Id);
            banners.SmallBannerMiddle1 = request.SmallBannerMiddle1 != null
                ? up.Uploadfile(request.SmallBannerMiddle1, "Banner")
                : banners.SmallBannerMiddle1;
            banners.BigBannerMiddle1 = request.BigBannerMiddle1 != null
                ? up.Uploadfile(request.BigBannerMiddle1, "Banner")
                : banners.BigBannerMiddle1;
            banners.BigBannerMiddle2 = request.BigBannerMiddle2 != null
                ? up.Uploadfile(request.BigBannerMiddle2, "Banner")
                : banners.BigBannerMiddle2;
            banners.LargeSideBanner = request.LargeSideBanner != null
                ? up.Uploadfile(request.LargeSideBanner, "Banner")
                : banners.LargeSideBanner;
            banners.SmallBannerMiddle2 = request.SmallBannerMiddle2 != null
                ? up.Uploadfile(request.SmallBannerMiddle2, "Banner")
                : banners.SmallBannerMiddle2;
            banners.SmallBannerMiddle3 = request.SmallBannerMiddle3 != null
                ? up.Uploadfile(request.SmallBannerMiddle3, "Banner")
                : banners.SmallBannerMiddle3;
            banners.SmallBannerMiddle4 = request.SmallBannerMiddle4 != null
                ? up.Uploadfile(request.SmallBannerMiddle4, "Banner")
                : banners.SmallBannerMiddle4;
            banners.SmallSideBanner = request.SmallSideBanner != null
                ? up.Uploadfile(request.SmallSideBanner, "Banner")
                : banners.SmallSideBanner;
            banners.BigBannerMiddle1Href = request.BigBannerMiddle1Href;
            banners.BigBannerMiddle2Href = request.BigBannerMiddle2Href;
            banners.LargeSideBannerHref = request.LargeSideBannerHref;
            banners.SmallBannerMiddle1Href = request.SmallBannerMiddle1Href;
            banners.SmallBannerMiddle2Href = request.SmallBannerMiddle2Href;
            banners.SmallBannerMiddle3Href = request.SmallBannerMiddle3Href;
            banners.SmallBannerMiddle4Href = request.SmallBannerMiddle4Href;
            banners.SmallSideBannerHref = request.SmallSideBannerHref;

            banners.BigBannerMiddle1Col = up.Uploadfile(request.BigBannerMiddle1Col, "Banner");
            banners.BigBannerMiddle2Col = up.Uploadfile(request.BigBannerMiddle2Col, "Banner");
            banners.BigBannerMiddle1ColHref = request.BigBannerMiddle1ColHref;
            banners.BigBannerMiddle2ColHref = request.BigBannerMiddle2ColHref;
            await _work.GenericRepository<Banner>().UpdateAsync(banners, CancellationToken.None);
        }

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
            var footer = await _work.GenericRepository<FooterLink>().GetByIdAsync(CancellationToken.None, request.Id);
            var entity = _mapper!.Map<FooterLink>(request);
            entity.Id = footer.Id;
            await _work.GenericRepository<FooterLink>().UpdateAsync(entity, CancellationToken.None);
        }

        return RedirectToAction("ManageFactor");
    }

    public async Task<IActionResult> UpdateSlider(BannerDto request)
    {
        if (request.Id <= 0)
        {
            Upload up = new Upload(_webHostEnvironment);

            await _work.GenericRepository<Banner>().AddAsync(new Banner
            {
                SliderImage = up.Uploadfile(request.SliderImage, "Banner"),
                SliderImage1 = up.Uploadfile(request.SliderImage1, "Banner"),
                SliderImage2 = up.Uploadfile(request.SliderImage2, "Banner"),
                SliderHref = request.SliderHref,
                SliderHref1 = request.SliderHref1,
                SliderHref2 = request.SliderHref2,
                SliderTitle = request.SliderTitle,
                SliderTitle1 = request.SliderTitle1,
                SliderTitle2 = request.SliderTitle2,
            }, CancellationToken.None);
        }
        else
        {
            Upload up = new Upload(_webHostEnvironment);
            var banners = await _work.GenericRepository<Banner>().GetByIdAsync(CancellationToken.None, request.Id);
            banners.SliderHref = request.SliderHref;
            banners.SliderHref1 = request.SliderHref1;
            banners.SliderHref2 = request.SliderHref2;
            banners.SliderImage = request.SliderImage != null
                ? up.Uploadfile(request.SliderImage, "Banner")
                : banners.SliderImage;
            banners.SliderImage1 = request.SliderImage1 != null
                ? up.Uploadfile(request.SliderImage1, "Banner")
                : banners.SliderImage1;
            banners.SliderImage2 = request.SliderImage2 != null
                ? up.Uploadfile(request.SliderImage2, "Banner")
                : banners.SliderImage2;

            banners.SliderTitle = request.SliderTitle;
            banners.SliderTitle1 = request.SliderTitle1;
            banners.SliderTitle2 = request.SliderTitle2;
            await _work.GenericRepository<Banner>().UpdateAsync(banners, CancellationToken.None);
        }

        return RedirectToAction("ManageSlider");
    }

    public async Task<ActionResult> Login()
    {
        return View();
    }

    public async Task<ActionResult> LoginPassword(string phoneNumber)
    {
        ViewBag.exUser = await _mediator.Send(new AdminExistCommand(phoneNumber));
        return View("LoginPassword");
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
        var user = await _mediator.Send(new ConfirmCodAdminCommand(phoneNumber, code));
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
                    .Where(x => x.Title.Contains(search) || x.Desc.Contains(search))
                    .ToListAsync();
            }
            else
            {
                ViewBag.Faqs = await _work.GenericRepository<Faq>().TableNoTracking.ToListAsync();
            }

            return View();
        }
        else
        {
            return RedirectToAction("Login");
        }
    }

    public async Task<ActionResult> UpdateFaq(int id, string title, string desc)
    {
        if (User.Identity.IsAuthenticated)
        {
            var faq = await _work.GenericRepository<Faq>().Table.FirstOrDefaultAsync(x => x.Id == id);
            faq.Title = title;
            faq.Desc = desc;
            await _work.GenericRepository<Faq>().UpdateAsync(faq, CancellationToken.None);
            return RedirectToAction("ManageFaq");
        }
        else
        {
            return RedirectToAction("Login");
        }
    }

    public async Task<ActionResult> InsertFaq(string title, string desc)
    {
        if (User.Identity.IsAuthenticated)
        {
            await _work.GenericRepository<Faq>().AddAsync(new Faq
            {
                Desc = desc,
                Title = title,
            }, CancellationToken.None);
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
                IsActive = true
            }, CancellationToken.None);
            return RedirectToAction("ManageSearchResult");
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
                        Roles = a.ToList()
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


    public async Task<ActionResult> InsertProduct(Root request)
    {
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
        await _mediator.Send(new InsertProductCommand
        {
            Product = request
        });
        return RedirectToAction("ProductManage");
    }

    public async Task<ActionResult> UpdateProduct(Root request)
    {
        var product = await _work.GenericRepository<Product>().Table.FirstOrDefaultAsync(x => x.Id == request.Id);
        if (product == null) throw new Exception();
        Upload up = new Upload(_webHostEnvironment);

        #region prod

        product.Title = request.Title;
        product.PersianTitle = request.PersianTitle;
        product.Detail = request.Detail;
        product.MetaDesc = request.MetaDesc;
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
                var val = request.ProductDetails.FirstOrDefault(x => x.DetailId.ToInt() == i.Id);
                if (val != null)
                {
                    i.Value = val.DetailName;
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
                offer.Days = request.Offer.Days.ToInt();
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
                    Days = request.Offer.Days.ToInt(),
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

        return RedirectToAction("ProductManage");
    }
    // string Title, string PersianTitle, string Detail, string MetaDesc,
    // string MetaKeyword, string FullDesc, string[] ImageUri, string ProductGift, double DiscountAmount, int BrandId,
    // int SubCategoryId, string[] Images, ProductDetail[] ProductDetails, ProductColor[] ProductColors,
    //     Offer Offer, ProductStatus ProductStatus, bool IsActive, bool IsOffer

    public async Task<List<DetailProdAdmin>> GetCategoryDetailBySubCatId(int subCatId)
    {
        var detail = await _work.GenericRepository<CategoryDetail>()
            .TableNoTracking
            .Include(x=>x.SubCategory).ThenInclude(x=>x.Category)
            .Include(x => x.Feature)
            .Where(x => x.SubCategoryId == subCatId).OrderByDescending(x => x.Id).ToListAsync();
        List<DetailProdAdmin> detailProdAdmins = new List<DetailProdAdmin>();
        foreach (var i in detail)
        { 
            if (detailProdAdmins.Any(x => x.FeatureId == i.FeatureId))
            {
                var result = detailProdAdmins.FirstOrDefault(x => x.FeatureId == i.FeatureId);
                result.CategoryDetails.Add(new CategoryDetailDto
                {
                    Option = i.Option, DataType = (int)i.DataType, Priority = i.Priority, Title = i.Title,
                    FeatureId = i.FeatureId, ShowInSearch = i.ShowInSearch, SubCategoryId = i.SubCategoryId,Id = i.Id
                });
            }
            else
            {
                var li = new DetailProdAdmin();
                li.Feature = i.Feature;
                li.FeatureId = i.FeatureId;
                li.CategoryDetails.Add(new CategoryDetailDto
                {
                    Option = i.Option,DataType = (int)i.DataType,Priority = i.Priority,Title = i.Title,FeatureId = i.FeatureId,ShowInSearch = i.ShowInSearch,SubCategoryId = i.SubCategoryId,Id = i.Id
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
}