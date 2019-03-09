using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MovieTime.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required]
        [MaxLength(20, ErrorMessage = "Username cannot be longer than 20 characters")]
        [MinLength(5, ErrorMessage = "Username must contain at least 5 characters")]
        public string Username { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Password must contain at least 5 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
