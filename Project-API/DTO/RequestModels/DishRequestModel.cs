namespace Project_API.DTO.RequestModels
{
    public class DishRequestModel
    {
        public string? DishName { get; set; }
        public string DishDescription { get; set; } = null!;
        public int RestaurantId { get; set; }
        public decimal Price { get; set; }
        public bool? Require18 { get; set; }
    }
}
