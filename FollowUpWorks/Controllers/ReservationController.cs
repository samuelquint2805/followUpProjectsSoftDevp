using FollowUpWorks.DTOs;
using FollowUpWorks.Models;
using FollowUpWorks.services.Abstractions;
using FollowUpWorks.services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FollowUpWorks.Controllers
{
    public class ReservationController : Controller
    {
        private readonly CustomQuerableOperationsService _service;
        private readonly IJSONReservation _jsonReservation;

        public ReservationController(CustomQuerableOperationsService service, IJSONReservation Reservation)
        {
            _service = service;
            _jsonReservation = Reservation;
        }

        // GET: Event/Index
        public IActionResult Index()
        {
            var recipes = _jsonReservation.GetAll(); // Carga desde el archivo JSON
                                                 // Convertir a DTO si es necesario antes de pasar a la vista

            return View(recipes);
        }

        // GET: Event/Details/5
        public IActionResult Details(Guid id)
        {
            var response = _service.GetOneGeneric<ReservationClass, ReservationClassDTO>(id);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ReservationClassDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto); // Retorna a la vista con errores
            }

            var newReservation = new ReservationClass
            {
                idReservation = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Username = dto.Username,
              ReservationDate = dto.ReservationDate,
                NumberOfGuests = dto.NumberOfGuests,
                    ContactInfo = dto.ContactInfo

            };

            // Cargar, agregar y guardar
            var reservation = _jsonReservation.GetAll();
            reservation.Add(newReservation);
            _jsonReservation.SaveAll(reservation);

            // Datos para el modal de confirmación
            TempData["ReservationConfirmed"] = true;
            TempData["ConfirmedName"] = newReservation.Username;
            TempData["ConfirmedDate"] = newReservation.ReservationDate.ToString("dd MMM yyyy");
            TempData["ConfirmedTime"] = newReservation.ReservationDate.ToString("hh:mm tt");
            TempData["ConfirmedGuests"] = newReservation.NumberOfGuests.ToString();

            TempData["SuccessMessage"] = "Reserva creada exitosamente";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            // 1. Cargar todas las recetas
            var reservation = _jsonReservation.GetAll();

            // 2. Buscar la receta específica
            var reservationToEdit = reservation.FirstOrDefault(r => r.idReservation == id);

            if (reservationToEdit == null)
            {
                return NotFound();
            }

            // Mapear el modelo (ReservationClass) al DTO para la vista (si es necesario)
            var dto = new ReservationClassDTO
            {
                idReservation = reservationToEdit.idReservation,
                Username = reservationToEdit.Username,
                ReservationDate = reservationToEdit.ReservationDate
                // ... Mapear el resto de propiedades
            };

            return View(dto);
        }

        // --- Acción POST para guardar los cambios ---
        [HttpPost]
        public IActionResult Edit(ReservationClassDTO dto)
        {
            if (ModelState.IsValid)
            {
                // 1. Mapear el DTO de vuelta a la clase de modelo (ReservationClass)
                var updatedModel = new ReservationClass
                {
                    idReservation = dto.idReservation,
                    Username = dto.Username,
                    ReservationDate = dto.ReservationDate,
                    NumberOfGuests = dto.NumberOfGuests,
                    ContactInfo = dto.ContactInfo
                };

                // 2. Llamar al servicio/repositorio para que actualice la lista y guarde el JSON
                _jsonReservation.UpdateReservation(updatedModel);

                return RedirectToAction("Index"); // Redirigir a la lista de recetas
            }

            return View(dto);
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            var reservation = _jsonReservation.GetAll().FirstOrDefault(r => r.idReservation == id);

            if (reservation == null)
            {
                return NotFound();
            }
            // Pasar el objeto a una vista de confirmación
            return RedirectToAction("Index");
        }

        // --- Acción POST para ejecutar la eliminación ---
        [HttpPost, ActionName("Delete")] // Especificamos el nombre de la acción en la ruta
        public IActionResult DeleteConfirmed(Guid id)
        {
            // 1. Llamar al repositorio para eliminar el registro del archivo JSON
            _jsonReservation.DeleteReservation(id);

            // 2. Redirigir a la lista principal
            return RedirectToAction("Index");
        }
    }
}
