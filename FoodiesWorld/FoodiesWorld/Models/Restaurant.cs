namespace FoodiesWorld.Models
{
    public class Restaurant
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Address { get; set; }

        public ICollection<Opinion>? opinions { get; set; } = new List<Opinion>();

        public Restaurant() { }
    }
}
