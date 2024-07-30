using Microsoft.EntityFrameworkCore;

namespace BasicBlogs.Models
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<MyBlog>MyBlogs { get; set; }
    }
}
