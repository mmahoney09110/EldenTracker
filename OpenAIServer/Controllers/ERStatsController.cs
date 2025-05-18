using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OpenAIServer.Data;
using OpenAIServer.Entities;

namespace OpenAIServer.Controllers
{
    public class ERStatsController : Controller
    {
        private readonly OpenAIServerContext _context;

        public ERStatsController(OpenAIServerContext context)
        {
            _context = context;
        }

        // GET: ERStats
        public async Task<IActionResult> Index()
        {
            return View(await _context.ERStats.ToListAsync());
        }

        // GET: ERStats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eRStats = await _context.ERStats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eRStats == null)
            {
                return NotFound();
            }

            return View(eRStats);
        }

        // GET: ERStats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ERStats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Level,ResposneCount,CreatedAt,UpdatedAt")] ERStats eRStats)
        {
            if (ModelState.IsValid)
            {
                eRStats.CreatedAt = DateTime.Now.ToUniversalTime();
                eRStats.UpdatedAt = DateTime.Now.ToUniversalTime();
                _context.Add(eRStats);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eRStats);
        }

        // GET: ERStats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eRStats = await _context.ERStats.FindAsync(id);
            if (eRStats == null)
            {
                return NotFound();
            }
            return View(eRStats);
        }

        // POST: ERStats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Level,ResposneCount,CreatedAt,UpdatedAt")] ERStats eRStats)
        {
            if (id != eRStats.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var original = await _context.ERStats.AsNoTracking().FirstOrDefaultAsync(x => x.Id == eRStats.Id);
                if (original == null) return NotFound();

                try
                {
                    eRStats.CreatedAt = original.CreatedAt;
                    eRStats.UpdatedAt = DateTime.Now.ToUniversalTime();
                    _context.Update(eRStats);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ERStatsExists(eRStats.Id))
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
            return View(eRStats);
        }

        // GET: ERStats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eRStats = await _context.ERStats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eRStats == null)
            {
                return NotFound();
            }

            return View(eRStats);
        }

        // POST: ERStats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eRStats = await _context.ERStats.FindAsync(id);
            if (eRStats != null)
            {
                _context.ERStats.Remove(eRStats);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ERStatsExists(int id)
        {
            return _context.ERStats.Any(e => e.Id == id);
        }
    }
}
