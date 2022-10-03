namespace Project_API.DTO.RequestModels
{
    public class UserRequestModel
    {
        public string FullName { get; set; } = null!;
        public string UserAddress { get; set; } = null!;
        public bool? IsOver18 { get; set; }
    }
}
