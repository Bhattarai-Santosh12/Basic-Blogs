using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BasicBlogs.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(UserName), IsUnique = true)]
    public class UserAccount
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "User Name is Required")]
        [MaxLength(50, ErrorMessage =("User Name shouldn't be more then 50 characters"))]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter strong password")]
        [MaxLength(50, ErrorMessage = ("Password shouldn't be more then 50 characters"))]
        public string Password { get; set; }

        


        [Required(ErrorMessage = "Email is Required")]
        [MaxLength(100, ErrorMessage = ("Email shouldn't be more then 100 characters"))]
        public string Email { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

    }
}
