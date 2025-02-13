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

        // Verify that the label exists.
        var label = imageInput.Locator("h5.text-decoration-underline.my-auto.text-center");
        await Expect(label).ToBeVisibleAsync();

        var fileInput = Page.Locator("#ExampleImageInput-fileInput");

        // Simulate image file upload based on an image file on local machine.
        await fileInput.SetInputFilesAsync(new[] { @"D:/SVSU/Capstone/Test Images/myman.jpg" });

        // Verify that the background image is set correctly.
        var backgroundImage = await imageInput.EvaluateAsync<string>("element => getComputedStyle(element).backgroundImage");
        Assert.Contains("data:image/jpeg;base64,", backgroundImage);
    }

}