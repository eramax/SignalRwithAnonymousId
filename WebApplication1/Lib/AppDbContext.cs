using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Lib
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<Product> Products { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> opt):base(opt)
        {
        }
    }
}