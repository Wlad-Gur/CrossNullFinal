using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CrossNull.Web.Model
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Field must be filled")]
        [StringLength(16, MinimumLength = 3, ErrorMessage = "Invalid name length")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Field must be filled")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field must be filled")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.{8,16}$)(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9]).*$",
            ErrorMessage = "Password must contain numbers, uppercase and lowercase letters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Field must be filled")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Password and ConfirmPassword don't match")]
        [Display(Name = "Repeat password")]
        public string ConfirmPassword { get; set; }

    }
}