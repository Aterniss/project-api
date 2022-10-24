namespace Project_API.DTO.RequestModels
{
    public class AccountRequestModel
    {
        public string UserName { get; set; } = null!;
        public string UserPassword { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string? TelNumber { get; set; }
        
    }
}
