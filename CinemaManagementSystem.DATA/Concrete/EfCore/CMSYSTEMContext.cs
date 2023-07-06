using Microsoft.EntityFrameworkCore;
using CinemaManagementSystem.ENTITY;

namespace CinemaManagementSystem.DATA.Concrete.EfCore
{
    public class CMSYSTEMContext: DbContext
    {

        
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-2PUAJM9;Database=CMS;User Id=selin;Password=selin12345;Encrypt=False");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieCategory>()
                .HasKey(c => new { c.CategoryId, c.MovieId });
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BookingCart> BookingCarts { get; set; }
        public DbSet<BookingCartItem> BookingCartItems { get; set; }
        public DbSet<AspNetUsers> users { get; set; }
        
    }
}
