using EsignPlatform.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.DAL.Entities
{
    public class Template : BaseEntity
    {
        public string Name { get; set; } = null!;
        public TemplateCategory Category { get; set; }
        public string? Description { get; set; }

        // Dinamik field-lərin tərifi (JSON array):
        // [{ "key":"rentAmount", "label":"Kirayə məbləği", "type":"number", "required":true }, ...]
        public string SchemaJson { get; set; } = "[]";

        public bool IsActive { get; set; } = true;

        // Navigation
        public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    }

}
