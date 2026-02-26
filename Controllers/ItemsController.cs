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

        // Moramo napraviti Controller konstruktor koji prija našu impleementaciju baze podataka, kako bi mogli koristiti bazu u svim metodama unutar kontrolera
        // to je tako napravljeno da bismo mogli koristiti različite implementacije baze podataka, a da ne moramo mijenjati kod unutar metoda, već samo konstruktor

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
        


        // ova funkcija je samo za isprobavanje asp-controller
        // i asp-action tag helpera
        // tu bi se stavljalo asp-controller="Items"
        // i asp-action = "Edit_demo" u <a> element
        public IActionResult Edit_demo(int itemId)
        {
            return Content($"Edit item with ID: {itemId}");
        }

        public async Task<IActionResult> Index()
        {
            var items = await _context.Items.ToListAsync();
            return View(items);
            
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id, Name, Price")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Items.Add(item);
                await _context.SaveChangesAsync();

                // asfter adding the newly created item to teh context and saving changes
                // we redirect the user to the Items/Index page
                return RedirectToAction("Index");
            }
            return View(item);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);
            return View(item);

        }

        [HttpPost]
        public async Task<IActionResult> Edit([Bind("ID, Name, Price")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Items.Add(item);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(item);

        }

    }
}