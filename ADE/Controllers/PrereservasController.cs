using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ADE.Models;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ADE.Servicios.Contrato;
using ADE.Recursos;
using ADE.Servicios.Implementacion;
using System.Numerics;
using System.Data;
using ClosedXML.Excel;
using Microsoft.Win32;


namespace ADE.Controllers
{
    [Authorize]

    public class PrereservasController : Controller
    {
        private readonly AdeContext _context;
        private readonly IValidateDatesService _datesService;


        public PrereservasController(AdeContext context, IValidateDatesService datesService)
        {
            _context = context;
            _datesService = datesService;

        }

        public ActionResult Reporte(string selected_nom_res, string fechaInicio, string fechaFin)
        {

            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[6] {new DataColumn("Nombre del cliente"),
                                            new DataColumn("Fecha separada"),
                                            new DataColumn("Estado"),
                                            new DataColumn("Tipo de evento"),
                                            new DataColumn("Cantidad de personas"),
                                            new DataColumn("Fecha de creacion de registro")});


            string dateTime = DateTime.Now.ToShortDateString();
            DateOnly date = DateOnly.Parse(dateTime);
            var adminSalonId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var adeContext = _context.Prereservas.Include(p => p.IdSalonNavigation).Include(p => p.IdUsuarioNavigation).Where(p => p.IdSalonNavigation.IdSalon.ToString() == adminSalonId && p.EstadoPrereserva != "Evento agendado" && p.FechaEventoPrereserva.CompareTo(date) >= 0);
            if (!string.IsNullOrEmpty(selected_nom_res))
            {
                adeContext = adeContext.Where(s => s.IdUsuarioNavigation.NombreUsuario.ToString().Equals(selected_nom_res));
            }

            if (!string.IsNullOrEmpty(fechaInicio))
            {
                DateOnly fecha = DateOnly.Parse(fechaInicio);
                adeContext = adeContext.Where(s => s.FechaEventoPrereserva >= fecha);

            }

            if (!string.IsNullOrEmpty(fechaFin))
            {
                DateOnly fecha = DateOnly.Parse(fechaFin);
                adeContext = adeContext.Where(s => s.FechaEventoPrereserva <= fecha);
            }
            foreach (var registro in adeContext)
            {
                dt.Rows.Add(registro.IdUsuarioNavigation.NombreUsuario, registro.FechaEventoPrereserva, registro.EstadoPrereserva, registro.TipoEventoPrereserva, registro.CantidadPersonasPrereserva, registro.FechaRegistroPrereserva);
            }
            ViewBag.selected_nom_res = selected_nom_res;
            ViewBag.fechaInicio = fechaInicio;
            ViewBag.fechaFin = fechaFin;

            var unique_nom_res = from s in adeContext
            group s by s.IdUsuarioNavigation.NombreUsuario into newGroup
            where newGroup.Key != null
            orderby newGroup.Key
            select new { nom_res = newGroup.Key };
            ViewBag.unique_nom_res = unique_nom_res.Select(m => new SelectListItem { Value = m.nom_res.ToString(), Text = m.nom_res.ToString() }).ToList();


            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte-preregistros.xlsx");
                }
            }

        }

        // GET: Prereservas
        public async Task<IActionResult> IndexUsuarios()
        {
            string dateTime = DateTime.Now.ToShortDateString();
            DateOnly date = DateOnly.Parse(dateTime);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var adeContext = _context.Prereservas.Include(p => p.IdSalonNavigation).Include(p => p.IdUsuarioNavigation).Where(p => p.IdUsuarioNavigation.IdUsuario.ToString() == userId && p.FechaEventoPrereserva.CompareTo(date) >= 0);
            return View(await adeContext.ToListAsync());
        }

        public async Task<IActionResult> IndexAdmin(string selected_nom_res, string fechaInicio, string fechaFin)
        {
            string dateTime = DateTime.Now.ToShortDateString();
            DateOnly date = DateOnly.Parse(dateTime);
            var adminSalonId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var adeContext = _context.Prereservas.Include(p => p.IdSalonNavigation).Include(p => p.IdUsuarioNavigation).Where(p => p.IdSalonNavigation.IdSalon.ToString() == adminSalonId && p.EstadoPrereserva != "Evento agendado" && p.FechaEventoPrereserva.CompareTo(date) >= 0);
            if (!string.IsNullOrEmpty(selected_nom_res))
            {
                adeContext = adeContext.Where(s => s.IdUsuarioNavigation.NombreUsuario.ToString().Equals(selected_nom_res));
            }

            if (!string.IsNullOrEmpty(fechaInicio))
            {
                DateOnly fecha = DateOnly.Parse(fechaInicio);
                adeContext = adeContext.Where(s => s.FechaEventoPrereserva >= fecha);

            }

            if (!string.IsNullOrEmpty(fechaFin))
            {
                DateOnly fecha = DateOnly.Parse(fechaFin);
                adeContext = adeContext.Where(s => s.FechaEventoPrereserva <= fecha);
            }

            ViewBag.selected_nom_res = selected_nom_res;
            ViewBag.fechaInicio = fechaInicio;
            ViewBag.fechaFin = fechaFin;

            var unique_nom_res = from s in adeContext
                                 group s by s.IdUsuarioNavigation.NombreUsuario into newGroup
                                 where newGroup.Key != null
                                 orderby newGroup.Key
                                 select new { nom_res = newGroup.Key };
            ViewBag.unique_nom_res = unique_nom_res.Select(m => new SelectListItem { Value = m.nom_res.ToString(), Text = m.nom_res.ToString() }).ToList();
            return View(await adeContext.ToListAsync());
        }

        // GET: Prereservas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prereserva = await _context.Prereservas
                .Include(p => p.IdSalonNavigation)
                .Include(p => p.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdPrereserva == id);
            if (prereserva == null)
            {
                return NotFound();
            }

            return View(prereserva);
        }

        // GET: Prereservas/Create
        public IActionResult Create()
        {
            ViewData["IdSalon"] = new SelectList(_context.Salones, "IdSalon", "IdSalon");
            ViewData["IdUsuario"] = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            return View();
        }

        // POST: Prereservas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPrereserva,IdSalon,FechaEventoPrereserva,CantidadPersonasPrereserva,TipoEventoPrereserva,FechaRegistroPrereserva")] Prereserva prereserva)
        {
            string date = DateTime.Now.ToShortDateString();
            prereserva.EstadoPrereserva = "Fecha separada";
            prereserva.IdUsuario = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            prereserva.FechaRegistroPrereserva = DateOnly.Parse(date);

            Prereserva validateDatePrereservas = await _datesService.ValidateDatePrereserva(prereserva.FechaEventoPrereserva, prereserva.IdSalon);
            Evento validateDateEventos = await _datesService.ValidateDateEvento(prereserva.FechaEventoPrereserva, prereserva.IdSalon);

            if (validateDateEventos == null && validateDatePrereservas == null)
            {
                _context.Add(prereserva);
                await _context.SaveChangesAsync();
                return RedirectToAction("IndexUsuarios");
            }
            ViewData["IdSalon"] = new SelectList(_context.Salones, "IdSalon", "IdSalon", prereserva.IdSalon);
            return View(prereserva);
        }

        // GET: Prereservas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prereserva = await _context.Prereservas.FindAsync(id);
            if (prereserva == null)
            {
                return NotFound();
            }
            ViewData["IdSalon"] = new SelectList(_context.Salones, "IdSalon", "IdSalon", prereserva.IdSalon);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", prereserva.IdUsuario);
            return View(prereserva);
        }

        // POST: Prereservas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPrereserva,IdSalon,IdUsuario,FechaEventoPrereserva,EstadoPrereserva,CantidadPersonasPrereserva,TipoEventoPrereserva,FechaRegistroPrereserva,TotalPrereserva")] Prereserva prereserva)
        {
            if (id != prereserva.IdPrereserva)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prereserva);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrereservaExists(prereserva.IdPrereserva))
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
            ViewData["IdSalon"] = new SelectList(_context.Salones, "IdSalon", "IdSalon", prereserva.IdSalon);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", prereserva.IdUsuario);
            return View(prereserva);
        }

        // GET: Prereservas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prereserva = await _context.Prereservas
                .Include(p => p.IdSalonNavigation)
                .Include(p => p.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdPrereserva == id);
            if (prereserva == null)
            {
                return NotFound();
            }

            return View(prereserva);
        }

        // POST: Prereservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prereserva = await _context.Prereservas.FindAsync(id);
            if (prereserva != null)
            {
                _context.Prereservas.Remove(prereserva);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrereservaExists(int id)
        {
            return _context.Prereservas.Any(e => e.IdPrereserva == id);
        }
    }
}
