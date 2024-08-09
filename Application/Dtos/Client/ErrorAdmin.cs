using Application.Common.ApiResult;

namespace Application.Dtos.Client;

public class ErrorAdmin
{
    public string Message { get; set; } = string.Empty;
    public bool IsError { get; set; }
    public ApiResultStatusCode StatusCode { get; set; }
}