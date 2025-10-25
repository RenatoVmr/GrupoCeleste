using Microsoft.AspNetCore.Mvc;

namespace GrupoCeleste.Controllers
{
    public class MembershipController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Seleccionar(int id)
        {
            ViewBag.MembresiaId = id;
            return View();
        }
    }
}
