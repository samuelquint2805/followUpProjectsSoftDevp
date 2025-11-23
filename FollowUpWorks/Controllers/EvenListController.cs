using AutoMapper;
using FollowUpWorks.DTOs;
using FollowUpWorks.Models;
using FollowUpWorks.services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FollowUpWorks.Controllers
{
    public class EvenListController : Controller
    {
        private readonly CustomQuerableOperationsService _service;

        public EvenListController(CustomQuerableOperationsService service)
        {
            _service = service;
        }

        // GET: Event/Index
        public IActionResult Index()
        {
            var response = _service.GetAllGeneric<EventClass, EventClassDTO>();

            if (!response.IsSuccess)
            {
                ViewBag.Errors = response.Errors;
                return View(new List<EventClassDTO>());
            }

            return View(response.Result);
        }

        // GET: Event/Details/5
        public IActionResult Details(Guid id)
        {
            var response = _service.GetOneGeneric<EventClass, EventClassDTO>(id);

            if (!response.IsSuccess)
            {
                TempData["Errors"] = response.Errors;
                return RedirectToAction(nameof(Index));
            }

            return View(response.Result);
        }

        // GET: Event/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Event/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EventClassDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var response = _service.CreateGeneric<EventClass, EventClassDTO>(dto);

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
            var response = _service.GetOneGeneric<EventClass, EventClassDTO>(id);

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
        public IActionResult Edit(Guid id, EventClassDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var response = _service.UpdateGeneric<EventClass, EventClassDTO>(id, dto);

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
            var response = _service.DeleteGeneric<EventClass>(id);

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
            var response = _service.DeleteGeneric<EventClass>(id);

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
