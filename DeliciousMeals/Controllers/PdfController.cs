using Microsoft.AspNetCore.Mvc;
using DinkToPdf.Contracts;
using DinkToPdf;

namespace DeliciousMeals.Controllers
{
    public class PdfController : Controller
    {
        private readonly IConverter converter;

        public PdfController(IConverter converter)
        {
            this.converter = converter;
        }

        public IActionResult GeneratePdf()
        {
            string htmlContent = "<html><body> <h1>Hello PDF! </h1> </body></html>";

            var pdf = converter.Convert(new HtmlToPdfDocument()
            {
                GlobalSettings =
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4
                },

                Objects =
                {
                    new ObjectSettings()
                    {
                        HtmlContent = htmlContent,
                    }
                }
            });

            return File(pdf, "application/pdf", "output.pdf");
        }
    }
}
