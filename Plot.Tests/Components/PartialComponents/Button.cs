using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using Xunit;

namespace PlaywrightTests;

public class ButtonTest : PageTest
{
    [Fact]
    public async Task HasButtonAndContent()
    {
        await Page.GotoAsync("http://localhost:8080/test/button");

        var button = Page.Locator("button");

        // Expect a button to be on the page.
        await Expect(button).ToBeVisibleAsync();

        // Expect the button to have "Hello World" as it's body text.
        await Expect(button).ToHaveTextAsync("Hello World");

        // Expect the button to have a data-id attribute with the value "hello".
        await Expect(button).ToHaveAttributeAsync("data-text", "hello");
    }

}