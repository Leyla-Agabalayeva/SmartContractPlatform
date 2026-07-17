using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.BLL.DTOs.Account
{

    public class LoginDto
    {
        [Required, EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Required, DataType(DataType.Password)]
        [Display(Name = "Şifrə")]
        public string Password { get; set; } = null!;

        [Display(Name = "Məni xatırla")]
        public bool RememberMe { get; set; }
    }

}
