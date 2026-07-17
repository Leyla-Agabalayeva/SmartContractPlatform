using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.DAL.Enums
{
    public enum SignatureType
    {
        Otp = 1,     // OTP (SMS verification)
        Digital = 2  // Digital signature (ASAN İmza inteqrasiya planı)
    }

}
