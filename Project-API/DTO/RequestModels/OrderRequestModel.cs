namespace Project_API.DTO.RequestModels
{
    public class OrderRequestModel
    {
        public string OrderStatus { get; set; } = null!;
        public int RiderId { get; set; }
        public int IdUser { get; set; }

        public List<int> Dishes { get; set; }
    }
}
