using BasicBlogs.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using BasicBlogs.Entities;

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
            var blogs = _context.MyBlogs.ToList();
            var blogVMs = blogs.Select(blog => new BlogVM
            {
                Id = blog.Id,
                Title = blog.Title,
                AuthorName = blog.AuthorName,
                Description = blog.Description,
                PublishDate = DateOnly.FromDateTime(blog.PublishTime), // Adjust if necessary
                ImagePath = blog.ImagePath // Correct property name for image path
            }).ToList();
            return View(blogVMs);
        }

        //public IActionResult ReadBlogs()
        //{
        //    var myBlogs = _context.MyBlogs.ToList();
        //    var blogVMs = myBlogs.Select(blog => new BlogVM
        //    {
        //        Id = blog.Id,
        //        Title = blog.Title,
        //        AuthorName = blog.AuthorName,
        //        Description = blog.Description,
        //        PublishDate = blog.PublishDate, // Ensure you include this property if it exists
        //        Image = null // Since you're handling images as paths, not `IFormFile`, leave this null
        //    }).ToList();

        //    return View(blogVMs);
        //}


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
           

            MyBlog myBlogFromDb = _context.MyBlogs.Find(id);
            if (myBlogFromDb == null)
            {
                return NotFound();
            }
            var blogvm = new BlogVM()
            {
                Id = myBlogFromDb.Id,
                Title = myBlogFromDb.Title,
                AuthorName = myBlogFromDb.AuthorName,
                Description = myBlogFromDb.Description,
                PublishDate = myBlogFromDb.PublishDate,
                ImagePath= myBlogFromDb.ImagePath,
            };
            return View(blogvm);
        }

        [HttpPost]
        public async Task<IActionResult> EditMyBlogs(BlogVM vm)
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
            blogFromDb.PublishDate = vm.PublishDate;

            // Handle the image update
            if (vm.Image != null)
            {
                // Optionally delete the old image
                if (!string.IsNullOrEmpty(blogFromDb.ImagePath))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "image", blogFromDb.ImagePath);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Upload the new image
                blogFromDb.ImagePath = UploadImage(vm.Image);
            }

            _context.MyBlogs.Update(blogFromDb);
            await _context.SaveChangesAsync();

            return RedirectToAction("ReadBlogs");
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


        //view Blogs
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
                PublishDate = DateOnly.FromDateTime(blog.PublishTime), // Adjust if necessary
                ImagePath = blog.ImagePath // Correct property name for image path
            };

            return View(blogVM);
        }
    }
    }

