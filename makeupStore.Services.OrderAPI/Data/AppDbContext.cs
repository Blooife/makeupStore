
using makeupStore.Services.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace makeupStore.Services.OrderAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public AppDbContext()
        {
            
        }
        
        public virtual DbSet<OrderHeader> OrderHeaders { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
     
    }
}
