using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DOTNETCOREEXAMPLE.DataContext;
using DOTNETCOREEXAMPLE.Models;

namespace DOTNETCOREEXAMPLE.Controllers
{
    public class SLOTMASTERController : Controller
    {
        private readonly ApplicationDBContext _context;

        public SLOTMASTERController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: SLOTMASTER
        public async Task<IActionResult> Index()
        {
            return View(await _context.obj_SLOT_MASTER.ToListAsync());
        }

        // GET: SLOTMASTER/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var class_SLOT_MASTER = await _context.obj_SLOT_MASTER
                .FirstOrDefaultAsync(m => m.SLOTID == id);
            if (class_SLOT_MASTER == null)
            {
                return NotFound();
            }

            return View(class_SLOT_MASTER);
        }

        // GET: SLOTMASTER/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SLOTMASTER/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SLOTID,SLOTNAME,ISACTIVE")] Class_SLOT_MASTER class_SLOT_MASTER)
        {
            if (ModelState.IsValid)
            {
                _context.Add(class_SLOT_MASTER);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(class_SLOT_MASTER);
        }

        // GET: SLOTMASTER/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var class_SLOT_MASTER = await _context.obj_SLOT_MASTER.FindAsync(id);
            if (class_SLOT_MASTER == null)
            {
                return NotFound();
            }
            return View(class_SLOT_MASTER);
        }

        // POST: SLOTMASTER/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SLOTID,SLOTNAME,ISACTIVE")] Class_SLOT_MASTER class_SLOT_MASTER)
        {
            if (id != class_SLOT_MASTER.SLOTID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(class_SLOT_MASTER);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Class_SLOT_MASTERExists(class_SLOT_MASTER.SLOTID))
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
            return View(class_SLOT_MASTER);
        }

        // GET: SLOTMASTER/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var class_SLOT_MASTER = await _context.obj_SLOT_MASTER
                .FirstOrDefaultAsync(m => m.SLOTID == id);
            if (class_SLOT_MASTER == null)
            {
                return NotFound();
            }

            return View(class_SLOT_MASTER);
        }

        // POST: SLOTMASTER/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var class_SLOT_MASTER = await _context.obj_SLOT_MASTER.FindAsync(id);
            _context.obj_SLOT_MASTER.Remove(class_SLOT_MASTER);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Class_SLOT_MASTERExists(int id)
        {
            return _context.obj_SLOT_MASTER.Any(e => e.SLOTID == id);
        }
        public string GetStotName(int SLOTID)
        {
            var strSlotName = _context.obj_SLOT_MASTER.Where(s => s.SLOTID == SLOTID).Select(s => s.SLOTNAME).FirstOrDefault();
            return strSlotName;
        }
    }
}
