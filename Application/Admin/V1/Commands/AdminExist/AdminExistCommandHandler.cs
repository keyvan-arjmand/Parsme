using Application.Dtos.User;
using Domain.Entity.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Admin.V1.Commands.AdminExist;

public class AdminExistCommandHandler:IRequestHandler<AdminExistCommand,UserLoginDto>
{
    private readonly UserManager<User> _userManager;

    public AdminExistCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserLoginDto> Handle(AdminExistCommand request, CancellationToken cancellationToken)
    {
        // if (request.PhoneNumber.IsPhone())     
        //     throw new Exception("Invalid Phone Number");
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