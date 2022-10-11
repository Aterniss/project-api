namespace Project_API.DTO
{
    public class RestaurantDTO
    {

        public int RestaurantId { get; set; }
        public string? RestaurantName { get; set; }
        public string? CategoryName { get; set; }
        public string? RestaurantAddress { get; set; }
        public int ZoneId { get; set; }

        public virtual FoodCategoryDTO? CategoryNameNavigation { get; set; }
        public virtual ZoneDTO Zone { get; set; } = null!;
        public virtual ICollection<DishDTO> Dishes { get; set; }


    }
}
