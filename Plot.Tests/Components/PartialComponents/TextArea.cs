using Microsoft.Playwright.Xunit;
using Xunit;

namespace PlaywrightTests;

public class TextAreaTest : PageTest
{
    [Fact]
    public async Task TextAreaComponentBehavesCorrectly()
    {
        // Go to test page.
        await Page.GotoAsync("http://localhost:8080/test/text-area");

        // Expect the page to have a body visible.
        await Expect(Page.Locator("body")).ToBeVisibleAsync();

        // Locate text areas based on their labels
        var textAreaWithIcon = Page.Locator("#textBoxWithIcon");
        var textAreaWithoutIcon = Page.Locator("#textBoxWithoutIcon");


        await Page.WaitForTimeoutAsync(1000);
        // Expect the text areas to be visible
        await Expect(textAreaWithIcon).ToBeVisibleAsync();
        await Expect(textAreaWithoutIcon).ToBeVisibleAsync();

        // Expect correct placeholders
        // await Expect(textAreaWithIcon).ToHaveAttributeAsync("placeholder", "Text box with an icon here...");
        // await Expect(textAreaWithoutIcon).ToHaveAttributeAsync("placeholder", "Text box with no icon here....");

        // Expect the icon to be present for the first text area
        await Expect(Page.Locator(".fa-pen")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task TextAreaSavesOnClose()
    {
        await Page.GotoAsync("http://localhost:8080/test/text-area");

        var textArea = Page.Locator("label:has-text('Icon Header') + textarea");

        // Fill the text area
        await textArea.FillAsync("Updated text");

        // Simulate closing action (e.g., clicking outside)
        await Page.ClickAsync("body");

        // Expect the text area to retain its value after closing
        await Expect(textArea).ToHaveValueAsync("Updated text");
    }
}
