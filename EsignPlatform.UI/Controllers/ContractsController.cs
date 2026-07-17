using EsignPlatform.BLL.DTOs.Contract;
using EsignPlatform.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EsignPlatform.UI.Controllers
{
    [Authorize]
    public class ContractsController : Controller
    {
        private readonly IContractService _contractService;
        private readonly ITemplateService _templateService;
        private readonly IPdfService _pdfService;

        public ContractsController(
            IContractService contractService,
            ITemplateService templateService,
            IPdfService pdfService)
        {
            _contractService = contractService;
            _templateService = templateService;
            _pdfService = pdfService;
        }

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        // /Contracts -> mənim müqavilələrim
        public async Task<IActionResult> Index()
        {
            var contracts = await _contractService.GetMyContractsAsync(UserId);
            return View(contracts);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int templateId)
        {
            var model = await _contractService.BuildCreateModelAsync(templateId);
            if (model is null)
            {
                TempData["Error"] = "Şablon tapılmadı və ya aktiv deyil.";
                return RedirectToAction("Index", "Templates");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateContractDto dto)
        {
            // Field-lər POST-da itir, template-dən yenidən doldururuq (validation üçün)
            var rebuilt = await _contractService.BuildCreateModelAsync(dto.TemplateId);
            if (rebuilt is not null)
            {
                dto.Fields = rebuilt.Fields;
                dto.TemplateName = rebuilt.TemplateName;
            }

            if (!ModelState.IsValid) return View(dto);

            try
            {
                var id = await _contractService.CreateAsync(dto, UserId);
                TempData["Success"] = "Müqavilə yaradıldı.";
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var contract = await _contractService.GetDetailAsync(id);
            if (contract is null) return NotFound();
            return View(contract);
        }

        public async Task<IActionResult> DownloadPdf(int id)
        {
            var contract = await _contractService.GetDetailAsync(id);
            if (contract is null) return NotFound();

            var bytes = _pdfService.GenerateContractPdf(contract);
            var fileName = $"contract-{contract.Id}.pdf";
            return File(bytes, "application/pdf", fileName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            await _contractService.RejectAsync(id, UserId);
            TempData["Success"] = "Müqavilə rədd edildi.";
            return RedirectToAction(nameof(Details), new { id });
        }
    }

}
