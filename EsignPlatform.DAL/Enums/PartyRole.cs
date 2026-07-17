using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.DAL.Enums
{
    public enum PartyRole
    {
        [Display(Name = "Kirayə verən")] Owner = 1,
        [Display(Name = "Kirayəçi")] Tenant = 2,
        [Display(Name = "Xidmət göstərən")] Provider = 3,
        [Display(Name = "Sifarişçi")] Client = 4,
        [Display(Name = "Satıcı")] Seller = 5,
        [Display(Name = "Alıcı")] Buyer = 6,
        [Display(Name = "Borc verən")] Creditor = 7,
        [Display(Name = "Borc alan")] Debtor = 8
    }
}
