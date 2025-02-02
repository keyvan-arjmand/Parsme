using Application.Interfaces;
using Domain.Entity.Factor;
using Domain.Entity.Factor.Product;
using Domain.Entity.Product;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace WebApp.Jobs;

public class CounterProduct : IJob
{
    private readonly IUnitOfWork _unitOfWork;

    public CounterProduct(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var factors = await _unitOfWork.GenericRepository<Factor>().Table
            .Include(x => x.Products).ThenInclude(x => x.FactorProductColor)
            .Where(x => !x.IsCounter && x.IsPay && x.SaleReferenceId > 0)
            .ToListAsync();

        if (factors.Any())
        {
            foreach (var factor in factors)
            {
                var fac = factor;
                foreach (var product in factor.Products)
                {
                    foreach (var color in product.FactorProductColor)
                    {
                        var colorFactor = await _unitOfWork.GenericRepository<ProductColor>().Table
                            .FirstOrDefaultAsync(x => x.ColorId == color.ColorId && x.ProductId == product.ProductId);
                        colorFactor!.Inventory -= color.Count;
                        await _unitOfWork.GenericRepository<ProductColor>()
                            .UpdateAsync(colorFactor, CancellationToken.None);
                        fac.IsCounter = true;
                    }
                }
                await _unitOfWork.GenericRepository<Factor>().UpdateAsync(fac, CancellationToken.None);
            }
        }
    }
}