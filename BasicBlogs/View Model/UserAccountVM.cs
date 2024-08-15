using System.ComponentModel.DataAnnotations;

namespace BasicBlogs.ViewModel
{
    public class UserAccountVM
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "User Name is Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter strong password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        

        [Required(ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage ="Please Enter a Valid Email")]
        public string Email { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }
    }
}
