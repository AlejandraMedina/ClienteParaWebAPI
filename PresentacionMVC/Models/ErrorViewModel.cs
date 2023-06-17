namespace PresentacionMVC.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        //comentario
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}