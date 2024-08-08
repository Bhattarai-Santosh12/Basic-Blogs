using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BasicBlogs.Models
{
    public class BlogVM
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author Name is required")]
        public string AuthorName { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

    
        public IFormFile? Image { get; set; }
    }
}
