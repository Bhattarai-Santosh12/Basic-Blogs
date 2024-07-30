using System.ComponentModel.DataAnnotations;

namespace BasicBlogs.Models
{
    public class MyBlog
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage ="AuthorName name is required")]
        public string AuthorName { get; set; }

        [Required(ErrorMessage ="Description is required")]
        public string Description { get; set; }

      
        public string? Image {  get; set; }

        public DateOnly PublishDate {  get; set; }=new DateOnly();

        public DateTime PublishTime { get; set; } = DateTime.Now;
    }
}
