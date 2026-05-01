using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Proyecto_fin_curso_karimsiño.Repository;

namespace Proyecto_fin_curso_karimsiño.Controllers
{
    public class ReportesController : Controller
    {
        private readonly RegisterRepository _registerRepo;

        public ReportesController(RegisterRepository registerRepo)
        {
            _registerRepo = registerRepo;
        }

        // GET: /Reportes/Index → descarga directa del Excel
        public async Task<IActionResult> Index()
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

            // Estilo encabezados
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

            // Formato
            ws.Columns().AdjustToContents();
            ws.RangeUsed()!.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.RangeUsed()!.Style.Border.InsideBorder = XLBorderStyleValues.Hair;

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            string fileName = $"Colaboradores_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }
    }
}