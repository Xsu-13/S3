using EFTraining.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFTraining.Data
{
    public class DBContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Test> Tests { get; set; }

        public DBContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("EFDemo"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasData(
                    new Book { Id = 1, Description = "FUF", Title = "ghghg"},
                    new Book { Id = 2, Description = "Небо", Title = "тайтл" },
                    new Book { Id = 3, Description = "как быть", Title = "или не быть" }
            );
        }
    }
}
