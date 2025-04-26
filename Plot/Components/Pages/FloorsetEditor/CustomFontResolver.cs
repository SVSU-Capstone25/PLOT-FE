using PdfSharp.Fonts;
using System.Net.Http;
using System.Reflection;

public class CustomFontResolver : IFontResolver
{
    private readonly byte[] fontData;

    public CustomFontResolver(byte[] fontBytes)
    {
        fontData = fontBytes;
    }

    public byte[] GetFont(string faceName)
    {
        return faceName switch
        {
            "SegoeUI" => fontData,
            _ => throw new InvalidOperationException($"Unknown face name: {faceName}")
        };
    }

    public FontResolverInfo? ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        if (familyName.Equals("Segoe UI", StringComparison.OrdinalIgnoreCase))
        {
            // We'll simulate italic if needed (PDFsharp only simulates italic anyway)
            return new FontResolverInfo("SegoeUI", mustSimulateBold: isBold, mustSimulateItalic: isItalic);
        }

        // Let it fall back to default
        return null;
    }
}