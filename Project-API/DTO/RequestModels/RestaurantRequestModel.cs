namespace Project_API.DTO.RequestModels
{
    public class RestaurantRequestModel
    {
        public string? RestaurantName { get; set; }
        public string? CategoryName { get; set; }
        public string? RestaurantAddress { get; set; }
        public int ZoneId { get; set; }
    }
}
