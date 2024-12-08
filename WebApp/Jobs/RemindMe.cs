using Application.Constants.Kavenegar;
using Application.Interfaces;
using Domain.Entity.Notif;
using Domain.Entity.Product;
using Domain.Entity.User;
using Kavenegar;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace WebApp.Jobs;

public class RemindMe : IJob
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;

    public RemindMe(IUnitOfWork unitOfWork, UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            var reminds = await _unitOfWork.GenericRepository<InventoryNotification>().TableNoTracking
                .Where(x => !x.IsRemind).ToListAsync();

            foreach (var i in reminds)
            {
                var prod = await _unitOfWork.GenericRepository<Product>().TableNoTracking.Include(x => x.ProductColors)
                    .FirstOrDefaultAsync(x => x.Id == i.ProductId);
                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == i.UserId);
                if (prod.ProductColors.Any(x => x.Id == i.ProductColorId && x.Inventory < 0))
                {
                    KavenegarApi webApi = new KavenegarApi(apikey: ApiKeys.ApiKey);
                    var result = webApi.VerifyLookup(user.PhoneNumber, user.Name, prod.PersianTitle.Substring(0, 25),
                        string.Empty,
                        "RemindMe");
                    i.IsRemind = true;
                    await _unitOfWork.GenericRepository<InventoryNotification>().UpdateAsync(i, CancellationToken.None);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}