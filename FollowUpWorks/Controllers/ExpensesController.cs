using AspNetCoreGeneratedDocument;
using FollowUpWorks.DTOs;
using FollowUpWorks.DTOs.ExpensesClassDTOSection;
using FollowUpWorks.Models;
using FollowUpWorks.Models.ExpensesClassSection;
using FollowUpWorks.services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FollowUpWorks.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly CustomQuerableOperationsService _service;

        public ExpensesController(CustomQuerableOperationsService service)
        {
            _service = service;
        }

        // GET: Expenses/Index
        public IActionResult Index(string category, int? month, int? year) // Añadimos parámetros para filtros
        {
            // 1. Obtener todos los gastos
            var response = _service.GetAllGeneric<ExpensesClass, ExpensesClassDTO>();

            if (!response.IsSuccess)
            {
                ViewBag.Errors = response.Errors;
                return View(new ExpensesIndexViewModel()); // Devolver ViewModel vacío si falla
            }

            // 2. Aplicar lógica de filtrado (usando los parámetros opcionales)
            var allExpenses = response.Result ?? new List<ExpensesClassDTO>();
            var filteredList = allExpenses.AsEnumerable();

            // Lógica de Filtrado (Aquí puedes implementar la lógica de mes/año si la necesitas)
            if (!string.IsNullOrEmpty(category) && category != "Todas")
            {
                filteredList = filteredList.Where(e =>
                    e.Category != null && e.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            }

            // (Opcional) Filtrar por mes/año, asumiendo que ExpensesClassDTO tiene una propiedad Date
            var currentMonth = month ?? DateTime.Now.Month;
            var currentYear = year ?? DateTime.Now.Year;

            filteredList = filteredList.Where(e => e.Date.Month == currentMonth && e.Date.Year == currentYear);


            // 3. Calcular Resúmenes y preparar el ViewModel
            var viewModel = new ExpensesIndexViewModel
            {
                Expenses = filteredList.ToList(),
                SelectedCategory = category ?? "Todas",
                SelectedMonth = currentMonth,
                SelectedYear = currentYear,
                // Lista de todas las categorías únicas para el filtro de la vista
                Categories = allExpenses.Select(e => e.Category).Where(c => c != null).Distinct().ToList()
            };

            // 4. Cálculos de Resumen (Totales y Agrupación)
            viewModel.TotalExpenses = viewModel.Expenses.Sum(e => e.Amount);

            viewModel.ExpensesByCategory = viewModel.Expenses
                .Where(e => e.Category != null) // Asegura que la categoría no sea null
                .GroupBy(e => e.Category!) // Agrupa por categoría
                .Select(g => new ExpenseCategorySummaryDTO // Mapea al DTO de resumen
                {
                    Category = g.Key,
                    Total = g.Sum(e => e.Amount),
                    Count = g.Count()
                })
                .OrderByDescending(s => s.Total) // Ordena por la más grande primero
                .ToList();

            // 5. Devolver la vista con el ViewModel completo
            return View(viewModel);
        }

        // GET: Event/Details/5
        public IActionResult Details(Guid id)
        {
            var response = _service.GetOneGeneric<ExpensesClass, ExpensesClassDTO>(id);
           

            if (!response.IsSuccess)
            {
                TempData["Errors"] = response.Errors;
                return RedirectToAction(nameof(Index));
            }

            return View(response.Result);
        }


        public IActionResult expensesCategory(string category)
        {
            // Usamos RedirectToAction para usar la nueva lógica de filtrado en Index
            return RedirectToAction(nameof(Index), new { category = category });
        }

        // GET: Event/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Event/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ExpensesClassDTO dto)
        {
            

            var response = _service.CreateGeneric<ExpensesClass, ExpensesClassDTO>(dto);

            if (!response.IsSuccess)
            {
                ViewBag.Errors = response.Errors;
                return View(dto);
            }

            TempData["SuccessMessage"] = response.Message;
            return RedirectToAction(nameof(Index));
        }

        // GET: Event/Edit/5
        public IActionResult Edit(Guid id)
        {
            var response = _service.GetOneGeneric<ExpensesClass, ExpensesClassDTO>(id);

            if (!response.IsSuccess)
            {
                TempData["Errors"] = response.Errors;
                return RedirectToAction(nameof(Index));
            }

            return View(response.Result);
        }

        // POST: Event/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, ExpensesClassDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var response = _service.UpdateGeneric<ExpensesClass, ExpensesClassDTO>(id, dto);

            if (!response.IsSuccess)
            {
                ViewBag.Errors = response.Errors;
                return View(dto);
            }

            TempData["SuccessMessage"] = response.Message;
            return RedirectToAction(nameof(Index));
        }

        // GET: Event/Delete/5
        public IActionResult Delete(Guid id)
        {
            var response = _service.DeleteGeneric<ExpensesClass>(id);

            if (!response.IsSuccess)
            {
                TempData["ErrorMessage"] = response.Errors;
            }
            else
            {
                TempData["SuccessMessage"] = response.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var response = _service.DeleteGeneric<ExpensesClass>(id);

            if (!response.IsSuccess)
            {
                TempData["Errors"] = response.Errors;
            }
            else
            {
                TempData["SuccessMessage"] = response.Message;
            }

            return RedirectToAction(nameof(Index));
        }
        
    }
}
