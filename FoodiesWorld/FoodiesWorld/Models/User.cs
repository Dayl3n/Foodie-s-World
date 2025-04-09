using Microsoft.AspNetCore.Identity;

namespace FoodiesWorld.Models
{
    public class User : IdentityUser
    {
        public ICollection<Opinion>? Opinions { get; set; }
        public ICollection<Visit>? visits { get; set; }
    }
}
