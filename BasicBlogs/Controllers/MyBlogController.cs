using BasicBlogs.Models;
using Microsoft.AspNetCore.Mvc;

namespace BasicBlogs.Controllers
{
    public class MyBlogController : Controller
    {
        private readonly AppDbContext _context;

        public MyBlogController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult ReadBlogs()
        {
            List<MyBlog> MyBlogs = _context.MyBlogs.ToList();
            return View(MyBlogs);
        }

        //create data
        public IActionResult CreateAddBlogs()
        {
            return View(new MyBlog());
        }

        [HttpPost]
        public IActionResult CreateAddBlogs(MyBlog obj)
        {
            if (ModelState.IsValid)
            {
                _context.MyBlogs.Add(obj);
                _context.SaveChanges();
                return RedirectToAction("ReadBlogs");
            }
            return View(obj);
        }

        //Edit data
        public IActionResult EditAddBlogs(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            MyBlog MyBlogFromDb = _context.MyBlogs.Find(id);
            if (MyBlogFromDb == null)
            {
                return NotFound();
            }
            return View(MyBlogFromDb);
        }

        [HttpPost]
        public IActionResult EditMyBlogs(MyBlog obj)
        {
            if (ModelState.IsValid)
            {
                _context.MyBlogs.Update(obj);
                _context.SaveChanges();
                return RedirectToAction("ReadBlogs");
            }
            return View(obj);
        }

        //Delete data

        public IActionResult Delete(int id)
        {
            var blogs = _context.MyBlogs.Find(id);
            if (id == 0)
            {
                return NotFound();
            }
            _context.MyBlogs.Remove(blogs);
            _context.SaveChanges();
            return RedirectToAction("ReadBlogs");

        }
    }
}
