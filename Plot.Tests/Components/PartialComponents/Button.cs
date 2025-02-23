using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using Xunit;

namespace PlaywrightTests
{
    public class ButtonTests : PageTest
    {
        // Test to check that all buttons exist and display the correct content.
        [Fact]
        public async Task HasButtonAndContent()
        {
            // Navigate to the test page.
            await Page.GotoAsync("http://localhost:8080/test/button");

            // Locate the buttons by their text content.
            var button1 = Page.Locator("button:has-text('Test Text 1')");
            var button2 = Page.Locator("button:has-text('Test Text 2')");
            var button3 = Page.Locator("button:has-text('Test Text 3')");

            // Verify that the buttons are visible.
            await Expect(button1).ToBeVisibleAsync();
            await Expect(button2).ToBeVisibleAsync();
            await Expect(button3).ToBeVisibleAsync();
        }

        // Test to check if button variants (colors) are applied correctly.
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

            // Get the class attribute for each button
            var classList1 = await button1.GetAttributeAsync("class");
            var classList2 = await button2.GetAttributeAsync("class");
            var classList3 = await button3.GetAttributeAsync("class");

            // Check that each button has the correct variant class
            Assert.Contains("btn-primary", classList1?.Trim()); // Check if button1 has 'btn-primary'
            Assert.Contains("btn-success", classList2?.Trim()); // Check if button2 has 'btn-success'
            Assert.Contains("btn-danger", classList3?.Trim()); // Check if button3 has 'btn-danger'
        }


        // Test to check if the correct icons are displayed in the buttons.
        [Fact]
        public async Task ButtonIconsAreCorrect()
        {
            // Navigate to the test page.
            await Page.GotoAsync("http://localhost:8080/test/button");

            // Locate each button by its text content.
            var button1 = Page.Locator("button:has-text('Test Text 1')");
            var button2 = Page.Locator("button:has-text('Test Text 2')");
            var button3 = Page.Locator("button:has-text('Test Text 3')");

            // Verify that each button's icon is visible.
            await Expect(button1.Locator("i.fa-ellipsis")).ToBeVisibleAsync();
            await Expect(button2.Locator("i.fa-download")).ToBeVisibleAsync();
            await Expect(button3.Locator("i.fa-copy")).ToBeVisibleAsync();
        }

        // Test to check the text alignment in buttons by inspecting the inline style.
        [Fact]
        public async Task ButtonTextAlignmentIsCorrect()
        {
            // Navigate to the test page
            await Page.GotoAsync("http://localhost:8080/test/button");

            // Locate the first button and check its class for the correct alignment
            var button1 = Page.Locator("button:has-text('Test Text 1')");
            var classList1 = await button1.GetAttributeAsync("class");
            Assert.Contains("text-center", classList1?.Trim()); // Check if class contains 'text-center'

            // Locate the second button and check for right alignment (use 'text-end' for Bootstrap 5)
            var button2 = Page.Locator("button:has-text('Test Text 2')");
            var classList2 = await button2.GetAttributeAsync("class");
            Assert.Contains("text-end", classList2?.Trim()); // Check if class contains 'text-end'

            // Locate the third button and check for left alignment (use 'text-start' for left alignment)
            var button3 = Page.Locator("button:has-text('Test Text 3')");
            var classList3 = await button3.GetAttributeAsync("class");
            Assert.Contains("text-start", classList3?.Trim()); // Check if class contains 'text-start'
        }

    }
}