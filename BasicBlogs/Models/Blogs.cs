using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace BasicBlogs.Models
{
    public class MyBlogs
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author name is required")]
        public string AuthorName { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

<<<<<<< HEAD
        [Required(ErrorMessage = "Image is required")]
        public string? Image { get; set; }


=======

        [Required(ErrorMessage = "Description is required")]
        public string? ImagePath { get; set; }
>>>>>>> 4ab04330cfaf5a17e2b9166ba514e0dda1d42173

        public DateOnly PublishDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public DateTime PublishTime { get; set; } = DateTime.Now;
    }
}
