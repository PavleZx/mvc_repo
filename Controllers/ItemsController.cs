using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc_app.Data;
using mvc_app.Models;

namespace mvc_app.Controllers
{
    public class ItemsController : Controller
    {

        private readonly MyContext _context;

        public ItemsController(MyContext context)
        {
            _context = context;
        }   
        // public IActionResult Index()
        // {
        //     return View();
        // }

        public IActionResult Overview()
        {
            // !!!
            // PROGRAM DAJE ID SAM ID, TAKO DA GA NE TREBAMO STAVLJATI
            // !!!
            var item = new Item() { Name = "Test item" };
            
            return View(item);
        }

        // ako se stavi parametar itemId, onda se moze passati vrijednost id-ja u querrystring (sa ?itemid=1)
        // ali se nece moci passati kao url parametar jer se tocno tako zove item class varijabla

        // ako se koristi bilo koji drugi naziv, onda se moze passati kao url parametar (sa /items/edit/1)
        // i kao querry string (sa ?id=1) 
        

        public IActionResult Edit(int itemId)
        {
            return Content($"Edit item with ID: {itemId}");
        }

        public async Task<IActionResult> Index()
        {
            var items = await _context.Items.ToListAsync();
            return View(items);
            
        }
    }
}