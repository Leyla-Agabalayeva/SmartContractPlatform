using EsignPlatform.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.BLL.DTOs.Contract
{


    public class ContractListDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string TemplateName { get; set; } = null!;
        public ContractStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int PartiesCount { get; set; }
        public int SignedCount { get; set; }
    }

}
