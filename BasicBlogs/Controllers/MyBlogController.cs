using BasicBlogs.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

using BasicBlogs.Entities;
using BasicBlogs.ViewModel;
using Microsoft.AspNetCore.Authorization;


namespace BasicBlogs.Controllers
{
    [Authorize]
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
            var blogs = _context.MyBlogs.ToList();
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

        // Create Blog
      
        public IActionResult CreateAddBlogs()
        {
            return View(new BlogVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAddBlogs(MyBlogs obj, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            if (file != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string imagePath = Path.Combine(wwwRootPath, "images", filename);

                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                //obj.ImagePath = Path.Combine("images", filename);
                obj.ImagePath = filename;
            }

            _context.MyBlogs.Add(obj);
            await _context.SaveChangesAsync();
            return RedirectToAction("ReadBlogs");
        }

        // Edit Blog
        
        public IActionResult EditAddBlogs(int id)
        {
            var myBlogFromDb = _context.MyBlogs.Find(id);
            if (myBlogFromDb == null)
            {
                return NotFound();
            }

            var blogVM = new BlogVM
            {
                Id = myBlogFromDb.Id,
                Title = myBlogFromDb.Title,
                AuthorName = myBlogFromDb.AuthorName,
                Description = myBlogFromDb.Description,
                PublishDate = myBlogFromDb.PublishDate,
                ImagePath = myBlogFromDb.ImagePath,
            };

            return View(blogVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditMyBlogs(BlogVM vm, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var blogFromDb = _context.MyBlogs.Find(vm.Id);
            if (blogFromDb == null)
            {
                return NotFound();
            }

            // Update the blog properties
            blogFromDb.Title = vm.Title;
            blogFromDb.AuthorName = vm.AuthorName;
            blogFromDb.Description = vm.Description;
            blogFromDb.PublishDate = vm.PublishDate; // Ensure consistency with property names

            // Handle the image update
            if (file != null)
            {
                // Optionally delete the old image
                if (!string.IsNullOrEmpty(blogFromDb.ImagePath))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", blogFromDb.ImagePath);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Upload the new image
                blogFromDb.ImagePath = UploadImage(file);
            }

            _context.MyBlogs.Update(blogFromDb);
            await _context.SaveChangesAsync();

            return RedirectToAction("ReadBlogs");
        }

        // Delete Blog
        public IActionResult Delete(int id)
        {
            var blog = _context.MyBlogs.Find(id); // Use _context.MyBlogs
            if (blog == null)
            {
                return NotFound();
            }

            _context.MyBlogs.Remove(blog);
            _context.SaveChanges();
            return RedirectToAction("ReadBlogs");
        }

        // View Blog
      
        public IActionResult ViewBlogs(int id)
        {
            var blog = _context.MyBlogs.Find(id);
            if (blog == null)
            {
                return NotFound();
            }

            var blogVM = new BlogVM
            {
                Id = blog.Id,
                Title = blog.Title,
                AuthorName = blog.AuthorName,
                Description = blog.Description,
                PublishDate = blog.PublishDate, // Adjust if necessary
                ImagePath = blog.ImagePath // Correct property name for image path
            };

            return View(blogVM);
        }

        private string UploadImage(IFormFile file)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            var filePath = Path.Combine(folderPath, uniqueFileName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return uniqueFileName; // Return only the filename
        }

    }
}
