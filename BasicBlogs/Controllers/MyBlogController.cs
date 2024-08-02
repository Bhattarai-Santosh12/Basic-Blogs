using BasicBlogs.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

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
            List<MyBlog> MyBlogs = _context.MyBlogs.ToList();
            return View(MyBlogs);
        }

        // Create data
        public IActionResult CreateAddBlogs()
        {
            return View(new MyBlog());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddBlogs(MyBlog obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Image != null)
                {
                    string folder = "images/CoverImages/";
                    folder += Guid.NewGuid().ToString() + "_" + obj.Image.FileName;
                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

                    using (var fileStream = new FileStream(serverFolder, FileMode.Create))
                    {
                        await obj.Image.CopyToAsync(fileStream);
                    }
                    Console.WriteLine($"Image Path: {folder}");
                    obj.ImagePath = folder; // Save the path to the database

                    // Log to verify the ImagePath assignment
                    Console.WriteLine($"Assigned ImagePath: {obj.ImagePath}");
                }

                _context.MyBlogs.Add(obj);
                _context.SaveChanges();
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
