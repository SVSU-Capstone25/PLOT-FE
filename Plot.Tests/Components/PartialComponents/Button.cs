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
        [Fact]
        public async Task ButtonVariantsAreCorrect()
        {
            // Navigate to the test page.
            await Page.GotoAsync("http://localhost:8080/test/button");

            // Locate each button by its text content.
            var button1 = Page.Locator("button:has-text('Test Text 1')");
            var button2 = Page.Locator("button:has-text('Test Text 2')");
            var button3 = Page.Locator("button:has-text('Test Text 3')");

            // Verify that each button's class includes the appropriate variant class.
            await Expect(button1).ToHaveClassAsync("btn-primary");
            await Expect(button2).ToHaveClassAsync("btn-success");
            await Expect(button3).ToHaveClassAsync("btn-danger");
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
            // Navigate to the test page.
            await Page.GotoAsync("http://localhost:8080/test/button");

            // For the first button, expecting center alignment.
            var button1 = Page.Locator("button:has-text('Test Text 1')");
            var style1 = await button1.GetAttributeAsync("style");
            Assert.Contains("center", style1 ?? string.Empty);

            // For the second button, expecting right alignment.
            var button2 = Page.Locator("button:has-text('Test Text 2')");
            var style2 = await button2.GetAttributeAsync("style");
            Assert.Contains("right", style2 ?? string.Empty);

            // For the third button, expecting left alignment.
            var button3 = Page.Locator("button:has-text('Test Text 3')");
            var style3 = await button3.GetAttributeAsync("style");
            Assert.Contains("left", style3 ?? string.Empty);
        }
    }
}