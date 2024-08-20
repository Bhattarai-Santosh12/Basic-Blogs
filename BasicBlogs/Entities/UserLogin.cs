using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BasicBlogs.Entities
{
    public class UserLogin
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "User Name or Email is Required")]
        [DisplayName("UserName or Email")]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "Please enter strong password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Role { get; set; }
    }
}
