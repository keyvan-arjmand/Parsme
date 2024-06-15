using Domain.Entity.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Admin.V1.Commands.ConfirmCodAdmin;

public class ConfirmCodAdminCommandHandler : IRequestHandler<ConfirmCodAdminCommand, User>
{
    private readonly UserManager<User> _userManager;

    public ConfirmCodAdminCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<User> Handle(ConfirmCodAdminCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.PhoneNumber);
        if (user == null) throw new Exception("User not found");
        if (!user.ConfirmCode.Equals(request.Cod))
        {
            throw new Exception("Code Invalid");
        }
        else if (DateTime.Now >= user.ConfirmCodeExpireTime)
        {
            throw new Exception("Code Expire");
        }

        user.PhoneNumberConfirmed = true;
        await _userManager.UpdateAsync(user);
        return user;
    }
}