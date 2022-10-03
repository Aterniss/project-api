namespace Project_API.DTO
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public string OrderStatus { get; set; } = null!;
        public byte[] CreatedAt { get; set; } = null!;
        public int RiderId { get; set; }
        public int IdUser { get; set; }
    }
}
