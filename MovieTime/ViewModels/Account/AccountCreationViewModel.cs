using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace MovieTime.ViewModels.Account
{
    public class AccountCreationViewModel
    {
        [Required]
        [MaxLength(20, ErrorMessage = "Username cannot be longer than 20 characters")]
        [MinLength(5, ErrorMessage = "Username must contain at least 5 characters")]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Password must contain at least 5 characters")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Re-type Password")]
        public string PasswordValidation { get; set; }
    }
}
