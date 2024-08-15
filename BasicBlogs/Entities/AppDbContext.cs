using BasicBlogs.Models;
using BasicBlogs.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace BasicBlogs.Entities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<MyBlog> MyBlogs { get; set; }
        public DbSet<UserAccountVM> UserAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
