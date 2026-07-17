using EsignPlatform.BLL.DTOs.Template;
using EsignPlatform.DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.BLL.DTOs.Contract
{
    public class CreateContractDto
    {
        [Required]
        public int TemplateId { get; set; }

        [Required(ErrorMessage = "Müqavilə başlığı mütləqdir")]
        [Display(Name = "Müqavilə başlığı")]
        [StringLength(300)]
        public string Title { get; set; } = null!;

        // --- Qarşı tərəf (counterparty) ---
        [Required(ErrorMessage = "Qarşı tərəfin FIN / VÖEN-i mütləqdir")]
        [Display(Name = "Qarşı tərəf FIN / VÖEN")]
        public string CounterpartyFinOrVoen { get; set; } = null!;

        [Required(ErrorMessage = "Qarşı tərəfin adı mütləqdir")]
        [Display(Name = "Qarşı tərəf ad / şirkət")]
        public string CounterpartyName { get; set; } = null!;

        [Display(Name = "Sizin rolunuz")]
        public PartyRole CreatorRole { get; set; }

        [Display(Name = "Qarşı tərəfin rolu")]
        public PartyRole CounterpartyRole { get; set; }

        // Template field-lərinin dəyərləri: key -> value
        public Dictionary<string, string> FieldValues { get; set; } = new();

        // View render etmək üçün (POST-da geri doldurulur)
        public List<TemplateFieldDto> Fields { get; set; } = new();
        public string TemplateName { get; set; } = string.Empty;
    }

}
