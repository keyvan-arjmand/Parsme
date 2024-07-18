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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTested.AspNetCore.Mvc.Utilities.Extensions;
using WebApp.Models;
using UserDto = WebApp.Models.UserDto;

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

            ViewBag.Users = await _userManager.Users.ToListAsync();
            ViewBag.Products = await _work.GenericRepository<Product>().TableNoTracking
                .Include(x => x.SubCategory)
                .Include(x => x.Brand)
                .ToListAsync();

            #endregion

            return View();
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
                    .Where(x => x.Title.Contains(search) || x.Desc.Contains(search) ||
                                x.SubCategory.Name.Contains(search)).ToListAsync();
            }
            else
            {
                ViewBag.Brands = await _work.GenericRepository<Brand>().TableNoTracking.Include(x => x.SubCategory)
                    .ToListAsync();
            }

            ViewBag.SubCats = await _work.GenericRepository<SubCategory>().TableNoTracking.ToListAsync();

            #endregion

            return View();
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> UpdateBrand(int id, string title, string desc, IFormFile image, int subCategoryId)
    {
        if (User.Identity.IsAuthenticated && id > 0 && subCategoryId > 0)
        {
            Upload up = new Upload(_webHostEnvironment);
            var brand = await _work.GenericRepository<Brand>().Table.FirstOrDefaultAsync(x => x.Id == id);
            brand.Title = title;
            brand.Desc = desc;
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

    public async Task<ActionResult> ManageCategory()
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
                .Include(x => x.Brand)
                .Include(x => x.SubCategory)
                .Include(x => x.ProductColors).ThenInclude(x => x.Color)
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
                SmallSideBannerHref = request.SmallSideBannerHref
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

        return RedirectToAction("ManageBanner");
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

    public async Task<ActionResult> ManageState(string search, int index)
    {
        if (User.Identity.IsAuthenticated)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                ViewBag.States =
                    await _work.GenericRepository<State>().TableNoTracking.Include(x => x.Cities).ToListAsync();
                ViewBag.Cities = await _work.GenericRepository<City>().TableNoTracking.Include(x => x.State)
                    .ToListAsync();
                return View();
            }
            else
            {
                if (index == 0)
                {
                    ViewBag.States =
                        await _work.GenericRepository<State>().TableNoTracking.Include(x => x.Cities)
                            .Where(x => x.Title.Contains(search)).ToListAsync();
                    ViewBag.Cities = await _work.GenericRepository<City>().TableNoTracking.Include(x => x.State)
                        .ToListAsync();
                }

                if (index == 1)
                {
                    ViewBag.States =
                        await _work.GenericRepository<State>().TableNoTracking.Include(x => x.Cities).ToListAsync();
                    ViewBag.Cities = await _work.GenericRepository<City>().TableNoTracking.Include(x => x.State)
                        .Where(x => x.Name.Contains(search) || x.State.Title.Contains(search)).ToListAsync();
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
    public async Task<ActionResult> InsertCity(string title,int stateId)
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
                var user = await _userManager.Users.ToListAsync();
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
                    x.PhoneNumber.Contains(search) || x.City.Name.Contains(search)).ToListAsync();
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

    public async Task<ActionResult> ManageGuarantee()
    {
        if (User.Identity.IsAuthenticated)
        {
            ViewBag.Guarantee = await _work.GenericRepository<Guarantee>().TableNoTracking.ToListAsync();

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

    public async Task<ActionResult> ManagePostMethod()
    {
        if (User.Identity.IsAuthenticated)
        {
            ViewBag.PostMethod = await _work.GenericRepository<PostMethod>().TableNoTracking.ToListAsync();

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