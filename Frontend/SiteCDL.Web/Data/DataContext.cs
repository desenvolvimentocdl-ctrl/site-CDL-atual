using Microsoft.EntityFrameworkCore;
using SiteCDL.Models;

namespace SiteCDL.Data
{
    public class DataContext :DbContext
    {

        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
            
        }

        public DbSet<Post> Post { get; set; }
        public DbSet<PostCategoria> PostCategoria { get; set; } 
        public DbSet<Paginas> Paginas { get; set; }
        public DbSet<Lead> Lead { get; set; }

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

            modelBuilder.Entity<Lead>(entity =>
            {
                entity.ToTable("LeadsWebhook");

                entity.HasKey(e => e.Codigo);

                entity.Property(e => e.Codigo)
                      .HasColumnName("Codigo")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.Json)
                      .HasColumnType("longtext");

                entity.Property(e => e.DataCadastro)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Integrado)
                      .HasColumnName("Integrado")
                      .HasColumnType("TINYINT")
                      .HasDefaultValue(0);
            });
        }
    }
}