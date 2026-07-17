using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.DAL.Entities
{

    public class Document : BaseEntity
    {
        public int ContractId { get; set; }
        public Contract Contract { get; set; } = null!;

        public string FileName { get; set; } = null!;

        // MVP-də local storage yolu (MinIO əvəzinə). URL saxlamaq üçün genişlənə bilər.
        public string FilePath { get; set; } = null!;

        // PDF-in SHA256 hash-i (audit üçün, dəyişdirilə bilməz)
        public string Hash { get; set; } = null!;
    }

}
