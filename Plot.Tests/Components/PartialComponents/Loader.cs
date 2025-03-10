using Microsoft.Playwright.Xunit;
using Xunit;

namespace PlaywrightTests;

public class LoaderTest : PageTest
{
    [Fact]
    public async Task HasLoaderAndContent()
    {
        // Go to the loader test page
        await Page.GotoAsync("http://localhost:8080/test/loader");

        // Expect the page to have a body visible
        await Expect(Page.Locator("body")).ToBeVisibleAsync();

        //locate the 2 test loaders from the page 
        var loader1 = Page.Locator("#LoaderTest1");
        var loader2 = Page.Locator("#LoaderTest2");

        //locate the 2 spinners from the page
        var spinner1 = Page.Locator("#LoaderTest1 i");
        var spinner2 = Page.Locator("#LoaderTest2 i");

        // Expect a loader to be on the page
        await Expect(loader1).ToBeVisibleAsync();
        await Expect(loader2).ToBeVisibleAsync();

        //Expect the spinner to be on the page 
        await Expect(spinner1).ToBeVisibleAsync();
        await Expect(spinner2).ToBeVisibleAsync();

        //expect the loader to have the test text
        await Expect(loader1).ToContainTextAsync("Loading a component");
        await Expect(loader2).ToContainTextAsync("Loading a second component");
    }
}