using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ADE.Models;

public partial class Administrador
{
    public int IdAdmin { get; set; }
    [Display(Name = "Usuario")]

    public string UsuarioAdmin { get; set; } = null!;
    [Display(Name = "Contraseña")]

    public string ContraAdmin { get; set; } = null!;

    public int IdSalon { get; set; }

    public virtual Salon? IdSalonNavigation { get; set; } = null!;
}
