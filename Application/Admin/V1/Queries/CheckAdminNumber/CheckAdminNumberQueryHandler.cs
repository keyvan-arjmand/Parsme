using Application.Dtos.User;
using Domain.Entity.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Admin.V1.Queries.CheckAdminNumber;

public class CheckAdminNumberQueryHandler:IRequestHandler<CheckAdminNumberQuery,UserLoginDto>
{
    private readonly UserManager<User> _userManager;
    public CheckAdminNumberQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    public async Task<UserLoginDto> Handle(CheckAdminNumberQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.PhoneNumber);
        var userRoles = await _userManager.GetRolesAsync(user);
        if (user == null && userRoles.Any(x => !x.Equals("admin")))
            throw new Exception("User not Exist");
        return new UserLoginDto
        {   
            Family = user.Family,
            Name = user.Name,
            Id = user.Id,
            Image = user.ImageName,
            PhoneNumber = user.PhoneNumber
        };
    }
}