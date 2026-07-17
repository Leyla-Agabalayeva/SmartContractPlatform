using EsignPlatform.BLL.DTOs.Signature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.BLL.Services.Interfaces
{
    public interface ISignatureService
    {
        // İstifadəçinin bu müqavilədə imzalaya biləcəyi tərəf varmı?
        Task<bool> CanUserSignAsync(int contractId, string userId);
        Task<StartSignResult> StartSignAsync(int contractId, string userId);
        Task<StartSignResult> VerifyAndSignAsync(VerifyOtpDto dto, string userId, string? ipAddress);
    }
}
