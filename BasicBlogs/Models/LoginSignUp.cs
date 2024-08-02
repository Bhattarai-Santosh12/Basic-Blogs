using System.ComponentModel.DataAnnotations;

namespace BasicBlogs.Models
{
    public class LoginSignUp
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="User Name is Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage ="Please enter strong password")]
        public string Password { get; set; }

        [Required(ErrorMessage ="Email is Required")]
        public string Email { get; set; }

        public string? Phone { get; set; }

        public string? Address {  get; set; }

        
    }
}
