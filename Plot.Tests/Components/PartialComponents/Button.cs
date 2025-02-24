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
            //navigate to page
            await Page.GotoAsync("http://localhost:8080/test/button");

            //locate each button
            await var button1 = Page.Locator("#buttonPrimary");
            await var button2 = Page.Locator("#buttonSuccess");
            await var button3 = Page.Locator("#buttonDanger");

            //verify that are visable (content is checked when we locate them)
            await Expect(button1).ToBeVisibleAsync();
            await Expect(button2).ToBeVisibleAsync();
            await Expect(button3).ToBeVisibleAsync();
        }

        // Test to check if button variants are applied correctly
        [Fact]
        public async Task ButtonVariantsAreCorrect()
        {
            //navigate to page
            await Page.GotoAsync("http://localhost:8080/test/button");

            //locate each button
            await var button1 = Page.Locator("#buttonPrimary");
            await var button2 = Page.Locator("#buttonSuccess");
            await var button3 = Page.Locator("#buttonDanger");

            //get class atrributes
            var classList1 = await button1.GetAttributeAsync("class");
            var classList2 = await button2.GetAttributeAsync("class");
            var classList3 = await button3.GetAttributeAsync("class");

            //check that each button has the proper class (primary, success, danger)
            Assert.Contains("btn-primary", classList1?.Trim()); //'btn-primary'
            Assert.Contains("btn-success", classList2?.Trim()); //'btn-success'
            Assert.Contains("btn-danger", classList3?.Trim()); //'btn-danger'
        }


        // Test to check if the correct icons are displayed in the buttons.
        [Fact]
        public async Task ButtonIconsAreCorrect()
        {
            //navigate to page
            await Page.GotoAsync("http://localhost:8080/test/button");

            //locate each button
            await var button1 = Page.Locator("#buttonPrimary");
            await var button2 = Page.Locator("#buttonSuccess");
            await var button3 = Page.Locator("#buttonDanger");

            //verify their respective icons are visable
            await Expect(button1.Locator("i.fa-ellipsis")).ToBeVisibleAsync();
            await Expect(button2.Locator("i.fa-download")).ToBeVisibleAsync();
            await Expect(button3.Locator("i.fa-copy")).ToBeVisibleAsync();
        }

        // Test to check the text alignment in buttons by inspecting the inline style.
        [Fact]
        public async Task ButtonTextAlignmentIsCorrect()
        {
            //navigate to page
            await Page.GotoAsync("http://localhost:8080/test/button");

            //locate each button
            await var button1 = Page.Locator("#buttonPrimary");
            await var button2 = Page.Locator("#buttonSuccess");
            await var button3 = Page.Locator("#buttonDanger");

            //get the class list from each button
            var classList1 = await button1.GetAttributeAsync("class");
            var classList2 = await button2.GetAttributeAsync("class");
            var classList3 = await button3.GetAttributeAsync("class");

            Console.WriteLine($"Button 1 Classes: {classList1}");
            Console.WriteLine($"Button 2 Classes: {classList2}");
            Console.WriteLine($"Button 3 Classes: {classList3}");

            //check for each proper text alignment class
            Assert.Contains("text-center", classList1?.Trim()); //'text-center'
            Assert.Contains("text-end", classList2?.Trim()); //'text-end'
            Assert.Contains("text-start", classList3?.Trim()); //'text-start'
        }

    }
}