namespace Project_API.DTO.RequestModels
{
    public class OrderUpdateRequest
    {
        public string OrderStatus { get; set; } = null!;
        public int RiderId { get; set; }
        public int IdUser { get; set; }
    }
}
