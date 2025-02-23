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

        [Fact]
        public async Task ButtonVariantsAreCorrect()
        {
            // Navigate to the test page
            await Page.GotoAsync("http://localhost:8080/test/button");

            // Locate each button by its text content
            var button1 = Page.Locator("button:has-text('Test Text 1')");
            var button2 = Page.Locator("button:has-text('Test Text 2')");
            var button3 = Page.Locator("button:has-text('Test Text 3')");

            // Correct class assertions
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

            // Verify that the button is disabled
            await Expect(button3).ToHaveAttributeAsync("disabled", "true");

            // Verify that the cursor is 'not-allowed' for the disabled button
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

            // Locate each button by its text content
            var button1 = Page.Locator("button:has-text('Test Text 1')");
            var button2 = Page.Locator("button:has-text('Test Text 2')");
            var button3 = Page.Locator("button:has-text('Test Text 3')");

            // Verify the text alignment for each button
            await Expect(button1).ToHaveClassAsync("text-center");
            await Expect(button2).ToHaveClassAsync("text-right");
            await Expect(button3).ToHaveClassAsync("text-left");
        }
    }
}