using EsignPlatform.BLL.Services.Interfaces;
using EsignPlatform.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EsignPlatform.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITemplateService _templateService;

        public HomeController(ITemplateService templateService)
        {
            _templateService = templateService;
        }

        public async Task<IActionResult> Index()
        {
            // Ana səhifədə aktiv şablonları göstər
            var templates = await _templateService.GetActiveAsync();
            return View(templates);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}