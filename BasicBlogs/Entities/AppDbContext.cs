using BasicBlogs.Data.Configuration;
using BasicBlogs.Models;
using BasicBlogs.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace BasicBlogs.Entities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<MyBlogs> MyBlogs { get; set; }
        public DbSet<UserAccountVM> UserAccounts { get; set; }

        public DbSet<UserLogin>UserLogins { get; set; }
        public DbSet<UserPanneVM>UserPanneVMs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserLoginConfiguration());

            
            base.OnModelCreating(modelBuilder);
        }
    }
}
