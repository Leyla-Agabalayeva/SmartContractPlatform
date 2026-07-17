using EsignPlatform.DAL.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.DAL.Entities
{
    // Identity istifadəçisi. Id string olaraq qalır (IdentityUser-dən gəlir).
    public class AppUser : IdentityUser
    {
        public UserType UserType { get; set; }

        // Fiziki şəxs üçün FIN, hüquqi şəxs üçün VÖEN
        public string FinOrVoen { get; set; } = null!;

        // Fiziki şəxs: Ad Soyad; Hüquqi şəxs: Şirkət adı
        public string DisplayName { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Contract> CreatedContracts { get; set; } = new List<Contract>();
    }

}
