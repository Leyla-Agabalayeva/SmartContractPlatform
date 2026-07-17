using EsignPlatform.BLL.DTOs.Signature;
using EsignPlatform.BLL.Services.Interfaces;
using EsignPlatform.DAL.Entities;
using EsignPlatform.DAL.Enums;
using EsignPlatform.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.BLL.Services.Implementations
{

    public class SignatureService : ISignatureService
    {
        private readonly IContractRepository _contractRepo;
        private readonly IRepository<Signature> _signatureRepo;
        private readonly IOtpService _otp;

        public SignatureService(
            IContractRepository contractRepo,
            IRepository<Signature> signatureRepo,
            IOtpService otp)
        {
            _contractRepo = contractRepo;
            _signatureRepo = signatureRepo;
            _otp = otp;
        }

        public async Task<bool> CanUserSignAsync(int contractId, string userId)
        {
            var c = await _contractRepo.GetFullAsync(contractId);
            if (c is null) return false;
            if (c.Status is ContractStatus.FullySigned or ContractStatus.Rejected) return false;

            var party = c.Parties.FirstOrDefault(p => p.UserId == userId);
            return party is not null && !party.Signed;
        }

        public async Task<StartSignResult> StartSignAsync(int contractId, string userId)
        {
            var c = await _contractRepo.GetFullAsync(contractId);
            if (c is null)
                return new StartSignResult { Success = false, Message = "Müqavilə tapılmadı." };

            var party = c.Parties.FirstOrDefault(p => p.UserId == userId);
            if (party is null)
                return new StartSignResult { Success = false, Message = "Siz bu müqavilənin tərəfi deyilsiniz." };
            if (party.Signed)
                return new StartSignResult { Success = false, Message = "Siz artıq imzalamısınız." };

            var code = _otp.Generate(OtpKey(contractId, userId));

            // MVP: real SMS gateway yoxdur. Kod demo üçün qaytarılır və konsola yazılır.
            Console.WriteLine($"[OTP] Contract #{contractId} / User {userId} => {code}");

            return new StartSignResult
            {
                Success = true,
                Message = "OTP kodu göndərildi (demo).",
                DemoOtpCode = code
            };
        }

        public async Task<StartSignResult> VerifyAndSignAsync(VerifyOtpDto dto, string userId, string? ipAddress)
        {
            var c = await _contractRepo.GetFullAsync(dto.ContractId);
            if (c is null)
                return new StartSignResult { Success = false, Message = "Müqavilə tapılmadı." };

            var party = c.Parties.FirstOrDefault(p => p.UserId == userId);
            if (party is null || party.Signed)
                return new StartSignResult { Success = false, Message = "İmzalamaq mümkün deyil." };

            var key = OtpKey(dto.ContractId, userId);
            if (!_otp.Validate(key, dto.Code))
                return new StartSignResult { Success = false, Message = "OTP kodu yanlış və ya vaxtı bitib." };

            // İmzanı qeyd et
            party.Signed = true;
            party.SignedAt = DateTime.UtcNow;

            c.Signatures.Add(new Signature
            {
                ContractId = c.Id,
                ContractPartyId = party.Id,
                UserId = userId,
                SignatureType = SignatureType.Otp,
                Timestamp = DateTime.UtcNow,
                IpAddress = ipAddress
            });

            // Status yenilə
            var total = c.Parties.Count;
            var signed = c.Parties.Count(p => p.Signed);
            c.Status = signed switch
            {
                _ when signed == total => ContractStatus.FullySigned,
                > 0 => ContractStatus.PartiallySigned,
                _ => c.Status
            };

            _contractRepo.Update(c);
            await _contractRepo.SaveChangesAsync();
            _otp.Remove(key);

            return new StartSignResult
            {
                Success = true,
                Message = c.Status == ContractStatus.FullySigned
                    ? "Müqavilə tam imzalandı."
                    : "İmzanız qeydə alındı. Digər tərəfin imzası gözlənilir."
            };
        }

        private static string OtpKey(int contractId, string userId) => $"{contractId}:{userId}";
    }

}
