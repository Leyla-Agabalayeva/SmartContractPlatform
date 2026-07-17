using EsignPlatform.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.DAL.Entities
{
    public class ContractParty : BaseEntity
    {
        public int ContractId { get; set; }
        public Contract Contract { get; set; } = null!;

        // Sistemdə qeydiyyatdan keçibsə - user, yoxdursa null
        public string? UserId { get; set; }
        public AppUser? User { get; set; }

        public string FinOrVoen { get; set; } = null!;
        public string DisplayName { get; set; } = null!;

        public PartyRole Role { get; set; }

        public bool Signed { get; set; }
        public DateTime? SignedAt { get; set; }
    }

}
