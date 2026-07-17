using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.BLL.DTOs.Signature
{

    public class VerifyOtpDto
    {
        [Required]
        public int ContractId { get; set; }

        [Required(ErrorMessage = "OTP kodu daxil edin")]
        [StringLength(6, MinimumLength = 4)]
        [Display(Name = "OTP kodu")]
        public string Code { get; set; } = null!;
    }
}
