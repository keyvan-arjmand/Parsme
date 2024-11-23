using Application.Constants.Kavenegar;
using Application.Interfaces;
using Domain.Enums;
using Kavenegar;
using Kavenegar.Models.Enums;
using MediatR;

namespace Application.Factors.Commands.SendMessageUser;

public class SendMessageUserCommandHandler : IRequestHandler<SendMessageUserCommand>
{
    private readonly IUnitOfWork _work;

    public SendMessageUserCommandHandler(IUnitOfWork work)
    {
        _work = work;
    }

    public Task Handle(SendMessageUserCommand request, CancellationToken cancellationToken)
    {
        if (request.IsReturn)
        {
            var templateMapping = new Dictionary<ReturnedStatus, string>
            {
                { ReturnedStatus.Accepted, "Returned" },
                { ReturnedStatus.Rejected, "ReturnsRejected" },
                { ReturnedStatus.Pending, "ReturnsProcessed" },
                { ReturnedStatus.Returned, "ReturnsReceiveOrder" },
            };

            if (templateMapping.TryGetValue(request.ReturnedStatus, out var templateName))
            {
                var webApi = new KavenegarApi(apikey: ApiKeys.ApiKey);
                webApi.VerifyLookup(
                    request.Number,
                    request.Token.Replace(" ","-") ?? "none",
                    request.Token2.Replace(" ","-") ?? "none",
                    request.Token3.Replace(" ","-") ?? "none",
                    templateName
                );
            }
        }
        else
        {
            var templateMapping = new Dictionary<Status, string>
            {
                { Status.Accepted, "sentOrder" },
                { Status.Rejected, "RejectOrder" },
                { Status.Pending, "ProcessingOrder" },
                { Status.ReadyToSend, "ReadyToSendOrder" },
                { Status.ReadyToGet, "ReceiveinPersonOrder" }
            };

            if (templateMapping.TryGetValue(request.Status, out var templateName))
            {
                var webApi = new KavenegarApi(apikey: ApiKeys.ApiKey);
                webApi.VerifyLookup(
                    request.Number,
                    request.Token.Replace(" ","-") ?? "none",
                    request.Token2.Replace(" ","-") ?? "none",
                    request.Token3.Replace(" ","-") ?? "none",
                    templateName
                );
            }
        }

        return Task.CompletedTask;
        ;
    }
}