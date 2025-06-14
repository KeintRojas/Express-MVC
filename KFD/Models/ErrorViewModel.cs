namespace KFD.Models
{
    public class ErrorViewModel
    {
        //Atributes
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
