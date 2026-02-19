using Microsoft.AspNetCore.Mvc;

namespace mvc_app.Controllers
{
    public class ExpenseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}