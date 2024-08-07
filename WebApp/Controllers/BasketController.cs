using Application.Interfaces;
using Domain.Entity.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace WebApp.Controllers;

public class BasketController : Controller
{
    private readonly IUnitOfWork _work;

    public BasketController(IUnitOfWork work)
    {
        _work = work;
    }

    public async Task<IActionResult> AddToBasket(int id)
    {
        var basketlist = new List<int>();

        if (HttpContext.Session.GetString("basket") != null)
        {
            basketlist = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("basket")).ToList();
            basketlist.Add(id);
            HttpContext.Session.SetString("basket", JsonConvert.SerializeObject(basketlist));
        }
        else
        {
            basketlist.Add(id);
            HttpContext.Session.SetString("basket", JsonConvert.SerializeObject(basketlist));
        }

        return RedirectToAction("Basket", "Home");
    }
    public IActionResult RemoveInBasket(int id)
    {
        var basketlist = new List<int>();
        if (HttpContext.Session.GetString("basket") != null)
        {
            basketlist = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("basket")).ToList();
        }

        basketlist.Remove(id);
        HttpContext.Session.SetString("basket", JsonConvert.SerializeObject(basketlist));
        return RedirectToAction("Basket", "Home");
    }
    public async Task<IActionResult> DiscountCheck(string code)
    {
        var result =await _work.GenericRepository<DiscountCode>().TableNoTracking
            .FirstOrDefaultAsync(x => x.Code == code && x.IsActive && x.Count > 0);
        if (result!=null)
        {
            HttpContext.Session.SetString("discountCode", result.Code);
            return RedirectToAction("Basket", "Home");
        }
        else
        {
            HttpContext.Session.SetString("discountCode", string.Empty);
            return RedirectToAction("Basket", "Home");
        }
    }
}