using Microsoft.EntityFrameworkCore;
using BoostOrderAssessment.Data.Entities;

namespace BoostOrderAssessment.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<VariationEntity> Variations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=bomart.db");
        }
    }
}

