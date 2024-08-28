using BasicBlogs.Entities;
using BasicBlogs.Models;
using BasicBlogs.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasicBlogs.Controllers
{
      
    public class MyBlogsUserPannelController : Controller
    {

        private readonly AppDbContext appDbContext;
        public MyBlogsUserPannelController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
       
        public IActionResult Index(int id)
        {

            if(ModelState.IsValid)
            {

            var blogs = appDbContext.MyBlogs.ToList();
            var blogVMs = blogs.Select(blog => new MyBlogs

            {
                Id = blog.Id,
                Title = blog.Title,
                AuthorName = blog.AuthorName,
                Description = blog.Description,
                PublishDate = blog.PublishDate, // Adjust if necessary
                ImagePath = blog.ImagePath // Correct property name for image path
            }).ToList();
            return View(blogVMs);
            }
            return View();

        }

        public IActionResult ReadBlogsUserPannel(int id)
        {
            var blog = appDbContext.MyBlogs.FirstOrDefault(b => b.Id == id);

            if (blog == null)
            {
                return NotFound(); // Return a 404 page if the blog is not found
            }

            return View(blog); // Pass the blog object to the view
        }

    }
}
