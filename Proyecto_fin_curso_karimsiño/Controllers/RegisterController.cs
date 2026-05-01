using Microsoft.AspNetCore.Mvc;
using Proyecto_fin_curso_karimsiño.Models;
using Proyecto_fin_curso_karimsiño.Repository;

namespace Proyecto_fin_curso_karimsiño.Controllers
{
    public class RegisterController : Controller
    {
        private readonly RegisterRepository registerRepo;

        // GET: /ACCOUNT/REGISTER
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        // POST: /ACCOUNT/REGISTER
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {  // Si el modelo no es válido,
               // vuelve a mostrar el formulario con los errores de validación.

                return View(model);
            }

            // Simulación de registro exitoso.
            TempData["Message"] = $"Usuario {model.nombre} registrado correctamente. Bienvenido.";
            return RedirectToAction("Index", "Home");
        }

        public RegisterController(RegisterRepository registerRepo)
        {
            this.registerRepo = registerRepo;
        }

        [HttpGet]
        public IActionResult Create() // Acción para mostrar el formulario de registro
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterModel registerModel) // Acción para procesar el formulario de registro
        {
            if (!ModelState.IsValid)
            {
                return View(registerModel); // Si el moelo no es válido, vuelve a mostrar el formulario con los errores de validación.
            }
            await registerRepo.AgregarRegisterAsync(registerModel); // Agrega al nuevo usuario a la base de datos.
            return RedirectToAction("Index", "Home"); // Redirige a la página principal después del registro exitoso.

        }
    }
}


//AHORA VAMOS A HACER LO MISMO PERO PARA EL LOGIN UWU
