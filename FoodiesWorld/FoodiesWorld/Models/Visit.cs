namespace FoodiesWorld.Models
{
    public class Visit
    {
        public string Id { get; set; }

        public DateTime  Date { get; set; }

        public bool IsBooked { get; set; }

        public string RestaurantId { get; set; }

        public Restaurant? Restaurant { get; set; }

        public string UserId { get; set; }

        public User? User { get; set; }

        public Visit()
        {
            
        }
    }
}
