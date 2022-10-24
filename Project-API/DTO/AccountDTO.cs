using Project_API.Models;

namespace Project_API.DTO
{
    public class AccountDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string UserPassword { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string? TelNumber { get; set; }
        public int? IdUsers { get; set; }
        public int? RestaurantId { get; set; }
        public int Role { get; set; }

        public virtual UserDTO? IdUsersNavigation { get; set; }
        public virtual RestaurantDTO? Restaurant { get; set; }
        public virtual RoleDTO RoleNavigation { get; set; } = null!;
    }
}
