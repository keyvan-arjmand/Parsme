using Application.Dtos.User;
using MediatR;

namespace Application.Admin.V1.Queries.CheckAdminNumber;

public class CheckAdminNumberQuery:IRequest<UserLoginDto>
{
    public string PhoneNumber { get; set; } = string.Empty;
}