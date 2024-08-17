using BasicBlogs.Entities;
using BasicBlogs.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasicBlogs.Controllers
{
      
    public class MyBlogsUserPannelController : Controller
    {

        private readonly AppDbContext appDbContext;
        public MyBlogsUserPannelController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        [Authorize]
        public IActionResult UserPannel(int id)
        {
            var blogs = appDbContext.MyBlogs.Find(id);
            if(blogs == null)
            {
                return NotFound();
            }
            var blogsVM= new UserPanneVM();
            blogsVM.Id = id;
            blogsVM.Title =blogs.Title;
            blogsVM.Description= blogs.Description;
            blogsVM.AuthorName = blogs.AuthorName;
            blogsVM.ImagePath = blogs.ImagePath;
            blogsVM.PublishDate = blogs.PublishDate;
            return View(blogsVM);
        }
    }
}
