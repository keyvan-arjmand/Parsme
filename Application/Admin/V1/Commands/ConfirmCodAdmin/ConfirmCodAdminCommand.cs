using Application.Dtos;
using Domain.Entity.User;
using MediatR;

namespace Application.Admin.V1.Commands.ConfirmCodAdmin;

public record ConfirmCodAdminCommand(string PhoneNumber , string Cod):IRequest<ApiAction>;