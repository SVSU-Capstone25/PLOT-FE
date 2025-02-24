using Microsoft.Playwright.Xunit;
using Xunit;

namespace PlaywrightTests;

public class NavBarTest : PageTest


{
    private const string BaseUrl = "http://localhost:8080";
    [Fact]
    public async Task HasHomePageAndContent()
    {
        // Go to test page.
        await Page.GotoAsync(BaseUrl);

        // Expect the page to have specific text
        await Expect(Page.Locator("p")).ToHaveTextAsync("You are on The Home page");
    }

    [Fact]
    public async Task HasNavBarAndContent()
    {
        // Go to test page.
        await Page.GotoAsync(BaseUrl);

        // Expect the page to have a body visibile.
        await Expect(Page.Locator("body")).ToBeVisibleAsync();

        var navBar = Page.Locator("#NavBar");

        // Expect a navigation bar to be on the page.
        await Expect(navBar).ToBeVisibleAsync();

        var topRow = Page.Locator(".top-row");

        // Expect the bar to have a specific color
        string backgroundColor = await topRow.EvaluateAsync<string>(@"element => {
        return getComputedStyle(element).backgroundColor;}");
        Assert.Equal("rgb(30, 166, 161)", backgroundColor);

    }

[Fact]
    public async Task HasStoresPageAndContent()
    {
        // Go to test page.
        await Page.GotoAsync($"{BaseUrl}/stores");

        // Expect the page to have a body visibile.
        await Expect(Page.Locator("body")).ToBeVisibleAsync();

        var navBar = Page.Locator("div#NavBar");

        // Expect a navigation bar to be on the page.
        await Expect(navBar).ToBeVisibleAsync();

        // Expect the page to have specific text   
        await Expect(Page.Locator("h1")).ToHaveTextAsync("Hello Store");

    }

    [Fact]
    public async Task HasUsersPageAndContent()
    {
        // Go to test page.
        await Page.GotoAsync($"{BaseUrl}/users");

        // Expect the page to have a body visibile.
        await Expect(Page.Locator("body")).ToBeVisibleAsync();

        var navBar = Page.Locator("div#NavBar");

        // Expect a navigation bar to be on the page.
        await Expect(navBar).ToBeVisibleAsync();

        // Expect the page to have specific text   
        await Expect(Page.Locator("h1")).ToHaveTextAsync("Hello User");

    }

    [Fact]
    public async Task NavBarNavigatesToStores()
    {
        // Go to test page.
        await Page.GotoAsync(BaseUrl);

        var storesLink = Page.Locator("a.nav-link:has-text('Stores')");

        //Expect the link to exist
        await Expect(storesLink).ToBeVisibleAsync();

        await storesLink.ClickAsync();

        //Expect the correct page to load
        await Expect(Page).ToHaveURLAsync($"{BaseUrl}/stores");
    }

    [Fact]
    public async Task NavBarNavigatesToUsers()
    {
         // Go to test page.
        await Page.GotoAsync(BaseUrl);

        var usersLink = Page.Locator("a.nav-link:has-text('Users')");

        //Expect the link to exist
        await Expect(usersLink).ToBeVisibleAsync();

        await usersLink.ClickAsync();

        //Expect the correct page to load
        await Expect(Page).ToHaveURLAsync($"{BaseUrl}/users");
    }

}