using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using Xunit;

namespace PlaywrightTests
{
    public class ButtonTests : PageTest
    {
        // Test that all buttons are visible and display the correct text content.
        [Fact]
        public async Task HasButtonAndContent()
        {
            await Page.GotoAsync("http://localhost:8080/test/button");

            var button1 = Page.Locator("button:has-text('Test Text 1')");
            var button2 = Page.Locator("button:has-text('Test Text 2')");
            var button3 = Page.Locator("button:has-text('Test Text 3')");

            await Expect(button1).ToBeVisibleAsync();
            await Expect(button2).ToBeVisibleAsync();
            await Expect(button3).ToBeVisibleAsync();
        }

        // Test that each button has the correct variant (color) class.
        [Fact]
        public async Task ButtonVariantsAreCorrect()
        {
            await Page.GotoAsync("http://localhost:8080/test/button");

            var button1 = Page.Locator("button:has-text('Test Text 1')");
            var button2 = Page.Locator("button:has-text('Test Text 2')");
            var button3 = Page.Locator("button:has-text('Test Text 3')");

            // Retrieve the full class attribute and check for the expected variant substring.
            var class1 = await button1.GetAttributeAsync("class");
            var class2 = await button2.GetAttributeAsync("class
            var class3 = await button3.GetAttributeAsync("class");

            Assert.Contains("btn-primary", class1);
            Assert.Contains("btn-success", class2);
            Assert.Contains("btn-danger", class3);
        }

        // Test that text alignment classes are applied correctly
        [Fact]
        public async Task ButtonTextAlignmentIsCorrect()
        {
            await Page.GotoAsync("http://localhost:8080/test/button");

            var button1 = Page.Locator("button:has-text('Test Text 1')");
            var button2 = Page.Locator("button:has-text('Test Text 2')");
            var button3 = Page.Locator("button:has-text('Test Text 3')");

            var class1 = await button1.GetAttributeAsync("class");
            var class2 = await button2.GetAttributeAsync("class");
            var class3 = await button3.GetAttributeAsync("class");

            Assert.Contains("text-center", class1);
            Assert.Contains("text-end", class2);  // "right" maps to "text-end"
            Assert.Contains("text-start", class3); // "left" maps to "text-start"
        }

        // Test that the disabled button has the correct attributes
        [Fact]
        public async Task ButtonDisablesCorrectly()
        {
            await Page.GotoAsync("http://localhost:8080/test/button");

            var button3 = Page.Locator("button:has-text('Test Text 3')");

            // Check if 'disabled' attribute exists
            var disabledAttr = await button3.GetAttributeAsync("disabled");
            Assert.NotNull(disabledAttr);

            // Check if the class includes the correct disabled styling
            var class3 = await button3.GetAttributeAsync("class");
            Assert.Contains("disabled-btn", class3);

            // Check if cursor style is set to 'not-allowed'
            var style = await button3.GetAttributeAsync("style");
            Assert.Contains("cursor: not-allowed", style);
        }
    }
}
