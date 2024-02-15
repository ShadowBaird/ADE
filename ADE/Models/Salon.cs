using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ADE.Models;

public partial class Salon
{

    public int IdSalon { get; set; }
    [Display(Name = "Nombre del Salon de Eventos")]

    public string NomSalon { get; set; } = null!;

    public virtual ICollection<Administrador> Administradores { get; set; } = new List<Administrador>();

    public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();

    public virtual ICollection<Prereserva> Prereservas { get; set; } = new List<Prereserva>();
}
