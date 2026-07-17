using EsignPlatform.BLL.DTOs.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.BLL.Services.Interfaces
{
    public interface ITemplateService
    {
        Task<List<TemplateListDto>> GetAllAsync();
        Task<List<TemplateListDto>> GetActiveAsync();
        Task<TemplateDetailDto?> GetDetailAsync(int id);
        Task ToggleActiveAsync(int id);
    }

}
