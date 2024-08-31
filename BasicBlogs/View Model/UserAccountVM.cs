using System.ComponentModel.DataAnnotations;

namespace BasicBlogs.ViewModel
{
    public class UserAccountVM
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "User Name is Required")]
        public string UserName { get; set; }

        // Password should not be required for Google login
        [Required(ErrorMessage = "Please enter a strong password", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Please Enter a Valid Email")]
        public string Email { get; set; }

        // Phone and Address can be null (optional for Google login)
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
