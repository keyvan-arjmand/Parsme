using Domain.Entity.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Admin.V1.Commands.ConfirmPasswordAdmin;

public class ConfirmPasswordAdminCommandHandler : IRequestHandler<ConfirmPasswordAdminCommand,User>
{
    private readonly UserManager<User> _userManager;

    public ConfirmPasswordAdminCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<User> Handle(ConfirmPasswordAdminCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.PhoneNumber);
        var userRoles = await _userManager.GetRolesAsync(user);
        if (user == null && userRoles.Any(x => !x.Equals("admin")))
            throw new Exception("User not Exist");
        return user;
     
    }
}