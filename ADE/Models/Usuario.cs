using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ADE.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }
    [Display(Name = "Correo")]

    public string CorreoUsuario { get; set; } = null!;
    [Display(Name = "Contraseña")]

    public string ContraUsuario { get; set; } = null!;
    [Display(Name = "Nombre completo")]

    public string NombreUsuario { get; set; } = null!;
    [Display(Name = "Telefono")]

    public string TelUsuario { get; set; } = null!;

    public virtual ICollection<Prereserva> Prereservas { get; set; } = new List<Prereserva>();
}
