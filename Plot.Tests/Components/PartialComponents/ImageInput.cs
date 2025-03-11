// /* Filename: ImageInput.cs
// Part of Project: PLOT

// File Purpose:
// The purpose of this file is to create and run tests that verify the functionality of the ImageInput component.

// Program Purpose:
// The purpose of PLOT is to allow users to easily create, manage, 
// and allocate floorsets for Platos Closet. 

// Author: Blake Pearsall (2/12/2025)  */

// using Microsoft.Playwright.Xunit;
// using Xunit;

// namespace PlaywrightTests;

// public class ImageInputTest : PageTest
// {
//     /// <summary>
//     /// Test to verify that the ImageInput component has the correct properties and elements on the page.
//     /// </summary>
//     /// <returns></returns>
//     [Fact]
//     public async Task HasImageInputAndProperties()
//     {
//         // Go to test page
//         await Page.GotoAsync("http://localhost:8080/test/image-input");

//         // Expect the page to have a body visibile
//         await Expect(Page.Locator("body")).ToBeVisibleAsync();

//         var imageInput = Page.Locator("#ExampleImageInput");

//         // Expect the image input to be on the page
//         await Expect(imageInput).ToBeVisibleAsync();

//         // Verify that the upload iamge label exists
//         var label = imageInput.Locator("h5.text-decoration-underline.my-auto.text-center");
//         await Expect(label).ToBeVisibleAsync();

//         // Verify initial background color of gray and border style of dashed
//         var initialBackgroundColor = await imageInput.EvaluateAsync<string>("element => getComputedStyle(element).backgroundColor");
//         var initialBorderStyle = await imageInput.EvaluateAsync<string>("element => getComputedStyle(element).borderStyle");
//         Assert.Equal("rgb(242, 242, 242)", initialBackgroundColor); // #f2f2f2 in RGB
//         Assert.Equal("dashed", initialBorderStyle);
//     }

//     /// <summary>
//     /// Test to verify that the ImageInput component changes the background image and border style after 
//     // a file is uploaded by clicking the component.
//     /// </summary>
//     /// <returns></returns>
//     [Fact]
//     public async Task SimulateFileUpload()
//     {
//         // Go to test page
//         await Page.GotoAsync("http://localhost:8080/test/image-input");

//         // Expect the page to have a body visibile
//         await Expect(Page.Locator("body")).ToBeVisibleAsync();

//         var imageInput = Page.Locator("#ExampleImageInput");
//         var fileInput = Page.Locator("#ExampleImageInput-fileInput");

//         // Simulate image file upload
//         await fileInput.SetInputFilesAsync(new[] { "../../../Components/PartialComponents/TestImages/myman.jpg" });

//         // Verify that the background image is set correctly
//         var backgroundImage = await imageInput.EvaluateAsync<string>("element => getComputedStyle(element).backgroundImage");
//         Assert.Contains("data:image/jpeg;base64,", backgroundImage);

//         // Verify that the border style is removed after image upload
//         var updatedBorderStyle = await imageInput.EvaluateAsync<string>("element => getComputedStyle(element).borderStyle");
//         Assert.Equal("none", updatedBorderStyle);
//     }

//     /// <summary>
//     /// Test to verify that the ImageInput component changes the background image and border style after
//     /// a file is uploaded by dragging and dropping the file onto the component.
//     /// </summary>
//     /// <returns></returns>
//     [Fact]
//     public async Task SimulateDragDrop()
//     {
//         // Go to test page
//         await Page.GotoAsync("http://localhost:8080/test/image-input");

//         // Expect the page to have a body visible
//         await Expect(Page.Locator("body")).ToBeVisibleAsync();

//         var imageInput = Page.Locator("#ExampleImageInput");

//         // Get the file path for the test image
//         var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../Components/PartialComponents/TestImages/myman.jpg");

//         // Create a DataTransfer object in JavaScript for drag-and-drop
//         var dataTransfer = await Page.EvaluateHandleAsync(@"() => {
//             const dt = new DataTransfer();
//             return dt;
//         }");

//         await Page.EvaluateAsync(@"({ dt, filePath }) => {
//             const file = new File([''], filePath.split('/').pop(), { type: 'image/jpeg' });
//             dt.items.add(file);
//             }", new { dt = dataTransfer, filePath });


//         // Simulate drag-and-drop events and deposit the image file
//         await Page.DispatchEventAsync("#ExampleImageInput", "dragenter", new { dataTransfer });
//         await Page.DispatchEventAsync("#ExampleImageInput", "dragover", new { dataTransfer });
//         await Page.DispatchEventAsync("#ExampleImageInput", "drop", new { dataTransfer });

//         // Wait for drag & drop to trigger
//         await Page.WaitForTimeoutAsync(500);

//         // Verify that the background image is set correctly
//         var backgroundImage = await imageInput.EvaluateAsync<string>("element => getComputedStyle(element).backgroundImage");
//         Assert.Contains("data:image/jpeg;base64,", backgroundImage);

//         // Verify that the border style is removed after image upload
//         var updatedBorderStyle = await imageInput.EvaluateAsync<string>("element => getComputedStyle(element).borderStyle");
//         Assert.Equal("none", updatedBorderStyle);
//     }

// }
