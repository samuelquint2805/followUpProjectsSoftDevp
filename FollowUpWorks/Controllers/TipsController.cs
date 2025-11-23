using FollowUpWorks.DTOs;
using FollowUpWorks.Models;
using FollowUpWorks.services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FollowUpWorks.Controllers
{
    public class TipsController : Controller
    {
        

        public IActionResult Index()
        {
            return View(new TipClassDTO());
        }

        // POST: Tips/Calculate (AJAX)
        [HttpPost]
        public IActionResult Calculate([FromBody] TipClassDTO dto)
        {
            if (dto.TipAmount <= 0)
            {
                return Json(new { success = false, message = "Ingresa un monto válido" });
            }

            if (dto.tipPercentage <= 0 || dto.tipPercentage > 100)
            {
                return Json(new { success = false, message = "Ingresa un porcentaje válido (1-100)" });
            }

            // Calcular propina
            dto.TipResult = dto.TipAmount * dto.tipPercentage / 100;

            var total = dto.TipAmount + dto.TipResult;

            return Json(new
            {
                success = true,
                tipResult = dto.TipResult,
                total = total,
                tipResultFormatted = dto.TipResult.ToString("C2"),
                totalFormatted = total.ToString("C2")
            });
        }
    }
}
