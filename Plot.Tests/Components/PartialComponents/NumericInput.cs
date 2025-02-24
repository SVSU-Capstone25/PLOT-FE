using Microsoft.Playwright.Xunit;
using Xunit;

namespace PlaywrightTests;

public class NumericInputTest : PageTest
{
    [Fact]
    public async Task HasNumericInputAndContent()
    {
        // Go to test page.
        await Page.GotoAsync("http://localhost:8080/test/numeric-input");

        // Expect the page to have a body visible.
        await Expect(Page.Locator("body")).ToBeVisibleAsync();

        var numericInput = Page.Locator("input#number-input");

        // Expect a numeric input field to be on the page.
        await Expect(numericInput).ToBeVisibleAsync();

        // Expect the numeric input to have a placeholder "2ft".
        await Expect(numericInput).ToHaveAttributeAsync("placeholder", "2ft");

        // Expect the label to be "Dimensions".
        await Expect(Page.Locator("label[for='number-input']")).ToHaveTextAsync("Dimensions");

        // Expect the description text to be "Enter the Dimensions in feet".
        await Expect(Page.Locator("small")).ToHaveTextAsync("Enter the Dimensions in feet");
    }

    [Fact]
    public async Task RendersCorrectly_WithInitialParameters()
    {
        // Arrange & Act
        await Page.GotoAsync("http://localhost:8080/test/numeric-input");

        // Assert the rendered HTML matches the expected markup
        var markup = await Page.ContentAsync();
        Assert.Contains(@"<label for=""number-input"">Dimensions</label>", markup);
        Assert.Contains(@"<input type=""number"" class=""input-number"" id=""number-input"" placeholder=""2ft""", markup);
        Assert.Contains(@"<small class=""form-text text-muted error"">Enter the Dimensions in feet</small>", markup);
    }

    [Fact]
    public async Task TriggersValueChanged_WhenValidInputIsEntered()
    {
        await Page.GotoAsync("http://localhost:8080/test/numeric-input");

        var input = Page.Locator("input#number-input");

        // Ensure the input field is visible and enabled before interacting with it
        await Expect(input).ToBeVisibleAsync();
        await Expect(input).ToBeEnabledAsync();

        // Act - Simulate entering a valid number
        await input.FillAsync("42");

        // Ensure the input value is correctly set
        await Expect(input).ToHaveValueAsync("42");

        var description = Page.Locator("p");
        await Expect(description).ToHaveTextAsync("Number is: 42");
    }

    [Fact]
    public async Task ShowsErrorMessage_WhenInvalidInputIsEntered()
    {

        await Page.GotoAsync("http://localhost:8080/test/numeric-input");

        var input = Page.Locator("input#number-input");

        // Ensure the input field is visible and enabled before interacting with it
        await Expect(input).ToBeVisibleAsync();
        await Expect(input).ToBeEnabledAsync();

        // Act - Simulate entering a valid number
        await input.FillAsync("-10");

        // Ensure the input value is correctly set
        await Expect(input).ToHaveValueAsync("-10");

        var description = Page.Locator("small");
        await Expect(description).ToHaveTextAsync("Invalid Number!");
    }

    [Fact]
    public async Task ClearsError_WhenValidInputIsEntered_AfterInvalid()
    {
        await Page.GotoAsync("http://localhost:8080/test/numeric-input");

        var input = Page.Locator("input#number-input");

        // Ensure the input field is visible and enabled before interacting with it
        await Expect(input).ToBeVisibleAsync();
        await Expect(input).ToBeEnabledAsync();

        // Act - Simulate entering invalid number
        await input.FillAsync("-10");

        // Ensure the input value is correctly set
        await Expect(input).ToHaveValueAsync("-10");

        var description = Page.Locator("small");
        await Expect(description).ToHaveTextAsync("Invalid Number!");

        // Check to make sure it goes back to normal after adding valid number
        await input.FillAsync("5000000");

        await Expect(input).ToBeVisibleAsync();
        await Expect(input).ToBeEnabledAsync();

        // Ensure the input value is correctly set
        await Expect(input).ToHaveValueAsync("5000000");

        description = Page.Locator("p");
        await Expect(description).ToHaveTextAsync("Number is: 5000000");
    }
}
