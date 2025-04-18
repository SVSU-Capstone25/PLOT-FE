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
        Console.WriteLine("Here!!");
        using (var stream = new MemoryStream())
        {
            using (var doc = new PdfDocument())
            {
                var page = doc.AddPage();
                using (var gfx = XGraphics.FromPdfPage(page))
                {
                    using (var originalMs = new MemoryStream(imageBytes))
                    using (var copyMs = new MemoryStream())
                    {
                        originalMs.CopyTo(copyMs);
                        copyMs.Position = 0;

                        var img = XImage.FromStream(copyMs);

                        // Scale image to fit page
                        double scale = Math.Min(page.Width / img.PixelWidth * 72 / img.HorizontalResolution,
                                                page.Height / img.PixelHeight * 72 / img.VerticalResolution);

                        double width = img.PixelWidth * scale * img.HorizontalResolution / 72;
                        double height = img.PixelHeight * scale * img.VerticalResolution / 72;

                        gfx.DrawImage(img, 0, 0, width, height);
                    }
                }
                doc.Save(stream);
                return stream.ToArray();
            }
        }
    }
}