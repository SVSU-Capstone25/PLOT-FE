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

        // Verify that the upload iamge label exists.
        var label = imageInput.Locator("h5.text-decoration-underline.my-auto.text-center");
        await Expect(label).ToBeVisibleAsync();

        // Verify initial background color of gray and border style of dashed.
        var initialBackgroundColor = await imageInput.EvaluateAsync<string>("element => getComputedStyle(element).backgroundColor");
        var initialBorderStyle = await imageInput.EvaluateAsync<string>("element => getComputedStyle(element).borderStyle");
        Assert.Equal("rgb(242, 242, 242)", initialBackgroundColor); // #f2f2f2 in RGB
        Assert.Equal("dashed", initialBorderStyle);

        var fileInput = Page.Locator("#ExampleImageInput-fileInput");

        // Simulate image file upload.
        await fileInput.SetInputFilesAsync(new[] { "../../../Components/PartialComponents/TestImages/myman.jpg" });

        // Verify that the background image is set correctly.
        var backgroundImage = await imageInput.EvaluateAsync<string>("element => getComputedStyle(element).backgroundImage");
        Assert.Contains("data:image/jpeg;base64,", backgroundImage);

        // Verify that the border style is removed after image upload.
        var updatedBorderStyle = await imageInput.EvaluateAsync<string>("element => getComputedStyle(element).borderStyle");
        Assert.Equal("none", updatedBorderStyle);
    }

}