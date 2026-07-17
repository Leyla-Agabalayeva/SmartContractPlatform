using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.BLL.Services.Interfaces
{
    public interface IOtpService
    {
        // Kod yaradır, saxlayır (MVP: memory cache) və qaytarır
        string Generate(string key);
        bool Validate(string key, string code);
        void Remove(string key);
    }
   
}
