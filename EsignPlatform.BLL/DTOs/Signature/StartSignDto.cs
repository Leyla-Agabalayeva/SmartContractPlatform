using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.BLL.DTOs.Signature
{
    // OTP göndərmə nəticəsi (MVP-də real SMS yoxdur, kod qaytarılır)
    public class StartSignResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        // Yalnız MVP/demo üçün — real sistemdə SMS ilə gedər, UI-a qaytarılmaz
        public string? DemoOtpCode { get; set; }
    }

}
