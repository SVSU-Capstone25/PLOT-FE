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

        var textAreaWithIcon = Page.Locator("textarea#textBoxWithIcon");
        var textAreaWithoutIcon = Page.Locator("textarea#textBoxWithoutIcon");

        // Expect the text areas to be visible on the page.
        await Expect(textAreaWithIcon).ToBeVisibleAsync();
        await Expect(textAreaWithoutIcon).ToBeVisibleAsync();

        // Expect the first text area to have an icon and the correct attributes.
        await Expect(textAreaWithIcon).ToHaveAttributeAsync("placeholder", "Text box with an icon here...");
        await Expect(Page.Locator(".fa-pen")).ToBeVisibleAsync();

        // Expect the second text area to have the correct attributes and no icon.
        await Expect(textAreaWithoutIcon).ToHaveAttributeAsync("placeholder", "Text box with no icon here....");
    }

    [Fact]
    public async Task TextAreaSavesOnClose()
    {
        await Page.GotoAsync("http://localhost:8080/test/text-area");

        var textArea = Page.Locator("textarea#textBoxWithIcon");
        await textArea.FillAsync("Updated text");

        // Simulate closing action (e.g., clicking outside or a save button)
        await Page.ClickAsync("body");

        // Expect the text area to retain its value after closing.
        await Expect(textArea).ToHaveValueAsync("Updated text");
    }
}
