using FollowUpWorks.DTOs;
using FollowUpWorks.Models;
using FollowUpWorks.services.Abstractions;
using FollowUpWorks.services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FollowUpWorks.Controllers
{
    public class RecipeController : Controller
    {
        private readonly CustomQuerableOperationsService _service;
        private readonly IJSONRecipes _jsonRecipes;

        public RecipeController(CustomQuerableOperationsService service, IJSONRecipes Recipes)
        {
            _service = service;
            _jsonRecipes = Recipes;
        }

        // GET: Event/Index
        public IActionResult Index()
        {
            var recipes = _jsonRecipes.GetAll(); // Carga desde el archivo JSON
                                                // Convertir a DTO si es necesario antes de pasar a la vista

            return View(recipes);
        }

        // GET: Event/Details/5
        public IActionResult Details(Guid id)
        {
            var response = _service.GetOneGeneric<RecipeClass, RecipeClassDTO>(id);

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
        public IActionResult Create(RecipeClassDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto); // Retorna a la vista con errores
            }

            var newRecipe = new RecipeClass
            {
                idRecipe = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Ingredients = dto.Ingredients ?? new List<string>(),
                Instructions = dto.Instructions,
                PreparationTimeMinutes = dto.PreparationTimeMinutes,
                CookingTimeMinutes = dto.CookingTimeMinutes,
                Servings = dto.Servings,
                DishType = dto.DishType,
                MainIngredientCategory = dto.MainIngredientCategory
            };

            // Cargar, agregar y guardar
            var recipes = _jsonRecipes.GetAll();
            recipes.Add(newRecipe);
            _jsonRecipes.SaveAll(recipes);

            TempData["SuccessMessage"] = "Receta creada exitosamente";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            // 1. Cargar todas las recetas
            var recipes = _jsonRecipes.GetAll();

            // 2. Buscar la receta específica
            var recipeToEdit = recipes.FirstOrDefault(r => r.Id == id);

            if (recipeToEdit == null)
            {
                return NotFound();
            }

            // Mapear el modelo (RecipeClass) al DTO para la vista (si es necesario)
            var dto = new RecipeClassDTO
            {
                idRecipe = recipeToEdit.idRecipe,
                Name = recipeToEdit.Name,
                // ... Mapear el resto de propiedades
            };

            return View(dto);
        }

        // --- Acción POST para guardar los cambios ---
        [HttpPost]
        public IActionResult Edit(RecipeClassDTO recipeDto)
        {
            if (ModelState.IsValid)
            {
                // 1. Mapear el DTO de vuelta a la clase de modelo (RecipeClass)
                var updatedModel = new RecipeClass
                {
                    idRecipe = recipeDto.idRecipe,
                    Name = recipeDto.Name,
                    Ingredients = recipeDto.Ingredients,
                    Instructions = recipeDto.Instructions,
                    PreparationTimeMinutes = recipeDto.PreparationTimeMinutes,
                    CookingTimeMinutes = recipeDto.CookingTimeMinutes,
                    Servings = recipeDto.Servings
                };

                // 2. Llamar al servicio/repositorio para que actualice la lista y guarde el JSON
                _jsonRecipes.UpdateRecipe(updatedModel);

                return RedirectToAction("Index"); // Redirigir a la lista de recetas
            }

            return View(recipeDto);
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            var recipe = _jsonRecipes.GetAll().FirstOrDefault(r => r.idRecipe == id);

            if (recipe == null)
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
            _jsonRecipes.DeleteRecipe(id);

            // 2. Redirigir a la lista principal
            return RedirectToAction("Index");
        }
    }
}
