using GrupoCeleste.Data;
using GrupoCeleste.Models;
using GrupoCeleste.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GrupoCeleste.Controllers
{
    [Authorize]
    public class RecommendationsController : Controller
    {
        private readonly RecommendationService _recommendationService;
        private readonly ApplicationDbContext _context;

        public RecommendationsController(RecommendationService recommendationService, ApplicationDbContext context)
        {
            _recommendationService = recommendationService;
            _context = context;
        }

        public IActionResult Index()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email) ?? User.Identity?.Name;
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account");
            }

            var recommendations = _recommendationService.GetRecommendationsForUser(userEmail, _context);
            return View(recommendations);
        }
    }
}