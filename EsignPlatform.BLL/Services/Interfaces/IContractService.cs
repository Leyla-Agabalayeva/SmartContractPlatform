using EsignPlatform.BLL.DTOs.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.BLL.Services.Interfaces
{
    public interface IContractService
    {
        Task<List<ContractListDto>> GetMyContractsAsync(string userId);
        Task<ContractDetailDto?> GetDetailAsync(int id);
        Task<CreateContractDto?> BuildCreateModelAsync(int templateId);
        Task<int> CreateAsync(CreateContractDto dto, string creatorUserId);
        Task RejectAsync(int contractId, string userId);
    }
}
