using Application.Common.ApiResult;

namespace Panel.Models;

public class ErrorAdmin
{
    public string Message { get; set; } = string.Empty;
    public bool IsError { get; set; }
    public ApiResultStatusCode StatusCode { get; set; }
}