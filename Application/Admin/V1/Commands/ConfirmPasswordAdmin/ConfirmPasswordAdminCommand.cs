using Domain.Entity.User;
using MediatR;

namespace Application.Admin.V1.Commands.ConfirmPasswordAdmin;

public record ConfirmPasswordAdminCommand(string Password,string PhoneNumber):IRequest<User>;