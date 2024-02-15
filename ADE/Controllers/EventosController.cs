using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ADE.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ADE.Controllers
{
    [Authorize]

    public class EventosController : Controller
    {
        private readonly AdeContext _context;

        public EventosController(AdeContext context)
        {
            _context = context;
        }

        // GET: Eventos
        public async Task<IActionResult> IndexAdmin()
        {
            var adminSalonId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var adeContext = _context.Eventos.Include(p => p.IdSalonNavigation).Where(p => p.IdSalonNavigation.IdSalon.ToString() == adminSalonId);
            return View(await adeContext.ToListAsync());
        }

        public async Task<IActionResult> IndexUsuarios()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var adeContext = _context.Eventos.Include(p => p.IdSalonNavigation).Include(p => p.IdPrereservaNavigation).Where(p => p.IdPrereservaNavigation.IdUsuario.ToString() == userId);
            return View(await adeContext.ToListAsync());
        }

        // GET: Eventos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos
                .Include(e => e.IdSalonNavigation)
                .FirstOrDefaultAsync(m => m.IdEvento == id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        // GET: Eventos/Create
        public IActionResult Create()
        {
            ViewData["IdSalon"] = new SelectList(_context.Salones, "IdSalon", "NomSalon");
            return View();
        }
        public async Task<IActionResult> Registrar(int? id)
        {
       
            Prereserva prereserva = await _context.Prereservas.Include(p => p.IdSalonNavigation).Include(p => p.IdUsuarioNavigation).Where(p => p.IdPrereserva == id).FirstOrDefaultAsync();
            var evento = new Evento();
            evento.FechaEvento = prereserva.FechaEventoPrereserva;
            evento.CantidadPersonasEvento = prereserva.CantidadPersonasPrereserva;
            evento.TipoEvento = prereserva.TipoEventoPrereserva;
            evento.NombreResponsableEvento = prereserva.IdUsuarioNavigation.NombreUsuario;
            evento.TelefonoResponsableEvento = prereserva.IdUsuarioNavigation.TelUsuario;
            evento.TotalPagarEvento = 0;
            if(prereserva.TotalPrereserva != null)
            {
                evento.TotalPagarEvento = (int)prereserva.TotalPrereserva;
            }
            evento.IdSalon = prereserva.IdSalon;
            evento.IdPrereserva = prereserva.IdPrereserva;

            ViewData["IdSalon"] = new SelectList(_context.Salones, "IdSalon", "NomSalon");

            return View(evento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar([Bind("IdEvento,FechaEvento,CantidadPersonasEvento,TipoEvento,NombreResponsableEvento,TelefonoResponsableEvento,TotalPagarEvento,IdSalon, AbonadoEvento, IdPrereserva")] Evento evento)
        {
            Prereserva prereserva = await _context.Prereservas.Where(p => p.IdPrereserva == evento.IdPrereserva).FirstOrDefaultAsync();

            if (ModelState.IsValid)
            {
                _context.Add(evento);
                await _context.SaveChangesAsync();
                prereserva.EstadoPrereserva = "Evento agendado";
                _context.Update(prereserva);
                await _context.SaveChangesAsync();
                return RedirectToAction("IndexAdmin");
            }
            ViewData["IdSalon"] = new SelectList(_context.Salones, "IdSalon", "NomSalon");
            return View(evento);
        }

        // POST: Eventos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEvento,FechaEvento,CantidadPersonasEvento,TipoEvento,NombreResponsableEvento,TelefonoResponsableEvento,TotalPagarEvento,IdSalon, AbonadoEvento")] Evento evento)
        {

            if (ModelState.IsValid)
            {
                _context.Add(evento);
                await _context.SaveChangesAsync();
                return RedirectToAction("IndexAdmin");
            }
            ViewData["IdSalon"] = new SelectList(_context.Salones, "IdSalon", "NomSalon", evento.IdSalon);
            return View(evento);
        }

        // GET: Eventos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }
            ViewData["IdSalon"] = new SelectList(_context.Salones, "IdSalon", "IdSalon", evento.IdSalon);
            return View(evento);
        }

        // POST: Eventos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEvento,FechaEvento,CantidadPersonasEvento,TipoEvento,NombreResponsableEvento,TelefonoResponsableEvento,TotalPagarEvento,IdSalon, AbonadoEvento")] Evento evento)
        {
            if (id != evento.IdEvento)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(evento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventoExists(evento.IdEvento))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("IndexAdmin");
            }
            ViewData["IdSalon"] = new SelectList(_context.Salones, "IdSalon", "IdSalon", evento.IdSalon);
            return View(evento);
        }

        // GET: Eventos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos
                .Include(e => e.IdSalonNavigation)
                .FirstOrDefaultAsync(m => m.IdEvento == id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        // POST: Eventos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento != null)
            {
                _context.Eventos.Remove(evento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("IndexAdmin");
        }

        private bool EventoExists(int id)
        {
            return _context.Eventos.Any(e => e.IdEvento == id);
        }
    }
}
