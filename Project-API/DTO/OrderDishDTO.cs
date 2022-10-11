

namespace Project_API.DTO
{
    public class OrderDishDTO
    {
        //public int Id { get; set; }
        public int OrderId { get; set; }
        public int DishId { get; set; }

         public virtual DishDTO Dish { get; set; } = null!;
        //public virtual OrderDTO Order { get; set; } = null!;
    }
}
