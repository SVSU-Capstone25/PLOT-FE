using Microsoft.Playwright.Xunit;
using Xunit;

namespace PlaywrightTests;

public class DropdownTest : PageTest
{
    [Fact]
    public async Task DropdownOpensOnClickAndHasExpectedOptions()
    {
        // Go to the test page.
        await Page.GotoAsync("http://localhost:8080/test/dropdown");

        // Expect the page body to be visible.
        await Expect(Page.Locator("body")).ToBeVisibleAsync();

        // Locate the ellipses button (assuming it has a test id or class).
        var ellipsesButton = Page.Locator("button");

        // Expect the ellipses button to be visible.
        await Expect(ellipsesButton).ToBeVisibleAsync();

        // Click the ellipses button to open the dropdown.
        await ellipsesButton.ClickAsync();

        // Locate the dropdown menu.
        var dropdownMenu = Page.Locator(".dropdown-menu");

        // Expect the dropdown to be visible after clicking the ellipses.
        await Expect(dropdownMenu).ToBeVisibleAsync();

        // Check for expected options in the dropdown.
        await Expect(dropdownMenu.Locator("li:has-text('Youtube')")).ToBeVisibleAsync();
        await Expect(dropdownMenu.Locator("li:has-text('Google')")).ToBeVisibleAsync();
        await Expect(dropdownMenu.Locator("li:has-text('SVSU')")).ToBeVisibleAsync();
    }
}
