using Application.Interfaces;
using Domain.Entity.User;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Admin.Ui.Components.Pages.BasicInformation;

public partial class ManagePostMethod
{
    [Inject] private IUnitOfWork _work { set; get; }
    private List<PostMethod> _postMethods = new();

    protected override async Task OnInitializedAsync()
    {
        _postMethods = await _work.GenericRepository<PostMethod>().TableNoTracking.ToListAsync();
    }
}