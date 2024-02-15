using Microsoft.EntityFrameworkCore;
using ADE.Models;
using ADE.Servicios.Contrato;

namespace ADE.Servicios.Implementacion
{
    public class DateService : IValidateDatesService
    {
        private readonly AdeContext _context;
        public DateService(AdeContext context)
        {
            _context = context;
        }

        public async Task<Prereserva> ValidateDatePrereserva(DateOnly fecha, int idSalon)
        {
            Prereserva validateDatePrereservas = await _context.Prereservas.Where(p => p.FechaEventoPrereserva == fecha && p.IdSalon == idSalon).FirstOrDefaultAsync();
            return validateDatePrereservas;

        }

        public async Task<Evento> ValidateDateEvento(DateOnly fecha, int idSalon)
        {
            Evento validateDateEventos =await _context.Eventos.Where(p => p.FechaEvento == fecha && p.IdSalon == idSalon).FirstOrDefaultAsync();
            return validateDateEventos;
        }
    }
}
