using EsignPlatform.BLL.DTOs.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.BLL.Services.Interfaces
{

    public interface IPdfService
    {
        // Müqavilənin PDF-ini generasiya edir, byte[] qaytarır
        byte[] GenerateContractPdf(ContractDetailDto contract);
    }
}
