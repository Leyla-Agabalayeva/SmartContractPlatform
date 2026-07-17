using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.DAL.Enums
{

    public enum ContractStatus
    {
        Draft = 1,
        Pending = 2,          // imza gözləyir
        PartiallySigned = 3,
        FullySigned = 4,
        Rejected = 5
    }

}
