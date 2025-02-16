using Microsoft.Playwright.Xunit;
using Xunit;

namespace PlaywrightTests;

public class OptionInputTest : PageTest
{
    [Fact]
    public async Task HasButtonAndContent()
    {
        // Go to test page.
        await Page.GotoAsync("http://localhost:8080/test/option-input");

        // Expect the page to have a body visibile
        await Expect(Page.Locator("body")).ToBeVisibleAsync();

        //option inputs 
        var optionInput1 = Page.Locator("#optionInput1");
        var optionInput2 = Page.Locator("#optionInput2");

        // Expect option inputs to be on the page
        await Expect(optionInput1).ToBeVisibleAsync();
        await Expect(optionInput2).ToBeVisibleAsync();

        // Expect the the options to have their labels 
        await Expect(optionInput1).ToContainTextAsync("Test Input");
        await Expect(optionInput2).ToContainTextAsync("Select Hanging");

        //option inputs drop downs
        var optionInput1Select = Page.Locator("#optionInput1 select");
        var optionInput2Select = Page.Locator("#optionInput2 select");
        
        // Expect option input dropdowns to be on the page
        await Expect(optionInput1Select).ToBeVisibleAsync();
        await Expect(optionInput2Select).ToBeVisibleAsync();

        //Expect the dropdowns to have the correct options
        await Expect(optionInput1Select).ToContainTextAsync("Hello" + "World");
        await Expect(optionInput2Select).ToContainTextAsync("Single Hung" + "Double Hung" + "Triple Hung" + "Quadruple Hung" );

        // Expect the dropdowns to have the correct values (initial values)
        await Expect(optionInput1Select).ToHaveValueAsync("hello");
        await Expect(optionInput2Select).ToHaveValueAsync("single");

        //Expect the dropdowns to have the correct values (after change)
        await optionInput1Select.SelectOptionAsync("world");
        await optionInput2Select.SelectOptionAsync("double");

        await Expect(optionInput1Select).ToHaveValueAsync("world");
        await Expect(optionInput2Select).ToHaveValueAsync("double");

        //option input 2 icon 
        var optionIcon = Page.Locator("#optionInput2 .fa-sign-hanging");

        // Expect the icon to be on the page
        await Expect(optionIcon).ToBeVisibleAsync();
    }

}