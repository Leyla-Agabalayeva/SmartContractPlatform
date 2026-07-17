using EsignPlatform.BLL.DTOs.Signature;
using EsignPlatform.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EsignPlatform.UI.Controllers
{
    [Authorize]
    public class SigningController : Controller
    {
        private readonly ISignatureService _signatureService;
        private readonly IContractService _contractService;

        public SigningController(ISignatureService signatureService, IContractService contractService)
        {
            _signatureService = signatureService;
            _contractService = contractService;
        }

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet]
        public async Task<IActionResult> Sign(int id)
        {
            var contract = await _contractService.GetDetailAsync(id);
            if (contract is null) return NotFound();

            if (!await _signatureService.CanUserSignAsync(id, UserId))
            {
                TempData["Error"] = "Bu müqaviləni imzalaya bilməzsiniz (artıq imzalanıb və ya tərəf deyilsiniz).";
                return RedirectToAction("Details", "Contracts", new { id });
            }

            ViewBag.Contract = contract;
            return View(new VerifyOtpDto { ContractId = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartOtp(int id)
        {
            var result = await _signatureService.StartSignAsync(id, UserId);
            if (!result.Success)
                TempData["Error"] = result.Message;
            else
            {
                TempData["Success"] = result.Message;
                // MVP/demo: real SMS yoxdur, kodu ekranda göstəririk
                TempData["DemoOtp"] = result.DemoOtpCode;
            }
            return RedirectToAction(nameof(Sign), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyOtp(VerifyOtpDto dto)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Sign), new { id = dto.ContractId });

            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var result = await _signatureService.VerifyAndSignAsync(dto, UserId, ip);

            if (result.Success)
                TempData["Success"] = result.Message;
            else
                TempData["Error"] = result.Message;

            return RedirectToAction("Details", "Contracts", new { id = dto.ContractId });
        }
    }

}
