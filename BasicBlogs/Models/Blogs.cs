using System;
using System.ComponentModel.DataAnnotations;

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

        public string? ImagePath { get; set; }

        public DateOnly PublishDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public DateTime PublishTime { get; set; } = DateTime.Now;
    }
}
