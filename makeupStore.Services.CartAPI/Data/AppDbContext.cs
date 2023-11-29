
using makeupStore.Services.CartAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace makeupStore.Services.CartAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public AppDbContext()
        {
            
        }
        
        public virtual DbSet<CartHeader> CartHeaders { get; set; }
        public virtual DbSet<CartDetails> CartDetails { get; set; }
     
    }
}
