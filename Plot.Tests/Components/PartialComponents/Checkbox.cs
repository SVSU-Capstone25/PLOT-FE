using Microsoft.Playwright.Xunit;
using Xunit;

namespace PlaywrightTests;

public class CheckboxTest : PageTest
{
    [Fact]
    public async Task HasCheckboxAndContent()
    {
        // Go to test page.
        await Page.GotoAsync("http://localhost:8080/test/checkbox");

        // Expect the page to have a body visible.
        await Expect(Page.Locator("body")).ToBeVisibleAsync();

        var checkbox = Page.Locator("[data-test='checkbox']");

        // Ensure the checkbox is visible
        await Expect(checkbox).ToBeVisibleAsync();

        // Find the label
        var label = Page.Locator("label[for='testCheckbox']");
        
        // Extract label text
        var labelText = await label.TextContentAsync();

        // Assert that the label text is correct
        Assert.Equal("Test Checkbox", labelText);

        // Check the checkbox
        await checkbox.CheckAsync();
    }
}
