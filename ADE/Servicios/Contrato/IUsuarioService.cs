using Microsoft.EntityFrameworkCore;
using ADE.Models;
namespace ADE.Servicios.Contrato
{
    public interface IUsuarioService
    {
        Task<Usuario> GetUsuario(string correo, string clave);
        Task<Usuario> ValidarUsuario(string correo);

        Task<Usuario> SaveUsuario(Usuario modelo);

        Task<Administrador> GetAdmin(string usuario, string clave);
    }
}
