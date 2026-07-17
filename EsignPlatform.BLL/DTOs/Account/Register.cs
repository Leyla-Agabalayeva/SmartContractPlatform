using EsignPlatform.DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.BLL.DTOs.Account
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "İstifadəçi növü seçilməlidir")]
        [Display(Name = "İstifadəçi növü")]
        public UserType UserType { get; set; } = UserType.Individual;

        [Required(ErrorMessage = "FIN / VÖEN mütləqdir")]
        [Display(Name = "FIN / VÖEN")]
        [StringLength(20, MinimumLength = 5)]
        public string FinOrVoen { get; set; } = null!;

        [Required(ErrorMessage = "Ad / Şirkət adı mütləqdir")]
        [Display(Name = "Ad Soyad / Şirkət adı")]
        [StringLength(200)]
        public string DisplayName { get; set; } = null!;

        [Required, EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Display(Name = "Telefon nömrəsi")]
        [Phone]
        public string? PhoneNumber { get; set; }

        [Required, DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        [Display(Name = "Şifrə")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Şifrələr uyğun gəlmir")]
        [Display(Name = "Şifrənin təkrarı")]
        public string ConfirmPassword { get; set; } = null!;
    }

}
