using BasicBlogs.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Metadata;
using BasicBlogs.ViewModel;

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

        public IActionResult ReadBlogs()
        {
            List<MyBlogs> MyBlogs = _context.Blogs.ToList();
            return View(MyBlogs);
        }

        // Create data
        public IActionResult CreateAddBlogs()
        {
            return View(new Blogs());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAddBlogs(MyBlogs obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {

                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string ImagePath = Path.Combine(wwwRootPath, @"images\BlogImages");

                    using (var fileStream = new FileStream(Path.Combine(ImagePath, filename), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                     obj.Image = @"images\BlogImages" + filename; 
                }
                _context.Blogs.Add(obj);
                await _context.SaveChangesAsync();
                return RedirectToAction("ReadBlogs");


            }
            return View(obj);
        }


        // Edit data
        public IActionResult EditAddBlogs(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            MyBlogs MyBlogFromDb = _context.Blogs.Find(id);
            if (MyBlogFromDb == null)
            {
                return NotFound();
            }
            return View(MyBlogFromDb);
        }

        [HttpPost]
        public IActionResult EditMyBlogs(MyBlogs obj)
        {
            if (ModelState.IsValid)
            {
                _context.Blogs.Update(obj);
                _context.SaveChanges();
                return RedirectToAction("ReadBlogs");
            }
            return View(obj);
        }

        // Delete data
        public IActionResult Delete(int id)
        {
            var blog = _context.Blogs.Find(id);
            if (blog == null)
            {
                return NotFound();
            }
            _context.Blogs.Remove(blog);
            _context.SaveChanges();
            return RedirectToAction("ReadBlogs");
        }
    }
}