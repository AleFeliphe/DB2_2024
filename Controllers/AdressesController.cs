﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BTecpar.Models;

namespace BTecpar.Controllers
{
    public class AdressesController : Controller
    {
        private readonly MyDbContext _context;

        public AdressesController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Adresses
        public async Task<IActionResult> Index()
        {
            var myDbContext = _context.Adresses.Include(a => a.Client);
            return View(await myDbContext.ToListAsync());
        }

        // GET: Adresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adress = await _context.Adresses
                .Include(a => a.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adress == null)
            {
                return NotFound();
            }

            return View(adress);
        }

        // GET: Adresses/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.clients, "Id", "Id");
            return View();
        }

        // POST: Adresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Street,Number,Neighborhood,City,UF,Country,ClientId")] Adress adress)
        {
            if (ModelState.IsValid)
            {
                _context.Add(adress);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.clients, "Id", "Id", adress.ClientId);
            return View(adress);
        }

        // GET: Adresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adress = await _context.Adresses.FindAsync(id);
            if (adress == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.clients, "Id", "Id", adress.ClientId);
            return View(adress);
        }

        // POST: Adresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Street,Number,Neighborhood,City,UF,Country,ClientId")] Adress adress)
        {
            if (id != adress.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdressExists(adress.Id))
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
            ViewData["ClientId"] = new SelectList(_context.clients, "Id", "Id", adress.ClientId);
            return View(adress);
        }

        // GET: Adresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adress = await _context.Adresses
                .Include(a => a.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adress == null)
            {
                return NotFound();
            }

            return View(adress);
        }

        // POST: Adresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var adress = await _context.Adresses.FindAsync(id);
            if (adress != null)
            {
                _context.Adresses.Remove(adress);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdressExists(int id)
        {
            return _context.Adresses.Any(e => e.Id == id);
        }
    }
}
