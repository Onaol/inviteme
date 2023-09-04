using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InviteMe.Data;
using InviteMe.Models;
using InviteMe.Extensions;

namespace InviteMe.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
              return _context.EventCategories.Where(_ => _.TenantId == User.GetUserId()) != null ? 
                          View(await _context.EventCategories.Where(_ => _.TenantId == User.GetUserId()).ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.EventCategories'  is null.");
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.EventCategories.Where(_ => _.TenantId == User.GetUserId()) == null)
            {
                return NotFound();
            }

            var eventCategory = await _context.EventCategories.Where(_ => _.TenantId == User.GetUserId())
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventCategory == null)
            {
                return NotFound();
            }

            return View(eventCategory);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] EventCategory eventCategory)
        {
            eventCategory.Id = Guid.NewGuid();
            eventCategory.TenantId = User.GetUserId();
            _context.Add(eventCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));           
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.EventCategories == null)
            {
                return NotFound();
            }

            var eventCategory = await _context.EventCategories.FindAsync(id);
            if (eventCategory == null)
            {
                return NotFound();
            }
            return View(eventCategory);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,TenantId,Name,Description")] EventCategory eventCategory)
        {
            if (id != eventCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventCategoryExists(eventCategory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(eventCategory);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.EventCategories == null)
            {
                return NotFound();
            }

            var eventCategory = await _context.EventCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventCategory == null)
            {
                return NotFound();
            }

            return View(eventCategory);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.EventCategories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.EventCategories'  is null.");
            }
            var eventCategory = await _context.EventCategories.FindAsync(id);
            if (eventCategory != null)
            {
                _context.EventCategories.Remove(eventCategory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventCategoryExists(Guid id)
        {
          return (_context.EventCategories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
