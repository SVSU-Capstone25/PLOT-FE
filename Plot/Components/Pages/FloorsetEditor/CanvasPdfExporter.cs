using System;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Snippets.Font;
using PdfSharp.Pdf.IO;
using Plot.Data.Models.Fixtures;
using Plot.Data.Models.Allocations;
using Plot.Data.Models.Floorsets;
using System.Threading.Tasks;
using Plot.Services;


public static class CanvasPdfExporter
{

    private static FixtureInstance? fixtureInstance;
    public static async Task<byte[]> GenerateFloorsetPdf(string floorsetName, bool notes, bool allocations,List<FixtureInstance> fixtureInstance)
    {
        using var stream = new MemoryStream();
        using var doc = new PdfDocument();
        var page = doc.AddPage();
        using var gfx = XGraphics.FromPdfPage(page);

        

        //fonts 
        var titleFont = new XFont("Segoe UI", 24, XFontStyleEx.Bold);
        var subtitleFont = new XFont("Segoe UI", 16, XFontStyleEx.Regular);
        var bodyFont = new XFont("Segoe UI", 12, XFontStyleEx.Regular);

        double margin = 50; // Left margin
        double top = 50;    // Starting height
        double center = 210; //center page margin
        double right = 430; //right margin

        // Title
        gfx.DrawString("Floorset Fixture Allocations Printout", titleFont, XBrushes.Black, new XPoint(center-100, top));
        top += 40;

        // Date (current date printed)
        gfx.DrawString($"Date: {DateTime.Now:MMMM dd, yyyy}", bodyFont, XBrushes.Black, new XPoint(right, top));
        top += 30;

        // Floorset Name
        gfx.DrawString($"For the floorset named: {floorsetName}", subtitleFont, XBrushes.Black, new XPoint(margin, top));
        top += 50;

        // Notes (optional)
        if (notes)
        {

            top += 20;
            gfx.DrawString("Notes:", subtitleFont, XBrushes.Black, new XPoint(margin, top));
            top += 25;

            // Handle long notes (wrap lines)
            var layoutRectangle = new XRect(margin, top, page.Width - 2 * margin, page.Height - top - margin);
            var tf = new PdfSharp.Drawing.Layout.XTextFormatter(gfx)
            {
                Alignment = PdfSharp.Drawing.Layout.XParagraphAlignment.Left
            };
            tf.DrawString("notes go here", bodyFont, XBrushes.Black, layoutRectangle);
        }

        if(allocations){

        }

        doc.Save(stream);
        return stream.ToArray();
    }
    // public static byte[] GeneratePdfFromImage(byte[] imageBytes)
    // {
    //     Console.WriteLine("Here!!");
    //     //add a new document and page
    //     using var stream = new MemoryStream();
    //     using var doc = new PdfDocument();
    //     var page = doc.AddPage();
    //     using var gfx = XGraphics.FromPdfPage(page);
    //     //rotate the page 90 degrees
    //     page.Rotate = (page.Rotate + 90) % 360;

    //     //get the image data stream
    //     using (var originalMs = new MemoryStream(imageBytes))
    //     using (var copyMs = new MemoryStream())
    //     {
    //         originalMs.CopyTo(copyMs);
    //         copyMs.Position = 0;

    //         var img = XImage.FromStream(copyMs);

    //         gfx.TranslateTransform(page.Width, 0);
    //         gfx.RotateTransform(90);

    //         // Scale image to fit page
    //         double scale = Math.Min(page.Height / img.PixelWidth * 72 / img.HorizontalResolution,
    //                                 page.Width / img.PixelHeight * 72 / img.VerticalResolution);

    //         double width = img.PixelWidth * scale * img.HorizontalResolution / 72;
    //         double height = img.PixelHeight * scale * img.VerticalResolution / 72;
    //         Console.WriteLine($"Image size: {img.PixelWidth}x{img.PixelHeight}");
    //         Console.WriteLine($"Image DPI: {img.HorizontalResolution} x {img.VerticalResolution}");
    //         gfx.DrawImage(img, 0, 0, width, height);

    //         doc.Save(stream);
    //         return stream.ToArray();
    //     }
    // }
}
