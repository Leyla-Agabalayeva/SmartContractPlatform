using AutoMapper;
using EsignPlatform.BLL.DTOs.Template;
using EsignPlatform.BLL.Services.Interfaces;
using EsignPlatform.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EsignPlatform.BLL.Services.Implementations
{
    public class TemplateService : ITemplateService
    {
        private readonly ITemplateRepository _repo;
        private readonly IMapper _mapper;

        public TemplateService(ITemplateRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<TemplateListDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return _mapper.Map<List<TemplateListDto>>(list);
        }

        public async Task<List<TemplateListDto>> GetActiveAsync()
        {
            var list = await _repo.GetActiveAsync();
            return _mapper.Map<List<TemplateListDto>>(list);
        }

        public async Task<TemplateDetailDto?> GetDetailAsync(int id)
        {
            var t = await _repo.GetByIdAsync(id);
            if (t is null) return null;

            return new TemplateDetailDto
            {
                Id = t.Id,
                Name = t.Name,
                Category = t.Category,
                Description = t.Description,
                Fields = ParseFields(t.SchemaJson)
            };
        }

        public async Task ToggleActiveAsync(int id)
        {
            var t = await _repo.GetByIdAsync(id);
            if (t is null) return;
            t.IsActive = !t.IsActive;
            _repo.Update(t);
            await _repo.SaveChangesAsync();
        }

        public static List<TemplateFieldDto> ParseFields(string schemaJson)
        {
            if (string.IsNullOrWhiteSpace(schemaJson)) return new();
            try
            {
                var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<List<TemplateFieldDto>>(schemaJson, opts) ?? new();
            }
            catch
            {
                return new();
            }
        }
    }

}
