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
    /*
        This function will generate a full PDF file using the floorsetName in its output, and depending on user input, 
        provide extra information about the fixtures and/or allocations for the floorset
        
        Written By: Krzysztof Hejno
    */
    public static byte[] GenerateFloorsetPdf(string floorsetName, bool notes, bool allocations, List<FixtureInstance> fixtureInstances)
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
        double center = 90; //center page margin
        double right = 430; //right margin

        //current date printed
        gfx.DrawString($"Date: {DateTime.Now:MMMM dd, yyyy}", bodyFont, XBrushes.Black, new XPoint(right, top));
        top += 30;

        // Title
        gfx.DrawString("Floorset Fixture Allocations Printout", titleFont, XBrushes.Black, new XPoint(center, top));
        top += 30;

        //floorset Name
        string normalText = "For the floorset named: ";
        var normalTextWidth = gfx.MeasureString(normalText, subtitleFont).Width;

        gfx.DrawString(normalText, subtitleFont, XBrushes.Black, new XPoint(center + 75, top));

        gfx.DrawString(floorsetName, new XFont(subtitleFont.FontFamily.Name, subtitleFont.Size, XFontStyleEx.Bold), XBrushes.Black, new XPoint(center + 75 + normalTextWidth, top));
        top += 50;

        //choose whether or not to draw fixture information and/or notes
        if (!allocations && !notes)
        {
            gfx.DrawString("Fixture Information:", subtitleFont, XBrushes.Black, new XPoint(margin, top));
            top += 25;

            //This loop will go through each fixture and give information about each of them (notes, size, etc)
            var tf = new PdfSharp.Drawing.Layout.XTextFormatter(gfx);
            foreach (var fixture in fixtureInstances)
            {
                if (top > page.Height - margin - 100)
                {
                    //Add new page if space runs out
                    page = doc.AddPage();
                    var gfxLoop = XGraphics.FromPdfPage(page);
                    tf = new PdfSharp.Drawing.Layout.XTextFormatter(gfxLoop);
                    top = margin;
                }

                string fixtureLine = $"{fixture.TUID}. {fixture.NAME ?? "N/A"} " +
                        $"({fixture.HANGER_STACK switch { 1 => "Single Hung", 2 => "Double Hung", 3 => "Triple Hung", 4 => "Quadruple Hung", _ => "N/A" }}) " +
                        $"- {fixture.LENGTH * fixture.WIDTH * fixture.HANGER_STACK} LF \n";

                //Draw out the fixture information on each line
                var fixtureRect = new XRect(margin, top, page.Width - 2 * margin, 20);
                tf.DrawString(fixtureLine, bodyFont, XBrushes.Black, fixtureRect);

                top += 60; // Move down for next fixture
            }

        }
        else if (notes && !allocations)
        {
            gfx.DrawString("Fixture Information:", subtitleFont, XBrushes.Black, new XPoint(margin, top));
            top += 25;

            //This loop will go through each fixture and give information about each of them (notes, size, etc)
            var tf = new PdfSharp.Drawing.Layout.XTextFormatter(gfx);
            foreach (var fixture in fixtureInstances)
            {
                if (top > page.Height - margin - 100)
                {
                    //Add new page if space runs out
                    page = doc.AddPage();
                    var gfxLoop = XGraphics.FromPdfPage(page);
                    tf = new PdfSharp.Drawing.Layout.XTextFormatter(gfxLoop);
                    top = margin;
                }

                string fixtureLine = $"{fixture.TUID}. {fixture.NAME ?? "N/A"} "+
                        $"({fixture.HANGER_STACK switch { 1 => "Single Hung", 2 => "Double Hung", 3 => "Triple Hung", 4 => "Quadruple Hung", _ => "N/A" }})"+
                        $" - {fixture.LENGTH * fixture.WIDTH * fixture.HANGER_STACK} LF \n";
                string noteLine = string.IsNullOrWhiteSpace(fixture.NOTE) ? "None" : fixture.NOTE + "\n";

                //Draw out the fixture information on each line
                var fixtureRect = new XRect(margin, top, page.Width - 2 * margin, 20);
                tf.DrawString(fixtureLine, bodyFont, XBrushes.Black, fixtureRect);
                top += 25; //extra spacing between lines

                //note line
                var noteBoldFont = new XFont(bodyFont.FontFamily.Name, bodyFont.Size, XFontStyleEx.Bold);
                tf.DrawString("Note: ", noteBoldFont, XBrushes.Black, new XRect(margin, top, page.Width - 2 * margin, 20));

                //make the note section text bold
                double noteLabelWidth = gfx.MeasureString("Note: ", noteBoldFont).Width;

                tf.DrawString(noteLine, bodyFont, XBrushes.Black, new XRect(margin + noteLabelWidth, top, page.Width - 2 * margin - noteLabelWidth, 40));

                top += 60; // Move down for next fixture
            }
        }
        else if (!notes && allocations)
        {
            gfx.DrawString("Fixture Information:", subtitleFont, XBrushes.Black, new XPoint(margin, top));
            top += 25;

            //This loop will go through each fixture and give information about each of them (notes, size, etc)
            var tf = new PdfSharp.Drawing.Layout.XTextFormatter(gfx);
            foreach (var fixture in fixtureInstances)
            {
                if (top > page.Height - margin - 100)
                {
                    //Add new page if space runs out
                    page = doc.AddPage();
                    var gfxLoop = XGraphics.FromPdfPage(page);
                    tf = new PdfSharp.Drawing.Layout.XTextFormatter(gfxLoop);
                    top = margin;
                }

                string fixtureLine = $"{fixture.TUID}. {fixture.NAME ?? "N/A"} "+
                        $"({fixture.HANGER_STACK switch { 1 => "Single Hung", 2 => "Double Hung", 3 => "Triple Hung", 4 => "Quadruple Hung", _ => "N/A" }})"+
                        $" : {fixture.LENGTH * fixture.WIDTH * fixture.HANGER_STACK} LF of {fixture.SUPERCATEGORY_NAME ?? "N/A"} - {fixture.SUBCATEGORY ?? "N/A"} \n";

                //Draw out the fixture information on each line
                var fixtureRect = new XRect(margin, top, page.Width - 2 * margin, 20);
                tf.DrawString(fixtureLine, bodyFont, XBrushes.Black, fixtureRect);

                top += 60; // Move down for next fixture
            }
        }
        else if (notes && allocations)
        {
            gfx.DrawString("Fixture Information:", subtitleFont, XBrushes.Black, new XPoint(margin, top));
            top += 25;

            //This loop will go through each fixture and give information about each of them (notes, size, etc)
            var tf = new PdfSharp.Drawing.Layout.XTextFormatter(gfx);
            foreach (var fixture in fixtureInstances)
            {
                if (top > page.Height - margin - 100)
                {
                    //Add new page if space runs out
                    page = doc.AddPage();
                    var gfxLoop = XGraphics.FromPdfPage(page);
                    tf = new PdfSharp.Drawing.Layout.XTextFormatter(gfxLoop);
                    top = margin;
                }

                string fixtureLine = $"{fixture.TUID}. {fixture.NAME ?? "N/A"}"+
                $" ({fixture.HANGER_STACK switch { 1 => "Single Hung", 2 => "Double Hung", 3 => "Triple Hung", 4 => "Quadruple Hung", _ => "N/A" }})"+
                $" : {fixture.LENGTH * fixture.WIDTH * fixture.HANGER_STACK} LF of {fixture.SUPERCATEGORY_NAME ?? "N/A"} - {fixture.SUBCATEGORY ?? "N/A"} \n";
                string noteLine = string.IsNullOrWhiteSpace(fixture.NOTE) ? "None" : fixture.NOTE + "\n";

                //Draw out the fixture information on each line
                var fixtureRect = new XRect(margin, top, page.Width - 2 * margin, 20);
                tf.DrawString(fixtureLine, bodyFont, XBrushes.Black, fixtureRect);
                top += 25; //extra spacing between lines

                //note line
                var noteBoldFont = new XFont(bodyFont.FontFamily.Name, bodyFont.Size, XFontStyleEx.Bold);
                tf.DrawString("Note: ", noteBoldFont, XBrushes.Black, new XRect(margin, top, page.Width - 2 * margin, 20));

                //make the note section text bold
                double noteLabelWidth = gfx.MeasureString("Note: ", noteBoldFont).Width;
                tf.DrawString(noteLine, bodyFont, XBrushes.Black, new XRect(margin + noteLabelWidth, top, page.Width - 2 * margin - noteLabelWidth, 40));

                top += 60; // Move down for next fixture
            }
        }

        doc.Save(stream);
        return stream.ToArray();
    }
}
