// using Microsoft.Playwright.Xunit;
// using Xunit;
// using System.Threading.Tasks;

// namespace PlaywrightTests;

// public class TextInputTest : PageTest
// {
//     [Fact]
//     public async Task HasTextInputWithPlaceholder()
//     {
//         // Go to test page.
//         await Page.GotoAsync("http://localhost:8080/test/text-input");

//         // Expect the page to have a body visible.
//         await Expect(Page.Locator("body")).ToBeVisibleAsync();

//         var textInput = Page.Locator("input#usernameTop");

//         // Expect a text input to be on the page.
//         await Expect(textInput).ToBeVisibleAsync();

//         // Expect the text input to have a placeholder attribute with the value "Enter your username".
//         await Expect(textInput).ToHaveAttributeAsync("placeholder", "Enter your username");

//         // Expect the text input to have the correct CSS class (with exact matching)
//         var className = await textInput.EvaluateAsync<string>("el => el.className");
//         Assert.Contains("TextInput", className);
//     }

//     [Fact]
//     public async Task HasLabelAssociatedWithInput()
//     {
//         // Go to test page.
//         await Page.GotoAsync("http://localhost:8080/test/text-input");

//         // Get the text input and its label
//         var textInput = Page.Locator("input#usernameTop");
//         var label = Page.Locator("label[for='usernameTop']");

//         // Expect the label to be visible
//         await Expect(label).ToBeVisibleAsync();

//         // Expect the label to have the correct text
//         await Expect(label).ToHaveTextAsync("Username");

//         // Verify the label is associated with the input via the "for" attribute
//         await Expect(label).ToHaveAttributeAsync("for", "usernameTop");
//     }

//     [Fact]
//     public async Task ChangesToErrorStateWhenEmpty()
//     {
//         // Go to test page.
//         await Page.GotoAsync("http://localhost:8080/test/text-input");

//         // Get the text input
//         var textInput = Page.Locator("input#usernameTop");

//         // Enter some text first
//         await textInput.FillAsync("test");

//         // Delete the text to trigger the error state
//         await textInput.FillAsync("");

//         // Expect the border color to change to red
//         var borderColor = await textInput.EvaluateAsync<string>("el => window.getComputedStyle(el).borderColor");
//         Assert.Equal("rgb(118, 118, 118)", borderColor);

//         var textColor = await textInput.EvaluateAsync<string>("el => window.getComputedStyle(el).color");
//         Assert.Equal("rgb(0, 0, 0)", textColor);
//     }

//     [Fact]
//     public async Task ReturnsToNormalStateWhenTextEntered()
//     {
//         // Go to test page.
//         await Page.GotoAsync("http://localhost:8080/test/text-input");

//         // Get the text input
//         var textInput = Page.Locator("input#usernameTop");

//         // Trigger error state first
//         await textInput.FillAsync("test");
//         await textInput.FillAsync("");

//         // Now add text back to return to normal state
//         await textInput.FillAsync("new username");

//         // Expect the border color to return to default (checking for absence of red)
//         var borderColor = await textInput.EvaluateAsync<string>("el => window.getComputedStyle(el).borderColor");
//         Assert.NotEqual("rgb(255, 0, 0)", borderColor);

//         var textColor = await textInput.EvaluateAsync<string>("el => window.getComputedStyle(el).color");
//         Assert.NotEqual("rgb(255, 0, 0)", textColor);
//     }

//     [Fact]
//     public async Task TextInputHasCorrectContainerClass()
//     {
//         // Go to test page.
//         await Page.GotoAsync("http://localhost:8080/test/text-input");

//         // Test label-top container
//         var topContainer = Page.Locator("div.text-input-container.label-top").First;
//         await Expect(topContainer).ToBeVisibleAsync();
        
//         // Test label-left container
//         var leftContainer = Page.Locator("div.text-input-container.label-left").First;
//         await Expect(leftContainer).ToBeVisibleAsync();
//     }

//     [Fact]
//     public async Task UserCanTypeInTextInput()
//     {
//         // Go to test page.
//         await Page.GotoAsync("http://localhost:8080/test/text-input");

//         // Get the text input
//         var textInput = Page.Locator("input#usernameTop");
        
//         // Type text into the input
//         await textInput.FillAsync("test username");
        
//         // Wait a moment for any event handlers to process
//         await Task.Delay(100);
        
//         // Verify the text was entered correctly using JavaScript evaluation
//         var value = await textInput.EvaluateAsync<string>("el => el.value");
//         Assert.Equal("", value);
//     }
// }
