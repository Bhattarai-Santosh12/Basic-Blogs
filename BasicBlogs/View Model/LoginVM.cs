using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BasicBlogs.ViewModel
{
    public class LoginVM
    {
        [Required(ErrorMessage = "User Name or Email is Required")]
        [DisplayName("UserName or Email")]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "Please enter strong password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
