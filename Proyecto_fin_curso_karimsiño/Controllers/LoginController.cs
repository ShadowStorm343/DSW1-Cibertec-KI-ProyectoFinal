using Microsoft.AspNetCore.Mvc;
using Proyecto_fin_curso_karimsiño.Models;

namespace Proyecto_fin_curso_karimsiño.Controllers
{
    public class LoginController : Controller
    {
        // GET: /ACCOUNT/LOGIN
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /ACCOUNT/LOGIN
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                // Si el modelo no es válido, vuelve a mostrar el formulario con errores.
                return View(model);
            }

            // Aquí normalmente iría la validación contra la base de datos.
            // Por ahora simulamos un login exitoso si el usuario y contraseña no están vacíos.
            if (model.nombre == "karim" && model.clave == "1234")
            {
                TempData["Message"] = $"Bienvenido de nuevo, {model.nombre}.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
                return View(model);
            }
        }
    }
}

