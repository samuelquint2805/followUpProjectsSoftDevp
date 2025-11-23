using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FollowUpWorks.Controllers
{
    public class MemoryGameController : Controller
    {
        // GET: MemoryGameController
        public ActionResult Index()
        {
            return View();
        }

      

        // GET: MemoryGameController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MemoryGameController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MemoryGameController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MemoryGameController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MemoryGameController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MemoryGameController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
