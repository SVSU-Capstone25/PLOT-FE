using Microsoft.Playwright.Xunit;
using Xunit;

namespace PlaywrightTests;

public class CardTest : PageTest
{
    [Fact]
    public async Task HasCardAndContent()
    {
        // Go to test page.
        await Page.GotoAsync("http://localhost:8080/test/card");

        // Expect the page to have a body visible
        await Expect(Page.Locator("body")).ToBeVisibleAsync();

        // Find the test cards on the page 
        var card1 = Page.Locator("#card1");
        var card2 = Page.Locator("#card2");

        // Expect a button to be on the page
        await Expect(card1).ToBeVisibleAsync();
        await Expect(card2).ToBeVisibleAsync();

        // Expect the card to have a title
        await Expect(card1.Locator("h3")).ToHaveTextAsync("Oct24 Layout");
        await Expect(card2.Locator("h3")).ToHaveTextAsync("Nov24 Layout");

        // Expect the card to have an image
        await Expect(card1.Locator("img")).ToBeVisibleAsync();
        await Expect(card2.Locator("img")).ToBeVisibleAsync();

        //Expect the card to have elipsis
        var card1Elipsis = Page.Locator("#card1 i");
        var card2Elipsis = Page.Locator("#card2 i");

        await Expect(card1Elipsis).ToBeVisibleAsync();
        await Expect(card2Elipsis).ToBeVisibleAsync();
    }
}