using Microsoft.AspNetCore.Mvc;
using ADE.Models;
using ADE.Recursos;
using ADE.Servicios.Contrato;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Numerics;
namespace ADE.Controllers
{
    public class InicioController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public InicioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public IActionResult Registrarse()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Registrarse(Usuario modelo)
        {
            Usuario usuario_encontrado = await _usuarioService.ValidarUsuario(modelo.CorreoUsuario);

            if (ModelState.IsValid && usuario_encontrado == null)
            {
                modelo.ContraUsuario = Utilidades.EncriptarClave(modelo.ContraUsuario);
                Usuario usuario_creado = await _usuarioService.SaveUsuario(modelo);
                if (usuario_creado.IdUsuario > 0)
                {
                    return RedirectToAction("IniciarSesion", "Inicio");
                }
            }

            if (usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "No se pudo crear el usuario";
            }
            else
            {
                ViewData["Mensaje"] = "Ya existe un usuario con el correo "+modelo.CorreoUsuario;
            }

                
            return View();

        }
        public IActionResult IniciarSesion()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> IniciarSesion(string usuario, string clave)
        {
            Usuario usuario_encontrado = await _usuarioService.GetUsuario(usuario, Utilidades.EncriptarClave(clave));
            Administrador admin_encontrado = await _usuarioService.GetAdmin(usuario, Utilidades.EncriptarClave(clave));

            if(usuario_encontrado == null && admin_encontrado == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias";
                return View();
            }
            if(usuario_encontrado !=null)
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, usuario_encontrado.NombreUsuario),
                    new Claim(ClaimTypes.NameIdentifier, usuario_encontrado.IdUsuario.ToString()),
                    new Claim(ClaimTypes.Role, "Usuario")

                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh=true,
                };
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    properties
                    );
            }
            else
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, admin_encontrado.UsuarioAdmin),
                    new Claim(ClaimTypes.NameIdentifier, admin_encontrado.IdSalon.ToString()),
                    new Claim(ClaimTypes.Role, "Administrador")

                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                };
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    properties
                    );
            }

            return RedirectToAction("Index", "Home");




        }
    }
}
