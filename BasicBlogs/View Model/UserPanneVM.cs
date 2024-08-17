using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasicBlogs.ViewModel
{
    public class UserPanneVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author Name is required")]
        public string AuthorName { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        public DateOnly PublishDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [NotMapped]
        public IFormFile? Image { get; set; }
        public string? ImagePath { get; set; }




    }
}
