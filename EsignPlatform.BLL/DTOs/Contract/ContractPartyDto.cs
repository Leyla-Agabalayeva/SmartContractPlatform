using EsignPlatform.DAL.Enums;

namespace EsignPlatform.BLL.DTOs.Contract

{
    public class ContractPartyDto
    {
        public int Id { get; set; }
        public string DisplayName { get; set; } = null!;
        public string FinOrVoen { get; set; } = null!;
        public PartyRole Role { get; set; }
        public bool Signed { get; set; }
        public DateTime? SignedAt { get; set; }
        public string? UserId { get; set; }
    }

}
