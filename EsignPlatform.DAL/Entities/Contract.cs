using EsignPlatform.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.DAL.Entities
{
    public class Contract : BaseEntity
    {
        public string Title { get; set; } = null!;

        public int TemplateId { get; set; }
        public Template Template { get; set; } = null!;

        // Yaradan istifadəçi (Identity Id -> string)
        public string CreatedByUserId { get; set; } = null!;
        public AppUser CreatedByUser { get; set; } = null!;

        public ContractStatus Status { get; set; } = ContractStatus.Draft;

        // Template field-lərinin doldurulmuş dəyərləri (JSON object)
        public string DataJson { get; set; } = "{}";

        // Navigation
        public ICollection<ContractParty> Parties { get; set; } = new List<ContractParty>();
        public ICollection<Signature> Signatures { get; set; } = new List<Signature>();
        public Document? Document { get; set; }
    }

}
