// using Microsoft.Playwright.Xunit;
// using Xunit;

// namespace PlaywrightTests;

// public class DateInputTest : PageTest
// {
//     [Fact]
//     public async Task TestElementsPresence()
//     {
//         // Navigate to the test page containing the date input field
//         await Page.GotoAsync("http://localhost:8080/test/date-input");
//         Console.WriteLine("Navigated to test page.");

//         // Locate elements on the page
//         var dateInput = Page.Locator("#date-input");
//         var label = Page.Locator(".inner-date-label");
//         var selectedDateText = Page.Locator("p:has-text('Selected Date:')");

//         Console.WriteLine("Located date input, label, and selected date elements.");

//         await Task.Delay(500);
//         // Check if elements are visible on the page
//         var dateInputIsVisible = await dateInput.IsVisibleAsync();
//         var labelIsVisible = await label.IsVisibleAsync();
//         var selectedDateTextIsVisible = await selectedDateText.IsVisibleAsync();

//         //Console.WriteLine($"Date input visibility: {dateInputIsVisible}");
//         //Console.WriteLine($"Label visibility: {labelIsVisible}");
//         //Console.WriteLine($"Selected date text visibility: {selectedDateTextIsVisible}");

//         // Assert that all expected elements are present and visible
//         Assert.True(dateInputIsVisible, "Date input field is not visible.");
//         Assert.True(labelIsVisible, "Label is not visible.");
//         Assert.True(selectedDateTextIsVisible, "Selected date text is not visible.");
//     }
    
//     [Fact]
//     public async Task TestValidDateEntry()
//     {
//         // Navigate to the test page containing the date input field
//         await Page.GotoAsync("http://localhost:8080/test/date-input");
//         Console.WriteLine("Navigated to test page.");

//         // Locate elements on the page
//         var dateInput = Page.Locator("#date-input");
//         await Expect(dateInput).ToBeVisibleAsync(new() { Timeout = 5000 });
//         var selectedDateText = Page.Locator("p:has-text('Selected Date:')");
//         var header = Page.Locator("h1"); // Click target to move focus away


//         Console.WriteLine("Located date input and selected date elements.");

//         // Clear the date input field before entering a valid date
//         await dateInput.FillAsync("");
//         Console.WriteLine("Cleared the date input field.");
//         await header.ClickAsync();
//         await Page.WaitForTimeoutAsync(1000);

//         // Enter a valid date using keyboard typing
//         await dateInput.FillAsync("2025-02-18"); // Valid date (02/18/2025)
//         Console.WriteLine("Typed valid date: 02182025");

//         // Move focus away to apply the valid date
//         await header.ClickAsync();
//         await Page.WaitForTimeoutAsync(1000);

//         // Verify that the selected date text is updated correctly
//         await Expect(selectedDateText).ToHaveTextAsync("Selected Date: 02/18/2025", new() { Timeout = 5000 });
//         await Expect(dateInput).ToHaveValueAsync("2025-02-18", new() { Timeout = 5000 });

//         // Retrieve and log the value of the input field and selected date text
//         var validDateValue = await dateInput.InputValueAsync();
//         var selectedDateValue = await selectedDateText.InnerTextAsync();
//         Console.WriteLine($"Date input field value after valid entry: {validDateValue}");
//         Console.WriteLine($"Selected date text after valid entry: {selectedDateValue}");
//     }

//     [Fact]
//     public async Task TestInvalidDateEntry()
//     {
//         // Navigate to the test page containing the date input field
//         await Page.GotoAsync("http://localhost:8080/test/date-input");
//         Console.WriteLine("Navigated to test page.");

//         // Locate elements on the page
//         var dateInput = Page.Locator("#date-input");
//         var selectedDateText = Page.Locator("p:has-text('Selected Date:')");
//         var dateLabel = Page.Locator("label.inner-date-label"); // Capturing label
//         var header = Page.Locator("h1"); // Click target to move focus away

//         Console.WriteLine("Located date input and selected date elements.");

//         // Print label text before entering an invalid date
//         var initialLabelText = await dateLabel.InnerTextAsync();
//         Console.WriteLine($"Before invalid entry, label text: {initialLabelText}");

//         // Click the input field to ensure focus before typing
//         await dateInput.ClickAsync();
//         Console.WriteLine("Clicked on date input.");

//         // Type an invalid date into the input field
//         await dateInput.FocusAsync();
//         await Page.Keyboard.TypeAsync("02312025"); // Invalid date
//         Console.WriteLine("Typed invalid date: 02312025");

//         // Move focus away to trigger UI validation and updates
//         await header.ClickAsync();
//         await Page.WaitForTimeoutAsync(1000);

//         // Verify that the selected date text remains unchanged
//         await Expect(selectedDateText).ToHaveTextAsync("Selected Date: ", new() { Timeout = 3000 });
//         //await Expect(dateInput).ToHaveValueAsync("275760-05-05", new() { Timeout = 2000 });

//         // Retrieve and log the value of the input field and selected date text
//         var invalidDateValue = await dateInput.InputValueAsync();
//         var selectedDateValue = await selectedDateText.InnerTextAsync();
//         Console.WriteLine($"Date input field value after invalid entry: {invalidDateValue}");
//         Console.WriteLine($"Selected date text after invalid entry: {selectedDateValue}");

//         // Print label text after entering an invalid date
//         var finalLabelText = await dateLabel.InnerTextAsync();
//         Console.WriteLine($"After invalid entry, label text: {finalLabelText}");
//     }

//     // Note: I couldn't test the "Invalid Date!" label directly, as entering an invalid date 
//     // sometimes results in the input being populated with a default value (e.g., "275760-05-05") 
//     // or remaining blank. The test still accurately checks that the "Selected Date" should be empty.
// }

// // Note: I couldn't test the actual calendar UI of the date input field programmatically, 
// // as Playwright struggled to locate and interact with the calendar elements. The functionality of the calendar 
// // is browser-dependent, making the calendar interaction 
// // inconsistent across platforms. I tested this functionality manually to ensure the 
// // date selection works as expected, but due to browser-specific variations, automating the 
// // calendar selection via Playwright did not seem to work correctly.
