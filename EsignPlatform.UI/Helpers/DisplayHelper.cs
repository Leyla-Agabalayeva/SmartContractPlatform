using EsignPlatform.DAL.Enums;

namespace EsignPlatform.UI.Helpers
{
    public static class DisplayHelper
    {
        public static string CategoryName(TemplateCategory c) => c switch
        {
            TemplateCategory.Rental => "Kirayə müqaviləsi",
            TemplateCategory.Service => "Xidmət müqaviləsi",
            TemplateCategory.Sale => "Satış müqaviləsi",
            TemplateCategory.Debt => "Borc müqaviləsi",
            _ => c.ToString()
        };

        public static string CategoryIcon(TemplateCategory c) => c switch
        {
            TemplateCategory.Rental => "🏠",
            TemplateCategory.Service => "🛠️",
            TemplateCategory.Sale => "🤝",
            TemplateCategory.Debt => "💰",
            _ => "📄"
        };

        public static string RoleName(PartyRole r) => r switch
        {
            PartyRole.Owner => "Kirayə verən",
            PartyRole.Tenant => "Kirayəçi",
            PartyRole.Provider => "Xidmət göstərən",
            PartyRole.Client => "Sifarişçi",
            PartyRole.Seller => "Satıcı",
            PartyRole.Buyer => "Alıcı",
            PartyRole.Creditor => "Borc verən",
            PartyRole.Debtor => "Borc alan",
            _ => r.ToString()
        };

        public static string StatusName(ContractStatus s) => s switch
        {
            ContractStatus.Draft => "Qaralama",
            ContractStatus.Pending => "İmza gözləyir",
            ContractStatus.PartiallySigned => "Qismən imzalanıb",
            ContractStatus.FullySigned => "Tam imzalanıb",
            ContractStatus.Rejected => "Rədd edilib",
            _ => s.ToString()
        };

        public static string StatusPill(ContractStatus s) => s switch
        {
            ContractStatus.Draft => "pill pill-draft",
            ContractStatus.Pending => "pill pill-pending",
            ContractStatus.PartiallySigned => "pill pill-partial",
            ContractStatus.FullySigned => "pill pill-full",
            ContractStatus.Rejected => "pill pill-reject",
            _ => "pill pill-draft"
        };

        public static string UserTypeName(UserType t) => t switch
        {
            UserType.Individual => "Fiziki şəxs",
            UserType.Company => "Hüquqi şəxs",
            _ => t.ToString()
        };
    }

}
