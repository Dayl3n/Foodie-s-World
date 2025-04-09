using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FoodiesWorld.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FoodiesWorld.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<FoodiesWorld.Models.Opinion> Opinion { get; set; } = default!;
        public DbSet<FoodiesWorld.Models.Restaurant> Restaurant { get; set; } = default!;
        public DbSet<FoodiesWorld.Models.Visit> Visit { get; set; } = default!;
        
    }
}
