using Microsoft.EntityFrameworkCore;
using ADE.Models;
using ADE.Servicios.Contrato;

namespace ADE.Servicios.Implementacion
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AdeContext _context;
        public UsuarioService(AdeContext context)
        {
            _context = context;
        }
        public async Task<Administrador> GetAdmin(string usuario, string clave)
        {
            Administrador admin = await _context.Administradores.Where(u => u.UsuarioAdmin == usuario && u.ContraAdmin == clave).FirstOrDefaultAsync();
            return admin;
        }

        public async Task<Usuario> GetUsuario(string correo, string clave)
        {
            Usuario usuario_encontrado = await _context.Usuarios.Where(u => u.CorreoUsuario == correo && u.ContraUsuario == clave).FirstOrDefaultAsync();
            return usuario_encontrado;
        
        }

        public async Task<Usuario> ValidarUsuario(string correo)
        {
            Usuario usuario_encontrado = await _context.Usuarios.Where(u => u.CorreoUsuario == correo).FirstOrDefaultAsync();
            return usuario_encontrado;

        }

        public async Task<Usuario> SaveUsuario(Usuario modelo)
        {
             _context.Usuarios.Add(modelo);
            await _context.SaveChangesAsync();
            return modelo;

        }
    }
}
