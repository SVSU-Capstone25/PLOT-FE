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

            // Locate the editable div inside the custom text area
            var editableDiv = Page.Locator("#textBoxWithIcon div[contenteditable='true']");

            // Wait for the editable div to be visible and attached
            await Expect(editableDiv).ToBeVisibleAsync();
            await Expect(editableDiv).ToBeAttachedAsync();

            // Fill the div with text (or type, depending on implementation)
            await editableDiv.FillAsync("Updated text");

            // Simulate closing by clicking outside (click anywhere on the body)
            await Page.ClickAsync("body");

            // Verify that the text is retained in the editable div (saved)
            await Expect(editableDiv).ToHaveTextAsync("Updated text");
        }


        // Test if the placeholder appears correctly in the text area
        [Fact]
        public async Task TextAreaHasCorrectPlaceholder()
        {
            // Navigate to the test page
            await Page.GotoAsync("http://localhost:8080/test/text-area");

            // Locate the text area with the placeholder text
            var textAreaWithIcon = Page.Locator("#textBoxWithIcon");

            // Check if the placeholder is correct
            await Expect(textAreaWithIcon).ToHaveAttributeAsync("placeholder", "Text box with an icon here...");
        }

        // Test if the label is displayed correctly
        [Fact]
        public async Task TextAreaDisplaysCorrectLabel()
        {
            // Navigate to the test page
            await Page.GotoAsync("http://localhost:8080/test/text-area");

            // Check for the label of the first text area
            var label = Page.Locator("label[for='textBoxWithIcon']");
            await Expect(label).ToHaveTextAsync("Icon Header");

            // Check for the label of the second text area
            var labelWithoutIcon = Page.Locator("label[for='textBoxWithoutIcon']");
            await Expect(labelWithoutIcon).ToHaveTextAsync("No Icon Header");
        }
    }
}
