using Microsoft.EntityFrameworkCore;
using SiteCDL.Domain.Entities;

namespace SiteCDL.Infrastructure.Data
{
    public class BannerDbContext : DbContext
    {


        public BannerDbContext(DbContextOptions<BannerDbContext> options):base(options)
        {
            
        }

        public DbSet<Banner> Banners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
 
            modelBuilder
                .Entity<Banner>()
                .Property(a => a.Nome)
                .HasColumnType("varchar(255)")
                .IsRequired();
        }
      
    }
}
