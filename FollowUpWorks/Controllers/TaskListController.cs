using FollowUpWorks.DTOs;
using FollowUpWorks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace FollowUpWorks.Controllers
{
    public class TaskListController : Controller
    {
        private readonly IMemoryCache _cache;
        private const string TasksKey = "TaskList";

        public TaskListController(IMemoryCache cache)
        {
            _cache = cache;
        }

        private List<TaskListClass> GetTasks()
        {
            return _cache.GetOrCreate(TasksKey, e =>
            {
                e.SlidingExpiration = TimeSpan.FromHours(24);
                return new List<TaskListClass>();
            }) ?? new List<TaskListClass>();
        }

        private void SaveTasks(List<TaskListClass> tasks)
        {
            _cache.Set(TasksKey, tasks, TimeSpan.FromHours(24));
        }

        // GET: TaskList/Index
        public IActionResult Index()
        {
            var tasks = GetTasks();
            var dtos = tasks.Select(t => new TaskListClassDTO
            {
                idTask = t.Id,
                Title = t.Title,
                Description = t.Description,
                IsCompleted = t.IsCompleted,
                Date = t.Date
            }).OrderByDescending(t => t.Date).ToList();

            return View(dtos);
        }

        // POST: TaskList/Create (AJAX)
        [HttpPost]
        public IActionResult Create([FromBody] TaskListClassDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                return Json(new { success = false, message = "El título es requerido" });
            }

            var task = new TaskListClass
            {
                Id = Guid.NewGuid(),
                idTask = Guid.NewGuid(),
                Title = dto.Title.Trim(),
                Description = dto.Description?.Trim() ?? "",
                IsCompleted = false,
                Date = DateTime.Now
            };

            var tasks = GetTasks();
            tasks.Add(task);
            SaveTasks(tasks);

            return Json(new
            {
                success = true,
                task = new TaskListClassDTO
                {
                    idTask = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    IsCompleted = task.IsCompleted,
                    Date = task.Date
                }
            });
        }

        // POST: TaskList/Toggle (AJAX)
        [HttpPost]
        public IActionResult Toggle([FromBody] Guid id)
        {
            var tasks = GetTasks();
            var task = tasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                return Json(new { success = false, message = "Tarea no encontrada" });
            }

            task.IsCompleted = !(task.IsCompleted ?? false);
            SaveTasks(tasks);

            return Json(new { success = true, isCompleted = task.IsCompleted });
        }

        // POST: TaskList/Delete (AJAX)
        [HttpPost]
        public IActionResult Delete([FromBody] Guid id)
        {
            var tasks = GetTasks();
            var task = tasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                return Json(new { success = false, message = "Tarea no encontrada" });
            }

            tasks.Remove(task);
            SaveTasks(tasks);

            return Json(new { success = true });
        }

        // POST: TaskList/ClearCompleted (AJAX)
        [HttpPost]
        public IActionResult ClearCompleted()
        {
            var tasks = GetTasks();
            tasks.RemoveAll(t => t.IsCompleted == true);
            SaveTasks(tasks);

            return Json(new { success = true });
        }
    }
}