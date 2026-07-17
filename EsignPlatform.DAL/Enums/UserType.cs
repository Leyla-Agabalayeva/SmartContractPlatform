using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.DAL.Enums
{
    public enum UserType
    {
        [Display(Name = "Fiziki şəxs (FIN)")]
        Individual = 1,

        [Display(Name = "Hüquqi şəxs (VÖEN)")]
        Company = 2
    }
}
