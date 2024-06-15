using Application.Common.Utilities;
using Newtonsoft.Json;

namespace Application.Common.ApiResult;

public class ApiResult
{
    public ApiResult(string message, ApiResultStatusCode statusCode, bool isSuccess = true)
    {
        IsSuccess = isSuccess;
        Message = message == string.Empty ? statusCode.ToDisplay() : message;
        StatusCode = statusCode;
    }
    public bool IsSuccess { get; set; }

    public ApiResultStatusCode StatusCode { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Message { get; set; }
}

public class ApiResult<T> : ApiResult
{

    public ApiResult(T data, string message, ApiResultStatusCode statusCode, bool isSuccess = true) : base(message, statusCode, isSuccess)
    {
        Data = data;
        IsSuccess = isSuccess;
        Message = message == string.Empty ? statusCode.ToDisplay() : message;
        StatusCode = statusCode;
    }
    public T Data { get; set; }
}