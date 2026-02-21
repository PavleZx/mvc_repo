using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Mvc;
using mvc_app.Models;

namespace mvc_app.Controllers
{
    public class ItemsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Overview()
        {
            // !!!
            // PROGRAM DAJE ID SAM ID, TAKO DA GA NE TREBAMO STAVLJATI
            // !!!
            var item = new Item() { Name = "Test item" };
            
            return View(item);
        }

        public IActionResult Edit(int itemId)
        {
            return Content($"Edit item with ID: {itemId}");
        }
    }
}