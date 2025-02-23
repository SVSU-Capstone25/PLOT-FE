using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using Xunit;

namespace PlaywrightTests
{
    public class ButtonTests : PageTest
    {
        // Test to check that all buttons exist and display the correct content
        [Fact]
        public async Task HasButtonAndContent()
        {
            // Navigate to the test page
            await Page.GotoAsync("http://localhost:8080/test/button");

            // Locate the buttons by their text content
            var button1 = Page.Locator("button:has-text('Test Text 1')");
            var button2 = Page.Locator("button:has-text('Test Text 2')");
            var button3 = Page.Locator("button:has-text('Test Text 3')");

            // Verify that the buttons are visible
            await Expect(button1).ToBeVisibleAsync();
            await Expect(button2).ToBeVisibleAsync();
            await Expect(button3).ToBeVisibleAsync();
        }

        // Test to check if button variants are applied correctly
        [Fact]
        public async Task ButtonVariantsAreCorrect()
        {
            // Navigate to the test page
            await Page.GotoAsync("http://localhost:8080/test/button");

            // Locate each button by its text content
            var button1 = Page.Locator("button:has-text('Test Text 1')");
            var button2 = Page.Locator("button:has-text('Test Text 2')");
            var button3 = Page.Locator("button:has-text('Test Text 3')");

            // Check if buttons have the correct classes for each variant
            await Expect(button1).ToHaveClassAsync("btn-primary");
            await Expect(button2).ToHaveClassAsync("btn-success");
            await Expect(button3).ToHaveClassAsync("btn-danger");
        }

        // Test to check if buttons are disabled correctly
        [Fact]
        public async Task ButtonDisablesCorrectly()
        {
            // Navigate to the test page
            await Page.GotoAsync("http://localhost:8080/test/button");

            // Locate the third button (disabled button)
            var button3 = Page.Locator("button:has-text('Test Text 3')");

            // Verify that the button has the 'disabled' attribute
            var isDisabled = await button3.GetAttributeAsync("disabled");
            Assert.NotNull(isDisabled); // Ensure the disabled attribute is present

            // Optionally verify cursor style
            var cursorStyle = await button3.GetAttributeAsync("style");
            Assert.Contains("cursor: not-allowed", cursorStyle);
        }



        // Test to check the icons in buttons
        [Fact]
        public async Task ButtonIconsAreCorrect()
        {
            // Navigate to the test page
            await Page.GotoAsync("http://localhost:8080/test/button");

            // Locate each button by its text content
            var button1 = Page.Locator("button:has-text('Test Text 1')");
            var button2 = Page.Locator("button:has-text('Test Text 2')");
            var button3 = Page.Locator("button:has-text('Test Text 3')");

            // Check if the icon is displayed for each button
            await Expect(button1.Locator("i.fa-ellipsis")).ToBeVisibleAsync();
            await Expect(button2.Locator("i.fa-download")).ToBeVisibleAsync();
            await Expect(button3.Locator("i.fa-copy")).ToBeVisibleAsync();
        }

        // Test to check the text alignment in buttons
        [Fact]
        public async Task ButtonTextAlignmentIsCorrect()
        {
            // Navigate to the test page
            await Page.GotoAsync("http://localhost:8080/test/button");

            // Locate the first button and check its class for the correct alignment
            var button1 = Page.Locator("button:has-text('Test Text 1')");
            var classList1 = await button1.GetAttributeAsync("class");
            Assert.Contains("text-center", classList1?.Trim()); // Check if class contains 'text-center'

            // Locate the second button and check for right alignment
            var button2 = Page.Locator("button:has-text('Test Text 2')");
            var classList2 = await button2.GetAttributeAsync("class");
            Assert.Contains("text-right", classList2?.Trim()); // Check if class contains 'text-right'

            // Locate the third button and check for left alignment
            var button3 = Page.Locator("button:has-text('Test Text 3')");
            var classList3 = await button3.GetAttributeAsync("class");
            Assert.Contains("text-left", classList3?.Trim()); // Check if class contains 'text-left'
        }

    }
}
