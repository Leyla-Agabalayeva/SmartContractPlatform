using EsignPlatform.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EsignPlatform.UI.Controllers
{
    [Authorize]
    public class TemplatesController : Controller
    {
        private readonly ITemplateService _templateService;

        public TemplatesController(ITemplateService templateService)
        {
            _templateService = templateService;
        }

        public async Task<IActionResult> Index()
        {
            var templates = await _templateService.GetActiveAsync();
            return View(templates);
        }

        public async Task<IActionResult> Details(int id)
        {
            var template = await _templateService.GetDetailAsync(id);
            if (template is null) return NotFound();
            return View(template);
        }
    }

}
