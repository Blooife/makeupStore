
using makeupStore.Services.AuthAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace makeupStore.Services.AuthAPI.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    
    public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    public AppDbContext()
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
    }
}