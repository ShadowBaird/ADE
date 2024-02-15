using Microsoft.EntityFrameworkCore;
using ADE.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace ADE.Servicios.Contrato
{
    public interface IValidateDatesService
    {
        Task<Prereserva> ValidateDatePrereserva(DateOnly fecha, int idSalon);
        Task<Evento> ValidateDateEvento(DateOnly fecha, int idSalon);
    }
}
