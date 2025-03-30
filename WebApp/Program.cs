using Application;
using Domain.DataBase;
using Domain.Entity.User;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Serilog;
using WebApp.Jobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddSession(option => { option.IdleTimeout = TimeSpan.FromHours(3);
    option.IOTimeout = TimeSpan.FromHours(3);
});
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddDbContext<AppDbContext>(s =>
    s.UseSqlServer(
        "Data Source=185.165.118.72;Initial Catalog=NewPars;User ID=pars;Password=I$w225am;Trust Server Certificate=True"));
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
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .SetIsOriginAllowed((host) => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console() // برای نمایش در کنسول
    .CreateLogger();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Admin/AccessDenied";
    options.Cookie.Name = "WebAppIdentityCooclie";
    options.ExpireTimeSpan = TimeSpan.FromHours(2); 
    options.LoginPath = "/Admin/Login";
    options.SlidingExpiration = true;
});
builder.Host.UseSerilog();
builder.Host.UseSerilog();
builder.Services.AddQuartz(q =>
{
    // Create a job and trigger
    q.UseMicrosoftDependencyInjectionJobFactory(); // use DI for job creation
    q.ScheduleJob<RemindMe>(trigger => trigger
        .WithIdentity("MyJobTrigger")
        .StartNow()
        .WithSimpleSchedule(x => x
            .WithIntervalInHours(6)
            .RepeatForever()));
});

// Add Quartz Hosted Service
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
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
app.UseSession();
app.UseAuthentication(); // باید قبل از UseAuthorization فراخوانی شود
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();