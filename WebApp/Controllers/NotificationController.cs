using Application.Interfaces;
using Domain.Entity.Notif;
using Domain.Entity.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers;

public class NotificationController : Controller
{
    private readonly IUnitOfWork _work;
    private readonly UserManager<User> _userManager;

    public NotificationController(IUnitOfWork work, UserManager<User> userManager)
    {
        _work = work;
        _userManager = userManager;
    }

    public async Task<IActionResult> InsertInventoryNotification(int id)
    {
        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
            if (!await _work.GenericRepository<InventoryNotification>().TableNoTracking
                    .AnyAsync(x => x.UserId == user.Id && x.ProductColorId == id))
            {
                await _work.GenericRepository<InventoryNotification>().AddAsync(new InventoryNotification
                {
                    ProductColorId = id,
                    InsertDate = DateTime.Now,
                    UserId = user.Id
                }, CancellationToken.None);
            }
            
            var previousUrl = Request.Headers["Referer"];
            if (!string.IsNullOrEmpty(previousUrl))
            {
                return Redirect(previousUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        else
        {
            return RedirectToAction("Login","Home");
        }
    }
}