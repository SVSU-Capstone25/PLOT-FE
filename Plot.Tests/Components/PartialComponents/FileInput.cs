/* 
    Filename:- FileInput.cs
    Part of Project: Plot

    File Purpose:
    This file creates and uses test to verify the functionality of the FileInput component.

    Program Purpose:
    The purpose of PLOT is to allow users to easily create, manage,
    and allocate floorsets for Plato's Closet.

    Author: Blake Pearsall (2/19/2025)
*/

using Microsoft.Playwright.Xunit;
using Xunit;

namespace PlaywrightTests;

public class FileInputTest : PageTest
{
    /// <summary>
    /// Test to verify that the FileInput component is rendered correctly.
    /// </summary>
    [Fact]
    public async Task HasFileInput()
    {
        // Go to test page.
        await Page.GotoAsync("http://localhost:8080/test/file-input");

        // Expect the page to have a body visibile.
        await Expect(Page.Locator("body")).ToBeVisibleAsync();

        var fileInput = Page.Locator("#TestFileInput");

        // Expect a button to be on the page.
        await Expect(fileInput).ToBeVisibleAsync();

         // Verify that the upload iamge label exists
        var label = fileInput.Locator("label.load-files-label");
        await Expect(label).ToBeVisibleAsync();

        // Verify initial background color of gray and border style of dashed
        var initialBackgroundColor = await fileInput.EvaluateAsync<string>("element => getComputedStyle(element).backgroundColor");
        var initialBorderStyle = await fileInput.EvaluateAsync<string>("element => getComputedStyle(element).borderStyle");
        Assert.Equal("rgb(235, 235, 235)", initialBackgroundColor); // #f2f2f2 in RGB
        Assert.Equal("dashed", initialBorderStyle); 
    }

    /// <summary>
    /// Test to verify that the FileInput component can take an uploaded file via the input element.
    /// </summary>
    /*[Fact]
    public async Task SimulateFileUpload()
    {
        // Go to test page.
        await Page.GotoAsync("http://localhost:8080/test/file-input");

        // Expect the page to have a body visible.
        await Expect(Page.Locator("body")).ToBeVisibleAsync();

        var fileInput = Page.Locator("#TestFileInput .file-select input[type='file']");

        // Simulate file upload.
        await fileInput.SetInputFilesAsync(new[] { "../../../Components/PartialComponents/TestImages/myman.jpg" });

        // Verify that the background image is set correctly.
        var fileInputZone = Page.Locator("#TestFileInput");
        var backgroundImage = await fileInputZone.EvaluateAsync<string>("element => getComputedStyle(element).backgroundImage");
        Assert.Contains("data:image/jpeg;base64,", backgroundImage);

        // Verify that the border style is removed after file upload.
        var updatedBorderStyle = await fileInputZone.EvaluateAsync<string>("element => getComputedStyle(element).borderStyle");
        Assert.Equal("none", updatedBorderStyle);
    }*/

}