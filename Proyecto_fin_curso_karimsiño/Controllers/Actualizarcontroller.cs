using Microsoft.AspNetCore.Mvc;
using Proyecto_fin_curso_karimsiño.Repository;
using Proyecto_fin_curso_karimsiño.Models;

namespace Proyecto_fin_curso_karimsiño.Controllers
{
    public class ActualizarController : Controller
    {
        private readonly RegisterRepository _registerRepo;

        public ActualizarController(RegisterRepository registerRepo)
        {
            _registerRepo = registerRepo;
        }

        // GET: /Actualizar/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var colaborador = await _registerRepo.ObtenerPorIdAsync(id);

            if (colaborador == null)
            {
                TempData["Error"] = "No se encontró el colaborador.";
                return RedirectToAction("ListaRegistro", "Listado");
            }

            return View(colaborador);
        }

        // POST: /Actualizar/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RegisterModel model)
        {
            // Solo validamos clave y rol, ignoramos errores de nombre/correo
            ModelState.Remove("nombre");
            ModelState.Remove("correo");

            if (!ModelState.IsValid)
                return View(model);

            await _registerRepo.ActualizarClaveRolAsync(model);

            TempData["Message"] = $"Credencial de {model.nombre} actualizada correctamente.";
            return RedirectToAction("ListaRegistro", "Listado");
        }
    }
}