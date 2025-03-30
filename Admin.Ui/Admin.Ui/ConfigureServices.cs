using Application;
using Domain.DataBase;
using Domain.Entity.User;
using Infrastructure.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

namespace Admin.Ui;

public static class ConfigureServices
{
    public static IServiceCollection AddUiServices(this IServiceCollection services)
    {
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromHours(3);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        services.AddDbContext<AppDbContext>(s =>
            s.UseSqlServer(
                "Data Source=185.165.118.72;Initial Catalog=NewPars;User ID=pars;Password=I$w225am;Trust Server Certificate=True"));

        services.AddIdentity<User, Role>(option =>
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

        services.Configure<SecurityStampValidatorOptions>(options =>
        {
            options.ValidationInterval = TimeSpan.FromHours(3);
        });

        services.AddCors(options =>
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

        services.ConfigureApplicationCookie(options =>
        {
            options.AccessDeniedPath = "/Admin/AccessDenied";
            options.Cookie.Name = "webappPanel";
            options.ExpireTimeSpan = TimeSpan.FromHours(3);
            options.LoginPath = "/Admin/Login";
            options.SlidingExpiration = true;
        });
        services.AddDistributedMemoryCache();
        services.AddApplicationServices();
        services.AddInfrastructureServices();
        services.AddAutoMapper(typeof(Program));
        services.AddMudServices();
        return services;
    }
}