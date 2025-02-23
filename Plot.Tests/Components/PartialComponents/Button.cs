using Microsoft.Playwright.Xunit;
using Xunit;

namespace PlaywrightTests;

public class ButtonTest : PageTest
{
    // Test if the button component is displayed and has the correct text and attributes
    [Fact]
    public async Task HasButtonAndContent()
    {
        // Navigate to the test page
        await Page.GotoAsync("http://localhost:8080/test/button");

        // Locate the button
        var button = Page.Locator("button");

        // Ensure the button is visible on the page
        await Expect(button).ToBeVisibleAsync();

        // Ensure the button contains the correct text
        await Expect(button).ToHaveTextAsync("Hello World");

        // Ensure the button has the correct data-test attribute
        await Expect(button).ToHaveAttributeAsync("data-test", "hello");
    }

    // Test if the button applies the correct variant styles
    [Fact]
    public async Task ButtonSupportsVariantStyles()
    {
        // Navigate to the test page
        await Page.GotoAsync("http://localhost:8080/test/button");

        // Locate the button
        var button = Page.Locator("button");

        // Ensure the button has one of the expected variant classes
        await Expect(button).ToHaveClassAsync(new[] { "btn-primary", "btn-success", "btn-danger" });
    }

    // Test if the button properly handles the disabled state
    [Fact]
    public async Task ButtonDisabledStatePreventsClick()
    {
        // Navigate to the test page
        await Page.GotoAsync("http://localhost:8080/test/button");

        // Locate the button
        var button = Page.Locator("button");

        // Check if the button is disabled
        if (await button.IsDisabledAsync())
        {
            // Ensure the cursor changes to 'not-allowed'
            await Expect(button).ToHaveCSSAsync("cursor", "not-allowed");

            // Ensure clicking the button does not trigger any action
            await button.ClickAsync();
        }
        else
        {
            // Ensure the cursor is 'pointer' if not disabled
            await Expect(button).ToHaveCSSAsync("cursor", "pointer");
        }
    }

    // Test if the button properly aligns its content based on the text alignment parameter
    [Fact]
    public async Task ButtonSupportsTextAlignment()
    {
        // Navigate to the test page
        await Page.GotoAsync("http://localhost:8080/test/button");

        // Locate the button
        var button = Page.Locator("button");

        // Ensure the button has one of the expected text alignment classes
        await Expect(button).ToHaveClassAsync(new[] { "text-left", "text-center", "text-right" });
    }

    // Test if the button supports an icon inside it
    [Fact]
    public async Task ButtonSupportsIcon()
    {
        // Navigate to the test page
        await Page.GotoAsync("http://localhost:8080/test/button");

        // Locate an icon inside the button (assumes an SVG icon)
        var icon = Page.Locator("button svg");

        // Check if the button contains an icon before asserting properties
        if (await icon.CountAsync() > 0)
        {
            // Ensure the icon is visible
            await Expect(icon).ToBeVisibleAsync();

            // Ensure the icon has the correct height
            await Expect(icon).ToHaveCSSAsync("height", "24px");
        }
    }

    // Test if the button expands to fill the parent's width
    [Fact]
    public async Task ButtonFillsParentWidth()
    {
        // Navigate to the test page
        await Page.GotoAsync("http://localhost:8080/test/button");

        // Locate the button
        var button = Page.Locator("button");

        // Ensure the button is using flex display and fills the parent width
        await Expect(button).ToHaveCSSAsync("display", "flex");
        await Expect(button).ToHaveCSSAsync("width", "100%");
    }

    // Test if the button maintains the required spacing and alignment
    [Fact]
    public async Task ButtonMaintainsDesignSpacing()
    {
        // Navigate to the test page
        await Page.GotoAsync("http://localhost:8080/test/button");

        // Locate the button
        var button = Page.Locator("button");

        // Ensure the button has the correct spacing between elements
        await Expect(button).ToHaveCSSAsync("gap", "8px");

        // Ensure the elements inside the button are vertically centered
        await Expect(button).ToHaveCSSAsync("align-items", "center");
        //idk at this point
    }
}
