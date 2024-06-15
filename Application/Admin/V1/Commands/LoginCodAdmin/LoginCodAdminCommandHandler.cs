using Application.Common.Utilities;
using Application.Constants.Kavenegar;
using Application.Dtos.User;
using Domain.Entity.User;
using Kavenegar;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Admin.V1.Commands.LoginCodAdmin;

public class LoginCodAdminCommandHandler : IRequestHandler<LoginCodAdminCommand,UserLoginDto>
{
    private readonly UserManager<User> _userManager;
    public LoginCodAdminCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserLoginDto> Handle(LoginCodAdminCommand request, CancellationToken cancellationToken)
    {
        // if (request.PhoneNumber.IsPhone())     
        //     throw new Exception("Invalid Phone Number");
        var user = await _userManager.FindByNameAsync(request.PhoneNumber);
        var userRoles = await _userManager.GetRolesAsync(user);
        KavenegarApi webApi = new KavenegarApi(apikey:ApiKeys.ApiKey);
        if (user == null && userRoles.Any(x => !x.Equals("admin")))
            throw new Exception("User not Exist");
        user.ConfirmCode = Helpers.GetConfirmCode();
        user.ConfirmCodeExpireTime = DateTime.Now.AddMinutes(2.5);
        await _userManager.UpdateAsync(user);
        var result = webApi.VerifyLookup(request.PhoneNumber, user.ConfirmCode,
            "VerifyCodeFaani");
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