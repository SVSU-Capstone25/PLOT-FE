using Microsoft.Playwright.Xunit;
using Xunit;

namespace PlaywrightTests;

public class ImageInputTest : PageTest
{
    [Fact]
    public async Task HasImageInput()
    {
        // Go to test page.
        await Page.GotoAsync("http://localhost:8080/test/image-input");

        // Expect the page to have a body visibile.
        await Expect(Page.Locator("body")).ToBeVisibleAsync();

        var imageInput = Page.Locator("#ExampleImageInput");

        // Expect the image input to be on the page.
        await Expect(imageInput).ToBeVisibleAsync();

        // Simulate image file upload.
        await imageInput.SetInputFilesAsync(new[] { @"D:/SVSU/Capstone/Test Images/myman.jpg" });

        // Verify that the image is set correctly.
        //var backgroundImage = await imageInput.EvaluateAsync<string>("element => getComputedStyle(element).backgroundImage");
        //Assert.Contains("myman.jpg", backgroundImage);
    }

}