using Microsoft.Playwright.Xunit;
using Xunit;

namespace PlaywrightTests
{
    public class ButtonTests : PageTest
    {
        // Test if the button is visible and contains the correct text
        [Fact]
        public async Task ButtonComponentBehavesCorrectly()
        {
            // Navigate to the test page
            await Page.GotoAsync("http://localhost:8080/test/button");

            // Locate the button by its data-test attribute
            var button = Page.Locator("button[data-test='hello']");

            // Wait for the button to be visible and ensure it is displayed
            await Expect(button).ToBeVisibleAsync();

            // Ensure the button contains the correct text
            var buttonText = await button.InnerTextAsync();
            Assert.Equal("Hello World", buttonText);
        }

        // Test if the button supports the correct variant (e.g., primary, success, or danger)
        [Fact]
        public async Task ButtonSupportsVariant()
        {
            // Navigate to the test page
            await Page.GotoAsync("http://localhost:8080/test/button");

            // Locate the button by its data-test attribute
            var button = Page.Locator("button[data-test='hello']");

            // Wait for the button to be visible
            await Expect(button).ToBeVisibleAsync();

            // Check if the button has the correct CSS class based on the variant
            var buttonClass = await button.GetAttributeAsync("class");

            // Assert that the correct class (e.g., btn-primary) is present
            Assert.Contains("btn-primary", buttonClass); // Change based on what variant you're testing
        }

        // Test if the button is disabled and the cursor changes to 'not-allowed'
        [Fact]
        public async Task ButtonIsDisabled()
        {
            // Navigate to the test page
            await Page.GotoAsync("http://localhost:8080/test/button");

            // Locate the button by its data-test attribute
            var button = Page.Locator("button[data-test='hello']");

            // Wait for the button to be visible
            await Expect(button).ToBeVisibleAsync();

            // Verify that the button is disabled
            var isDisabled = await button.IsDisabledAsync();
            Assert.True(isDisabled);

            // Check that the cursor style is 'not-allowed' when the button is disabled
            var cursorStyle = await Page.EvaluateAsync<string>("window.getComputedStyle(document.querySelector('button[data-test=\"hello\"]')).cursor");
            Assert.Equal("not-allowed", cursorStyle);
        }

        // Test if the button's text alignment is correct (e.g., left, center, or right)
        [Fact]
        public async Task ButtonTextAlignment()
        {
            // Navigate to the test page
            await Page.GotoAsync("http://localhost:8080/test/button");

            // Locate the button by its data-test attribute
            var button = Page.Locator("button[data-test='hello']");

            // Wait for the button to be visible
            await Expect(button).ToBeVisibleAsync();

            // Check for correct text alignment (left, center, or right)
            var textAlign = await Page.EvaluateAsync<string>("window.getComputedStyle(document.querySelector('button[data-test=\"hello\"]')).textAlign");
            Assert.Equal("left", textAlign); // Change to "center" or "right" as needed
        }
    }
}
