using BasicBlogs.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace BasicBlogs.Controllers
{
    public class MyBlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MyBlogController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // Read Blogs
        public IActionResult ReadBlogs()
        {
            List<MyBlog> myBlogs = _context.MyBlogs.ToList();
            return View(myBlogs);
        }

        // Create data
        public IActionResult CreateAddBlogs()
        {
            return View(new BlogVM());
        }


        [HttpPost]
        public async Task<IActionResult> CreateAddBlogs(BlogVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }

            var post = new MyBlog();

             post.Title = vm.Title;
            post.Description = vm.Description;
            post.AuthorName = vm.AuthorName;
  

            if (vm.Image != null)
            {
                post.ImagePath= UploadImage(vm.Image);
            }


            await _context.MyBlogs.AddAsync(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("ReadBlogs");
        }
        private string UploadImage(IFormFile file)
        {
            string uniqueFileName = "";
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "image");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(folderPath, uniqueFileName);
            using (FileStream fileStream = System.IO.File.Create(filePath))
            {
                file.CopyTo(fileStream);
            }
            return uniqueFileName;
        }

        // Edit data
        public IActionResult EditAddBlogs(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            MyBlog myBlogFromDb = _context.MyBlogs.Find(id);
            if (myBlogFromDb == null)
            {
                return NotFound();
            }
            return View(myBlogFromDb);
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

       


        // Delete data
        public IActionResult Delete(int id)
        {
            var blog = _context.MyBlogs.Find(id);
            if (blog == null)
            {
                return NotFound();
            }
            _context.MyBlogs.Remove(blog);
            _context.SaveChanges();
            return RedirectToAction("ReadBlogs");
        }
    }
}
