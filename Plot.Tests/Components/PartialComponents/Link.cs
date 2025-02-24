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

        var link = Page.Locator("a.test#link");

        // Expect a link to be on the page
        await Expect(link).ToBeVisibleAsync();

        // Expect the link to have some text
        await Expect(link).ToHaveTextAsync("Hello");

        // Expect the link to have a specific href attribute
        await Expect(link).ToHaveAttributeAsync("href", "https://www.svsu.edu/");

        // Verify color using EvaluateAsync for computed styles
        var color = await link.EvaluateAsync<string>("el => getComputedStyle(el).color");
        Assert.Equal("rgb(85, 181, 177)", color);
    }

    [Fact]
    public async Task LinkNavigatesToCorrectPage()
    {
        // Go to test page
        await Page.GotoAsync("http://localhost:8080/test/link");

        var link = Page.Locator("a.test#link");
        string? href = await link.GetAttributeAsync("href");

        // Click the link and wait for navigation
        await link.ClickAsync();
        await Page.WaitForURLAsync(href!);

        // Expect the new page URL to match the link's href
        Assert.Equal(href, Page.Url);
    }
}
