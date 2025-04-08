using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodiesWorld.Models
{
    public class Opinion
    {
        public string Id { get; set; }

        public string RestaurantId { get; set; }

        public Restaurant? Restaurant { get; set; }

        public string UserId { get; set; }

        public User? User { get; set; }


        [Range(0,5)]
        public int Rating { get; set; }

        public string Description {  get; set; }



        public Opinion()
        {
            
        }

    }
}
