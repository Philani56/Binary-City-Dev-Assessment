using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

namespace Project.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            var clients = await _context.Clients
                                        .Include(c => c.Contact)
                                        .OrderBy(c => c.Name) // Order by Name in ascending order
                                        .ToListAsync();
            return View(clients);
        }


        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include(c => c.Contact)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            var client = new Client
            {
                // Initially set ClientCode as null or empty
                ClientCode = string.Empty
            };

            ViewData["ContactId"] = new SelectList(_context.Contacts, "Id", "Id");
            return View(client);
        }

        // POST: Clients/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ContactId")] Client client)
        {
            //if (ModelState.IsValid)
            //{
                // Generate the ClientCode based on the client's name
                client.ClientCode = GenerateClientCode(client.Name);

            _context.Add(client);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Client created successfully!";
            return RedirectToAction(nameof(Create)); // Redirect back to the Create page
            //}
            ViewData["ContactId"] = new SelectList(_context.Contacts, "Id", "Id", client.ContactId); 
            return View(client);
        }





        private string GenerateClientCode(string clientName)
        {
            // Ensure the clientName has at least 3 characters
            string alphaPart;
            if (clientName.Length >= 3)
            {
                alphaPart = clientName.Substring(0, 3).ToUpper();
            }
            else
            {
                // If name has less than 3 characters, fill with 'A' to make it 3 characters
                alphaPart = clientName.ToUpper().PadRight(3, 'A');
            }

            // Retrieve the last client inserted with the same prefix
            var lastClientWithSamePrefix = _context.Clients
                .Where(c => c.ClientCode.StartsWith(alphaPart))
                .OrderByDescending(c => c.Id)
                .FirstOrDefault();

            // If no clients exist with the same prefix, start at 001
            int numericPart = lastClientWithSamePrefix == null
                ? 1
                : int.Parse(lastClientWithSamePrefix.ClientCode.Substring(3)) + 1;

            // Format the numeric part with leading zeros to ensure it is always 3 digits
            string clientCode = $"{alphaPart}{numericPart:D3}";

            return clientCode;
        }





        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            ViewData["ContactId"] = new SelectList(_context.Contacts, "Id", "Id", client.ContactId);
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ClientCode,ContactId")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            //}
            ViewData["ContactId"] = new SelectList(_context.Contacts, "Id", "Id", client.ContactId);
            return View(client);
        }


        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include(c => c.Contact)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clients == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Clients'  is null.");
            }
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
          return (_context.Clients?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
