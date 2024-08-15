using BasicBlogs.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace BasicBlogs.Models
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<MyBlogs>Blogs { get; set; }
        public DbSet<LoginSignUp>Registrations { get; set; }
    }
}


