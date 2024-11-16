using Application.Dtos;
using Domain.Entity.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Admin.V1.Commands.ConfirmCodAdmin;

public class ConfirmCodAdminCommandHandler : IRequestHandler<ConfirmCodAdminCommand, ApiAction>
{
    private readonly UserManager<User> _userManager;

    public ConfirmCodAdminCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApiAction> Handle(ConfirmCodAdminCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.PhoneNumber);
        if (user == null) 
        {
            return new ApiAction
            {
                IsSuccess = false,
                Message = "کاربر یافت نشد"
            };
        }

        if (!user.ConfirmCode.Equals(request.Cod) || DateTime.Now >= user.ConfirmCodeExpireTime)
        {
            return new ApiAction
            {
                IsSuccess = false,
                Message = "کد ورود نا معتبر میباشد"
            };
        }
        user.PhoneNumberConfirmed = true;
        await _userManager.UpdateAsync(user);

        return new ApiAction
        {
            IsSuccess = true,
            Message = "ورود موفقیت آمیز"
        };
    }
}