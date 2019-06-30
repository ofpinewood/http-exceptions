
namespace Opw.HttpExceptions.AspNetCore.Sample.CustomErrors
{
    public class CustomError
    {
        public string Message { get; set; }
        public string Type { get; set; }
        public int Status { get; set; }
        public int Code { get; set; }
        public string TraceId { get; set; }
    }
}
