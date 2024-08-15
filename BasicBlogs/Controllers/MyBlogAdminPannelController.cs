using BasicBlogs.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Reflection.Metadata;
using BasicBlogs.ViewModel;
=======
using Microsoft.AspNetCore.Identity;
<<<<<<< HEAD:BasicBlogs/Controllers/MyBlogAdminPannelController.cs
using BasicBlogs.Entities;
=======
>>>>>>> 4ab04330cfaf5a17e2b9166ba514e0dda1d42173
>>>>>>> 7468e1516a3dbfcd4f35293b07ccaeb632053972:BasicBlogs/Controllers/MyBlogController.cs

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
<<<<<<< HEAD
            List<MyBlogs> MyBlogs = _context.Blogs.ToList();
            return View(MyBlogs);
=======
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
>>>>>>> 4ab04330cfaf5a17e2b9166ba514e0dda1d42173
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
<<<<<<< HEAD
            return View(new Blogs());
=======
            return View(new BlogVM());
>>>>>>> 4ab04330cfaf5a17e2b9166ba514e0dda1d42173
        }


        [HttpPost]
<<<<<<< HEAD
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAddBlogs(MyBlogs obj, IFormFile? file)
=======
        public async Task<IActionResult> CreateAddBlogs(BlogVM vm)
>>>>>>> 4ab04330cfaf5a17e2b9166ba514e0dda1d42173
        {
            if (!ModelState.IsValid) { return View(vm); }

            var post = new MyBlog();

             post.Title = vm.Title;
            post.Description = vm.Description;
            post.AuthorName = vm.AuthorName;
  

            if (vm.Image != null)
            {
<<<<<<< HEAD

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


=======
                post.ImagePath= UploadImage(vm.Image);
>>>>>>> 4ab04330cfaf5a17e2b9166ba514e0dda1d42173
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
<<<<<<< HEAD
            MyBlogs MyBlogFromDb = _context.Blogs.Find(id);
            if (MyBlogFromDb == null)
=======
            var blogvm = new BlogVM()
>>>>>>> 4ab04330cfaf5a17e2b9166ba514e0dda1d42173
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
<<<<<<< HEAD
        public IActionResult EditMyBlogs(MyBlogs obj)
=======
        public async Task<IActionResult> EditMyBlogs(BlogVM vm)
>>>>>>> 4ab04330cfaf5a17e2b9166ba514e0dda1d42173
        {
            if (!ModelState.IsValid)
            {
<<<<<<< HEAD
                _context.Blogs.Update(obj);
                _context.SaveChanges();
                return RedirectToAction("ReadBlogs");
=======
                return View(vm);
>>>>>>> 4ab04330cfaf5a17e2b9166ba514e0dda1d42173
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
            var blog = _context.Blogs.Find(id);
            if (blog == null)
            {
                return NotFound();
            }
            _context.Blogs.Remove(blog);
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
<<<<<<< HEAD
}
=======
    }

>>>>>>> 4ab04330cfaf5a17e2b9166ba514e0dda1d42173
