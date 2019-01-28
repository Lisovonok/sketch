using System.ComponentModel.DataAnnotations;

namespace MVCBlog.Website.Models.InputModels.Login
{
    public class LoginFormInput
    {
        [Required(ErrorMessage = "*")]
        [Display(Name = "Username", ResourceType = typeof(Properties.Common))]
        public string Username { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Properties.Common))]
        public string Password { get; set; }

        [Display(Name = "RememberMe", ResourceType = typeof(Properties.Common))]
        public bool RememberMe { get; set; }
    }
}