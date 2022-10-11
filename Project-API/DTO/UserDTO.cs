namespace Project_API.DTO
{
    public class UserDTO
    {
        public int IdUser { get; set; }
        public string FullName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string UserAddress { get; set; } = null!;
        public bool? IsOver18 { get; set; }

      //  public virtual ICollection<OrderDTO> Orders { get; set; }
    }
}
