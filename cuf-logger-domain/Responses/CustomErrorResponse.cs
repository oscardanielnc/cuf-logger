
namespace cuf_admision_domain.Entities.Responses
{
    public class CustomErrorResponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public string? data { get; set; }
    }
}
