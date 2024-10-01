using Application;
using Domain.DataBase;
using Domain.Entity.User;
using Infrastructure.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(3);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices();
builder.Services.AddAutoMapper(typeof(Program));

// Configure DbContext
builder.Services.AddDbContext<AppDbContext>(s =>
    s.UseSqlServer(
        "Data Source=185.165.118.72;Initial Catalog=NewPars;User ID=pars;Password=I$w225am;Trust Server Certificate=True"));

// Configure Identity
builder.Services.AddIdentity<User, Role>(option =>
    {
        option.Password.RequireDigit = false;
        option.Password.RequireLowercase = false;
        option.Password.RequireNonAlphanumeric = false;
        option.Password.RequireUppercase = false;
        option.Password.RequiredLength = 4;
        option.SignIn.RequireConfirmedPhoneNumber = false;
        option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(3);
    })
    .AddUserManager<UserManager<User>>()
    .AddEntityFrameworkStores<AppDbContext>();

// Configure SecurityStampValidator to prevent frequent logouts
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromHours(3); // بررسی کوکی هر 3 ساعت یک بار
});

// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://127.0.0.1:5500")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

// Configure cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Admin/AccessDenied";
    options.Cookie.Name = "webappPanel";
    options.ExpireTimeSpan = TimeSpan.FromHours(3);
    options.LoginPath = "/Admin/Login";
    options.SlidingExpiration = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("AllowSpecificOrigin");

app.UseSession();
app.UseAuthentication(); // باید قبل از UseAuthorization فراخوانی شود
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();