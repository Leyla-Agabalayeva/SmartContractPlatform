using EsignPlatform.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.DAL.Repositories.Interfaces
{

    public interface ITemplateRepository : IRepository<Template>
    {
        Task<List<Template>> GetActiveAsync();
    }
}
