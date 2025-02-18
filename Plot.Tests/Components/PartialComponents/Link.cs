using Microsoft.Playwright.Xunit;
using Xunit;

namespace PlaywrightTests;

public class LinkTest : PageTest
{
    [Fact]
    public async Task HasLinkAndCorrectAttributes()
    {
        // Go to test page
        await Page.GotoAsync("http://localhost:8080/test/link");

        // Expect the page to have a body visible
        await Expect(Page.Locator("body")).ToBeVisibleAsync();

        var link = Page.Locator("a.Link");

        // Expect a link to be on the page
        await Expect(link).ToBeVisibleAsync();

        // Expect the link to have some text
        await Expect(link).ToHaveTextAsync("This is a test link");

        // Expect the link to have a specific href attribute
        await Expect(link).ToHaveAttributeAsync("href", "https://www.svsu.edu/");
    }

    [Fact]
    public async Task LinkNavigatesToCorrectPage()
    {
        // Go to test page
        await Page.GotoAsync("http://localhost:8080/test/link");

        var link = Page.Locator("a.Link");
        string? href = await link.GetAttributeAsync("href");

        // Click the link and wait for navigation
        await link.ClickAsync();
        await Page.WaitForURLAsync(href!);

        // Expect the new page URL to match the link's href
        Assert.Equal(href, Page.Url);
    }
}
