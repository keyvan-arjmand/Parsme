using Application;
using Domain.DataBase;
using Domain.Entity.User;
using Infrastructure.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddSession(option => { option.IdleTimeout = TimeSpan.FromMinutes(20); });
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddDbContext<AppDbContext>(s =>
    s.UseSqlServer(
        "Data Source=185.165.118.72,1433;Initial Catalog=Develop;User ID=key1;Password=7Dwuv15@;Trust Server Certificate=True"));
builder.Services.AddIdentity<User, Role>(option =>
    {
        option.Password.RequireDigit = false;
        option.Password.RequireLowercase = false;
        option.Password.RequireNonAlphanumeric = false;
        option.Password.RequireUppercase = false;
        option.Password.RequiredLength = 4;
        option.SignIn.RequireConfirmedPhoneNumber = false;
    })
    .AddUserManager<UserManager<User>>()
    .AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .SetIsOriginAllowed((host) => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Admin/AccessDenied";
    options.Cookie.Name = "WebAppIdentityCooclie";
    options.ExpireTimeSpan = TimeSpan.FromHours(2); 
    options.LoginPath = "/Admin/Login";
    options.SlidingExpiration = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();