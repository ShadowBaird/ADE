using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ADE.Models;

public partial class Evento
{
    public int IdEvento { get; set; }
    [Display(Name = "Fecha del evento")]

    public DateOnly FechaEvento { get; set; }
    [Display(Name = "Cantidad de personas del evento")]

    public int CantidadPersonasEvento { get; set; }
    [Display(Name = "Tipo de evento")]

    public string TipoEvento { get; set; } = null!;
    [Display(Name = "Responsable del evento")]

    public string NombreResponsableEvento { get; set; } = null!;
    [Display(Name = "Telefono del responsable")]

    public string TelefonoResponsableEvento { get; set; } = null!;
    [Display(Name = "Total a pagar")]

    public int TotalPagarEvento { get; set; }

    public int IdSalon { get; set; }
    [Display(Name = "Cantidad abonada")]

    public int AbonadoEvento { get; set; }
    [Display(Name = "ID de prereserva")]

    public int? IdPrereserva { get; set; }

    public virtual Prereserva? IdPrereservaNavigation { get; set; }

    public virtual Salon? IdSalonNavigation { get; set; } = null!;
}
