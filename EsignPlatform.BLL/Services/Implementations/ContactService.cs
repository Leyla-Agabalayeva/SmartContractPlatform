using AutoMapper;
using EsignPlatform.BLL.DTOs.Contract;
using EsignPlatform.BLL.Services.Interfaces;
using EsignPlatform.DAL.Entities;
using EsignPlatform.DAL.Enums;
using EsignPlatform.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EsignPlatform.BLL.Services.Implementations
{
    public class ContractService : IContractService
    {
        private readonly IContractRepository _contractRepo;
        private readonly ITemplateRepository _templateRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public ContractService(
            IContractRepository contractRepo,
            ITemplateRepository templateRepo,
            UserManager<AppUser> userManager,
            IMapper mapper)
        {
            _contractRepo = contractRepo;
            _templateRepo = templateRepo;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<List<ContractListDto>> GetMyContractsAsync(string userId)
        {
            var contracts = await _contractRepo.GetByUserAsync(userId);
            return _mapper.Map<List<ContractListDto>>(contracts);
        }

        public async Task<CreateContractDto?> BuildCreateModelAsync(int templateId)
        {
            var template = await _templateRepo.GetByIdAsync(templateId);
            if (template is null || !template.IsActive) return null;

            return new CreateContractDto
            {
                TemplateId = template.Id,
                TemplateName = template.Name,
                Fields = TemplateService.ParseFields(template.SchemaJson)
            };
        }

        public async Task<int> CreateAsync(CreateContractDto dto, string creatorUserId)
        {
            var template = await _templateRepo.GetByIdAsync(dto.TemplateId)
                ?? throw new InvalidOperationException("Template tapılmadı.");

            var creator = await _userManager.FindByIdAsync(creatorUserId)
                ?? throw new InvalidOperationException("İstifadəçi tapılmadı.");

            // Yalnız schema-da olan field-ləri saxla
            var schemaKeys = TemplateService.ParseFields(template.SchemaJson)
                .Select(f => f.Key).ToHashSet();
            var cleanValues = dto.FieldValues
                .Where(kv => schemaKeys.Contains(kv.Key))
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            // Qarşı tərəf sistemdə varsa - link et
            var counterparty = (await _userManager.Users
                .FirstOrDefaultAsync(u => u.FinOrVoen == dto.CounterpartyFinOrVoen));

            var contract = new Contract
            {
                Title = dto.Title,
                TemplateId = template.Id,
                CreatedByUserId = creatorUserId,
                Status = ContractStatus.Pending,
                DataJson = JsonSerializer.Serialize(cleanValues),
                Parties = new List<ContractParty>
            {
                new()
                {
                    UserId = creatorUserId,
                    FinOrVoen = creator.FinOrVoen,
                    DisplayName = creator.DisplayName,
                    Role = dto.CreatorRole
                },
                new()
                {
                    UserId = counterparty?.Id,
                    FinOrVoen = dto.CounterpartyFinOrVoen,
                    DisplayName = dto.CounterpartyName,
                    Role = dto.CounterpartyRole
                }
            }
            };

            await _contractRepo.AddAsync(contract);
            await _contractRepo.SaveChangesAsync();
            return contract.Id;
        }

        public async Task<ContractDetailDto?> GetDetailAsync(int id)
        {
            var c = await _contractRepo.GetFullAsync(id);
            if (c is null) return null;

            var fields = TemplateService.ParseFields(c.Template.SchemaJson);
            var values = ParseValues(c.DataJson);

            return new ContractDetailDto
            {
                Id = c.Id,
                Title = c.Title,
                TemplateName = c.Template.Name,
                Status = c.Status,
                CreatedAt = c.CreatedAt,
                CreatedByName = c.CreatedByUser.DisplayName,
                CreatedByUserId = c.CreatedByUserId,
                Fields = fields,
                FieldValues = values,
                Parties = _mapper.Map<List<ContractPartyDto>>(c.Parties),
                HasDocument = c.Document is not null
            };
        }

        public async Task RejectAsync(int contractId, string userId)
        {
            var c = await _contractRepo.GetFullAsync(contractId);
            if (c is null) return;

            // Yalnız tərəflərdən biri rədd edə bilər
            var isParty = c.CreatedByUserId == userId || c.Parties.Any(p => p.UserId == userId);
            if (!isParty) return;

            c.Status = ContractStatus.Rejected;
            _contractRepo.Update(c);
            await _contractRepo.SaveChangesAsync();
        }

        private static Dictionary<string, string> ParseValues(string dataJson)
        {
            if (string.IsNullOrWhiteSpace(dataJson)) return new();
            try
            {
                return JsonSerializer.Deserialize<Dictionary<string, string>>(dataJson) ?? new();
            }
            catch
            {
                return new();
            }
        }
    }

}
