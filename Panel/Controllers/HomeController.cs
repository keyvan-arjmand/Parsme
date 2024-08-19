using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Panel.Models;

namespace Panel.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return RedirectToAction("Index", "Admin");
    }

    public async Task<string> GetTel()
    {
        string url = "https://t.me/Bego_Mago_Channel/196?embed=1&mode=tme";
        using HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(url);

        // بررسی موفقیت‌آمیز بودن درخواست
        response.EnsureSuccessStatusCode();
        // دریافت محتوای HTML صفحه
        string htmlContent = await response.Content.ReadAsStringAsync();
        // نمایش HTML در کنسول یا ذخیره کردن آن
        return htmlContent;
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}