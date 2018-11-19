using AdventureGuide.Models.Destinations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdventureGuide.Models
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Destination> Destination { get; set; }

        public virtual DbSet<Review> Review { get; set; }

        public virtual DbSet<ImagePath> ImagePath { get; set; }

        public virtual DbSet<Keyword> Keyword { get; set; }

    }
}
