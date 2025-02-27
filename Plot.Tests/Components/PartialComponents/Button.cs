using Microsoft.Playwright.Xunit;
using Xunit;

namespace PlaywrightTests
{

    public class TextAreaTest : PageTest
    {
        // Test if the text area component is displayed and has the expected label and icon
        [Fact]
        public async Task TextAreaComponentBehavesCorrectly()
        {
            // Navigate to the test page
            await Page.GotoAsync("http://localhost:8080/test/text-area");

            // Locate the text areas by ID
            var textAreaWithIcon = Page.Locator("#textBoxWithIcon");
            var textAreaWithoutIcon = Page.Locator("#textBoxWithoutIcon");

            // Wait for them to be visible and ensure both are displayed
            await Expect(textAreaWithIcon).ToBeVisibleAsync();
            await Expect(textAreaWithoutIcon).ToBeVisibleAsync();

            // Ensure the icon is present for the first text area (with icon)
            await Expect(Page.Locator(".fa-pen")).ToBeVisibleAsync();
        }

        // Test if the text area saves input after closing (clicking outside)
        [Fact]
        public async Task TextAreaSavesOnClose()
        {
            // Navigate to the test page
            await Page.GotoAsync("http://localhost:8080/test/text-area");

            // Locate the textarea inside the #textBoxWithIcon div
            var textArea = Page.Locator("#textBoxWithIcon textarea");

            // Wait for the textarea to be visible and attached
            await Expect(textArea).ToBeVisibleAsync();
            await Expect(textArea).ToBeAttachedAsync();

            // Fill the textarea with text
            await textArea.FillAsync("Updated text");

            // Simulate closing by clicking outside (click anywhere on the body)
            await Page.ClickAsync("body");

            // Verify that the text is retained in the textarea (saved)
            await Expect(textArea).ToHaveValueAsync("Updated text");
        }

        // Test if the placeholder appears correctly in the text area
        [Fact]
        public async Task TextAreaHasCorrectPlaceholder()
        {
            // Navigate to the test page
            await Page.GotoAsync("http://localhost:8080/test/text-area");

            // Locate the text area with the placeholder text
            var textAreaWithIcon = Page.Locator("#textBoxWithIcon");

            // Locate the actual textarea element inside the #textBoxWithIcon div
            var textArea = Page.Locator("#textBoxWithIcon textarea");

            // Check if the placeholder is correct
            await Expect(textAreaWithIcon).ToHaveAttributeAsync("placeholder", "Text box with an icon here...");
            await Expect(textArea).ToHaveAttributeAsync("placeholder", "Text box with an icon here...");
        }


        // Test if the label is displayed correctly
        [Fact]
        public async Task TextAreaDisplaysCorrectLabel()
        {
            // Navigate to the test page
            await Page.GotoAsync("http://localhost:8080/test/text-area");

            // Check for the header text (Icon Header) for the first text area
            var headerWithIcon = Page.Locator("#textBoxWithIcon h5 p");
            await Expect(headerWithIcon).ToHaveTextAsync("Icon Header");
            
            // Check for the header text (No Icon Header) for the second text area
            var headerWithoutIcon = Page.Locator("#textBoxWithoutIcon h5 p");
            await Expect(headerWithoutIcon).ToHaveTextAsync("No Icon Header");
        }
    }
}