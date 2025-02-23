using Microsoft.Playwright.Xunit;
using Xunit;

namespace PlaywrightTests;

public class CheckboxTest : PageTest
{
    [Fact]
    public async Task HasCheckboxAndContent()
    {
  // Go to the test page
        await Page.GotoAsync("http://localhost:8080/test/checkbox");

        // Expect the page to have a body visible.
        await Expect(Page.Locator("body")).ToBeVisibleAsync();

        // Locate the checkbox using the class name (form-check-input class)
        var checkbox = Page.Locator("#checkboxOne");

        // Ensure the checkbox is visible
        await Expect(checkbox).ToBeVisibleAsync();

        // Find the label associated with the checkbox using the 'for' attribute
        var label = Page.Locator("label[for='checkboxOne']");
        
        // Extract label text
        var labelText = await label.TextContentAsync();

        // Assert that the label text is correct
        Assert.Equal("I'm a checkbox", labelText);

        var isChecked = await checkbox.IsCheckedAsync();
        Assert.False(isChecked, "Checkbox should be unchecked by default.");

        // Uncheck the checkbox
        await checkbox.CheckAsync();
        
        // Ensure the checkbox is checked
        isChecked = await checkbox.IsCheckedAsync();
        Assert.True(isChecked, "Checkbox should be checked after interaction.");
    }


    // For the second checkbox (checked by default)
    [Fact]
    public async Task HasCheckboxTwoAndContent()
    {
        // Go to the test page
        await Page.GotoAsync("http://localhost:8080/test/checkbox");

        // Expect the page to have a body visible.
        await Expect(Page.Locator("body")).ToBeVisibleAsync();

        // Locate checkboxTwo using the id (#checkboxTwo)
        var checkboxTwo = Page.Locator("#checkboxTwo");

        // Ensure the checkbox is visible
        await Expect(checkboxTwo).ToBeVisibleAsync();

        // Find the label associated with checkboxTwo using the 'for' attribute
        var labelTwo = Page.Locator("label[for='checkboxTwo']");

        // Extract label text
        var labelText = await labelTwo.TextContentAsync();

        // Assert that the label text is correct
        Assert.Equal("I'm a checkbox checked by default", labelText);

        // Ensure checkboxTwo is initially checked (should be true by default)
        var isChecked = await checkboxTwo.IsCheckedAsync();
        Assert.True(isChecked, "Checkbox should be checked by default.");

        // Uncheck checkboxTwo
        await checkboxTwo.UncheckAsync();

        // Ensure checkboxTwo is unchecked after interaction
        isChecked = await checkboxTwo.IsCheckedAsync();
        Assert.False(isChecked, "Checkbox should be unchecked after interaction.");
    }
}
