using Domain.Enums;
using MediatR;

namespace Application.Factors.Commands.SendMessageUser;

public class SendMessageUserCommand:IRequest
{
    public string Number { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string Token2 { get; set; }= string.Empty;
    public string Token3 { get; set; }= string.Empty;
    public Status Status { get; set; }
    public ReturnedStatus ReturnedStatus { get; set; }
    public bool IsReturn { get; set; } = false;
}