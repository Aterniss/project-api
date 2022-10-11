namespace Project_API.DTO
{
    public class RiderDTO
    {
        public int RiderId { get; set; }
        public string RiderName { get; set; } = null!;
        public int ZoneId { get; set; }

        public virtual ZoneDTO Zone { get; set; } = null!;

    }
}
