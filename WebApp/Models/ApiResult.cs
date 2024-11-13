namespace WebApp.Models;

public class ApiResult
{
    public int statusCode { get; set; }
    public bool isSuccess { get; set; }
    public string message { get; set; }

    public class MelatResultResponse : ApiResult
    {
        public MelatMessage data { get; set; }
    }

    public class MelatMessage
    {
        public bool isError { get; set; }
        public string errorMsg { get; set; }
        public string succeedMsg { get; set; }
    }
}