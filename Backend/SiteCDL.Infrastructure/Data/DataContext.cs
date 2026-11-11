using Microsoft.EntityFrameworkCore;
using SiteCDL.Domain.Entities;

namespace SiteCDL.Infrastructure.Data
{
    public class DataContext :DbContext
    {


        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
            
        }

        public DbSet<Post> Post { get; set; }
        public DbSet<PostCategoria> PostCategoria { get; set; } 
        public DbSet<Paginas> Paginas { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
 



            modelBuilder
                .Entity<Paginas>()
                .Property(a => a.Titulo)
                .HasColumnType("varchar(255)")
                .IsRequired();


            modelBuilder
                .Entity<Paginas>()
                .Property(a => a.Controller)
                .HasColumnType("varchar(255)")
                .IsRequired();

        }
 

    }
}
