using Application.Admin.V1.Commands.AdminExist;
using Application.Admin.V1.Commands.ConfirmCodAdmin;
using Application.Admin.V1.Commands.ConfirmPasswordAdmin;
using Application.Admin.V1.Commands.LoginCodAdmin;
using Application.Admin.V1.Queries.CheckAdminNumber;
using Application.Common.ApiResult;
using Application.Dtos.Factor;
using Application.Dtos.Products;
using Application.Dtos.User;
using Application.Interfaces;
using Application.Products.Commands;
using Domain.Entity.IndexPage;
using Domain.Entity.Product;
using Domain.Entity.User;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers;

public class AdminController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IUnitOfWork _work;

    public AdminController(UserManager<User> userManager, SignInManager<User> signInManager, IMediator mediator,
        IWebHostEnvironment webHostEnvironment, RoleManager<Role> roleManager, IUnitOfWork work)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mediator = mediator;
        _webHostEnvironment = webHostEnvironment;
        _roleManager = roleManager;
        _work = work;
    }

    public async Task<ActionResult> Index()
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            #endregion

            return View();
        }
        else
        {
            return View("Login");
        }
    }


    public async Task<ActionResult> Brand()
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            ViewBag.Brands = await _work.GenericRepository<Brand>().TableNoTracking.Include(x => x.SubCategory)
                .ToListAsync();
            ViewBag.SubCats = await _work.GenericRepository<SubCategory>().TableNoTracking.ToListAsync();

            #endregion

            return View();
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> InsertBrand(string title, string? desc, IFormFile? logo, int subCatId)
    {
        if (User.Identity.IsAuthenticated)
        {
            Upload up = new Upload(_webHostEnvironment);
            var img = logo != null ? up.Uploadfile(logo, "Brand") : string.Empty;
            var subCat = await _work.GenericRepository<SubCategory>().Table.FirstOrDefaultAsync(x => x.Id == subCatId);
            if (subCat == null) throw new Exception();
            await _work.GenericRepository<Brand>().AddAsync(new Brand
            {
                Desc = desc ?? string.Empty,
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

    public async Task<ActionResult> ManageCatDetail()
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            ViewBag.CatDetail = await _work.GenericRepository<CategoryDetail>().TableNoTracking
                .Include(x => x.SubCategory)
                .Include(x => x.Feature)
                .ToListAsync();
            ViewBag.SubCat = await _work.GenericRepository<SubCategory>().TableNoTracking.ToListAsync();
            ViewBag.Feature = await _work.GenericRepository<Feature>().TableNoTracking.ToListAsync();

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

    public async Task<ActionResult> ManageCategorey()
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            ViewBag.Cats = await _work.GenericRepository<Category>().TableNoTracking.ToListAsync();
            ViewBag.SubCats = await _work.GenericRepository<SubCategory>().TableNoTracking.Include(x => x.Category)
                .ToListAsync();

            #endregion

            return View();
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
            return RedirectToAction("ManageCategorey");
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
                return RedirectToAction("ManageCategorey");
            }

            await _work.GenericRepository<SubCategory>().AddAsync(new SubCategory
            {
                Name = title,
                IsActive = isActive,
                CategoryId = cat.Id
            }, CancellationToken.None);
            return RedirectToAction("ManageCategorey");
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> Color()
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            ViewBag.Colors = await _work.GenericRepository<Color>().TableNoTracking.ToListAsync();

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


    public async Task<ActionResult> ProductManage()
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            ViewBag.Brands = await _work.GenericRepository<Brand>().TableNoTracking.ToListAsync();
            ViewBag.Products = await _work.GenericRepository<Product>().TableNoTracking
                .Include(x=>x.Brand)
                .Include(x=>x.SubCategory)
                .Include(x=>x.ProductColors).ThenInclude(x=>x.Color)
                .ToListAsync();
            ViewBag.SubCats = await _work.GenericRepository<SubCategory>().TableNoTracking.ToListAsync();
            ViewBag.Colors = await _work.GenericRepository<Color>().TableNoTracking.ToListAsync();
            ViewBag.Guarantee = await _work.GenericRepository<Guarantee>().TableNoTracking.ToListAsync();

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
        ViewBag.Banner = await _work.GenericRepository<Banner>().TableNoTracking.ToListAsync();
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

    public async Task<ActionResult> GoToLoginWithCode()
    {
        return View("ConfirmCode");
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
        var result = await _signInManager.PasswordSignInAsync(user, user.Password, true, false);
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
        return View("Login");
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

    public async Task<ActionResult> ManageUser(int page = 1)
    {
        if (User.Identity.IsAuthenticated)
        {
            ViewBag.Users = await _userManager.Users.ToListAsync();
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

    // string Title, string PersianTitle, string Detail, string MetaDesc,
    // string MetaKeyword, string FullDesc, string[] ImageUri, string ProductGift, double DiscountAmount, int BrandId,
    // int SubCategoryId, string[] Images, ProductDetail[] ProductDetails, ProductColor[] ProductColors,
    //     Offer Offer, ProductStatus ProductStatus, bool IsActive, bool IsOffer
    public async Task<List<CategoryDetail>> GetCategoryDetailBySubCatId(int subCatId)
    {
        return await _work.GenericRepository<CategoryDetail>()
            .TableNoTracking
            .Include(x => x.Feature)
            .Where(x => x.SubCategoryId == subCatId).ToListAsync();
    }

    public async Task<ActionResult> Product(string search, int catId, int page = 1)
    {
        if (User.Identity.IsAuthenticated)
        {
            ViewBag.productCount = _work.GenericRepository<Product>().TableNoTracking.Count();
            ViewBag.Products = _work.GenericRepository<Product>().TableNoTracking.Include(x => x.SubCategory)
                .Include(x => x.ProductColors).ThenInclude(x => x.Color).Count();
            ViewBag.categories = _work.GenericRepository<Category>().TableNoTracking.ToList();


            ViewBag.productsPage = page;

            return View("Product");
        }
        else
        {
            return View("Index");
        }
    }
}