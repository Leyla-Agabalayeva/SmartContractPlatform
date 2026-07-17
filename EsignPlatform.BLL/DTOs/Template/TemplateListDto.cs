using EsignPlatform.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.BLL.DTOs.Template
{

    public class TemplateListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public TemplateCategory Category { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
