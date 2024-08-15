using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BasicBlogs.Models;

namespace BasicBlogs.ViewModel
{
    public class Blogs
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author name is required")]
        public string AuthorName { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Image is required")]
       
        public IFormFile? Photo { get; set; }

        public DateOnly PublishDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    }
}
