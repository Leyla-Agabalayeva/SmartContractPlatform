using EsignPlatform.BLL.DTOs.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EsignPlatform.DAL.Enums;

namespace EsignPlatform.BLL.DTOs.Contract
{
    public class ContractDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string TemplateName { get; set; } = null!;
        public ContractStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByName { get; set; } = null!;
        public string CreatedByUserId { get; set; } = null!;

        // Field label -> value (göstərmək üçün)
        public List<TemplateFieldDto> Fields { get; set; } = new();
        public Dictionary<string, string> FieldValues { get; set; } = new();

        public List<ContractPartyDto> Parties { get; set; } = new();

        public bool HasDocument { get; set; }
    }

}
