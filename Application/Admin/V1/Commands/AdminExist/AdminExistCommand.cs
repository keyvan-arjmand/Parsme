using Application.Dtos.User;
using MediatR;

namespace Application.Admin.V1.Commands.AdminExist;

public record AdminExistCommand(string PhoneNumber):IRequest<UserLoginDto>;