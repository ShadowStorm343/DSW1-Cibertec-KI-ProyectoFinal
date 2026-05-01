using Microsoft.AspNetCore.Mvc;
using Proyecto_fin_curso_karimsiño.Repository;
using ClosedXML.Excel;


namespace Proyecto_fin_curso_karimsiño.Controllers
{
    public class ListadoController : Controller
    {
        private readonly RegisterRepository _registerRepo;
        private const int PorPagina = 6;

        public ListadoController(RegisterRepository registerRepo)
        {
            _registerRepo = registerRepo;
        }

        // GET: /Listado/ListaRegistro?pagina=1
        [HttpGet]
        public async Task<IActionResult> ListaRegistro(int pagina = 1)
        {
            if (pagina < 1) pagina = 1;

            int total = await _registerRepo.ContarTotalAsync();
            int totalPaginas = (int)Math.Ceiling((double)total / PorPagina);

            if (pagina > totalPaginas && totalPaginas > 0)
                pagina = totalPaginas;

            var lista = await _registerRepo.ObtenerPaginadoAsync(pagina, PorPagina);

            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = totalPaginas;
            ViewBag.TotalRegistros = total;

            return View(lista);
        }

        // POST: /Listado/Eliminar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id, int pagina = 1)
        {
            var colaborador = await _registerRepo.ObtenerPorIdAsync(id);

            if (colaborador != null)
            {
                await _registerRepo.EliminarAsync(id);
                TempData["Message"] = $"El colaborador '{colaborador.nombre}' fue eliminado correctamente.";
            }

            // Si después de eliminar la página actual queda vacía, retroceder una
            int total = await _registerRepo.ContarTotalAsync();
            int totalPaginas = (int)Math.Ceiling((double)total / PorPagina);
            if (pagina > totalPaginas && totalPaginas > 0)
                pagina = totalPaginas;

            return RedirectToAction("ListaRegistro", new { pagina });
        }

        // GET: /Listado/DescargarExcel
        [HttpGet]
        public async Task<IActionResult> DescargarExcel()
        {
            var lista = await _registerRepo.ObtenerTodosAsync();

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Colaboradores");

            // Encabezados
            ws.Cell(1, 1).Value = "ID";
            ws.Cell(1, 2).Value = "Nombre";
            ws.Cell(1, 3).Value = "Correo";
            ws.Cell(1, 4).Value = "Rol";
            ws.Cell(1, 5).Value = "Contraseña";

            // Estilo de encabezados
            var headerRow = ws.Range("A1:E1");
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Fill.BackgroundColor = XLColor.FromHtml("#7C3AED");
            headerRow.Style.Font.FontColor = XLColor.White;
            headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Datos
            int fila = 2;
            foreach (var item in lista)
            {
                ws.Cell(fila, 1).Value = item.Id;
                ws.Cell(fila, 2).Value = item.nombre;
                ws.Cell(fila, 3).Value = item.correo;
                ws.Cell(fila, 4).Value = item.rol ?? "Sin rol";
                ws.Cell(fila, 5).Value = item.clave;
                fila++;
            }

            // Ajustar ancho de columnas automáticamente
            ws.Columns().AdjustToContents();

            // Borde a toda la tabla
            ws.RangeUsed()!.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.RangeUsed()!.Style.Border.InsideBorder = XLBorderStyleValues.Hair;

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            string fileName = $"Colaboradores_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }
    }
}