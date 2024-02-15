using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ADE.Models;

public partial class Prereserva
{
    public int IdPrereserva { get; set; }
    [Display(Name = "Salon")]

    public int IdSalon { get; set; }
    [Display(Name = "Usuario")]

    public int IdUsuario { get; set; }
    [Display(Name = "Fecha a separar")]

    public DateOnly FechaEventoPrereserva { get; set; }
    [Display(Name = "Estado prereserva")]

    public string EstadoPrereserva { get; set; } = null!;
    [Display(Name = "Cantidad de personas del evento")]

    public int CantidadPersonasPrereserva { get; set; }
    [Display(Name = "Tipo de evento")]

    public string TipoEventoPrereserva { get; set; } = null!;
    [Display(Name = "Fecha registro de reservacion")]

    public DateOnly FechaRegistroPrereserva { get; set; }
    [Display(Name = "Total a pagar")]

    public int? TotalPrereserva { get; set; }

    public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();

    public virtual Salon? IdSalonNavigation { get; set; } = null!;

    public virtual Usuario? IdUsuarioNavigation { get; set; } = null!;
}
