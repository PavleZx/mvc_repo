using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var items = await _context.Items.Include(s => s.SerialNumber)
                                            .Include(c => c.Category)
                                            .ToListAsync();
            return View(items);
            
        }

        public IActionResult Create()
        {
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id, Name, Price, CategoryId")] Item item, string SerialNumberName)
        {
            if (ModelState.IsValid)
            {
                // If a serial number was typed in, create the entity on the fly
                if (!string.IsNullOrEmpty(SerialNumberName))
                {
                    item.SerialNumber = new SerialNumber { Name = SerialNumberName };
                }

                _context.Items.Add(item);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View(item);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.Items
                .Include(i => i.SerialNumber)  // also need this to load the navigation property
                .FirstOrDefaultAsync(x => x.Id == id);

            if (item == null) return NotFound();

            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
            ViewBag.SerialNumberName = item.SerialNumber?.Name;

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([Bind("Id, Name, Price, CategoryId")] Item item, string SerialNumberName)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(SerialNumberName))
                {
                    // Find the EXISTING serial number for this item
                    var existingSerial = await _context.SerialNumbers
                        .FirstOrDefaultAsync(s => s.ItemId == item.Id);

                    if (existingSerial != null)
                    {
                        // Update it, don't create a new one
                        existingSerial.Name = SerialNumberName;
                        _context.SerialNumbers.Update(existingSerial);
                    }
                    else
                    {
                        // No serial number yet, create one
                        item.SerialNumber = new SerialNumber { Name = SerialNumberName };
                    }
                }

                _context.Items.Update(item);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View(item);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);
            return View(item);
        }
        
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteSelectedItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

    }
}