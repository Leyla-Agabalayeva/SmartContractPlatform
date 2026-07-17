using EsignPlatform.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.DAL.Entities
{
    public class Signature : BaseEntity
    {
        public int ContractId { get; set; }
        public Contract Contract { get; set; } = null!;

        public int ContractPartyId { get; set; }
        public ContractParty ContractParty { get; set; } = null!;

        public string? UserId { get; set; }

        public SignatureType SignatureType { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string? IpAddress { get; set; }
    }

}
