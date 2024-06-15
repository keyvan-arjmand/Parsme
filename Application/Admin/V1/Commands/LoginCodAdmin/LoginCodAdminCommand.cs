using Application.Dtos.User;
using MediatR;

namespace Application.Admin.V1.Commands.LoginCodAdmin;

public record LoginCodAdminCommand(string PhoneNumber):IRequest<UserLoginDto>;