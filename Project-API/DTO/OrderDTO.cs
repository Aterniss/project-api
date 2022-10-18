namespace Project_API.DTO
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public string OrderStatus { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int RiderId { get; set; }
        public int IdUser { get; set; }

        public virtual UserDTO IdUserNavigation { get; set; } = null!;
        public virtual RiderDTO Rider { get; set; } = null!;
        public virtual ICollection<OrderDishDTO> OrderDishes { get; set; }
    }
}
