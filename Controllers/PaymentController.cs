using GrupoCeleste.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GrupoCeleste.Controllers
{
    public class PaymentController : Controller
    {
        private readonly MercadoPagoService _mpService;
        public PaymentController(MercadoPagoService mpService)
        {
            _mpService = mpService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Pay(decimal monto, string membresiaNombre, string email)
        {
            var initPoint = await _mpService.CrearPreferenciaPagoAsync(monto, membresiaNombre, email);
            if (initPoint.StartsWith("Error"))
            {
                ViewBag.Mensaje = initPoint;
                return View("Index");
            }
            else
            {
                return Redirect(initPoint);
            }
        }

        [HttpGet]
        public IActionResult Success()
        {
            ViewBag.Mensaje = "Pago aprobado exitosamente";
            return View("Resultado");
        }

        [HttpGet]
        public IActionResult Failure()
        {
            ViewBag.Mensaje = "El pago fue rechazado o cancelado";
            return View("Resultado");
        }

        [HttpGet]
        public IActionResult Pending()
        {
            ViewBag.Mensaje = "El pago está pendiente de aprobación";
            return View("Resultado");
        }
    }
}
