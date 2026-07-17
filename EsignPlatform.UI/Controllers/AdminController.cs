using EsignPlatform.BLL.Services.Interfaces;
using EsignPlatform.DAL.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EsignPlatform.UI.Controllers
{
    [Authorize(Roles = DbInitializer.AdminRole)]
    public class AdminController : Controller
    {
        private readonly ITemplateService _templateService;

        public AdminController(ITemplateService templateService)
        {
            _templateService = templateService;
        }

        public async Task<IActionResult> Templates()
        {
            var templates = await _templateService.GetAllAsync();
            return View(templates);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(int id)
        {
            await _templateService.ToggleActiveAsync(id);
            TempData["Success"] = "Şablonun statusu dəyişdirildi.";
            return RedirectToAction(nameof(Templates));
        }
    }

}
