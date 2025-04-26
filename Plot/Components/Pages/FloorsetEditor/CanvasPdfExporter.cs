using System;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Snippets.Font;
using PdfSharp.Pdf.IO;

public static class CanvasPdfExporter
{
    public static byte[] GeneratePdfFromImage(byte[] imageBytes)
    {
        Console.WriteLine("In PDF creation!");
        //add a new document and page
        using var stream = new MemoryStream();
        using var doc = new PdfDocument();
        var page1 = doc.AddPage();
        using var gfx1 = XGraphics.FromPdfPage(page1);
        //rotate the page 90 degrees
        page1.Rotate = (page1.Rotate + 90) % 360;

        //get the image data stream
        using (var originalMs = new MemoryStream(imageBytes))
        using (var copyMs = new MemoryStream())
        {
            originalMs.CopyTo(copyMs);
            copyMs.Position = 0;

            var img = XImage.FromStream(copyMs);

            gfx1.TranslateTransform(page1.Width, 0);
            gfx1.RotateTransform(90);

            // Scale image to fit page
            double scale = Math.Min(page1.Height / img.PixelWidth * 72 / img.HorizontalResolution,
                                    page1.Width / img.PixelHeight * 72 / img.VerticalResolution);

            double width = img.PixelWidth * scale * img.HorizontalResolution / 72;
            double height = img.PixelHeight * scale * img.VerticalResolution / 72;
            Console.WriteLine($"Image size: {img.PixelWidth}x{img.PixelHeight}");
            Console.WriteLine($"Image DPI: {img.HorizontalResolution} x {img.VerticalResolution}");
            gfx1.DrawImage(img, 0, 0, width, height);

        }
        //add another page
        var page2 = doc.AddPage();
        using (var gfx2 = XGraphics.FromPdfPage(page2))
        {
            var font = new XFont("Segoe UI", 14, XFontStyleEx.Regular);
            gfx2.DrawString("Floorset text!", font, XBrushes.Black, new XPoint(50, 50));
        }
        doc.Save(stream);
        return stream.ToArray();
    }
}