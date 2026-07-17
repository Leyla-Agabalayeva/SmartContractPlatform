using EsignPlatform.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.DAL.Repositories.Interfaces
{

    public interface IContractRepository : IRepository<Contract>
    {
        Task<List<Contract>> GetByUserAsync(string userId);
        Task<Contract?> GetFullAsync(int id);
    }

}
