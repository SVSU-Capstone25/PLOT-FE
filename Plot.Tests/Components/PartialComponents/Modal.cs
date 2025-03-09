using Microsoft.Playwright.Xunit;
using Xunit;

namespace PlaywrightTests
{
    public class ModalTest : PageTest
    {
        // Test for checking the content and role of the Standard Modal
        [Fact]
        public async Task HasStandardModalWithContentAndRole()
        {
            // Navigate to the test page where modals are defined
            await Page.GotoAsync("http://localhost:8080/test/modal");

            // Wait for the body to be visible to ensure the page has loaded
            await Expect(Page.Locator("body")).ToBeVisibleAsync();

            // Wait for the Standard Modal button to be visible before interacting
            var standardModalButton = Page.Locator("button[data-bs-target='#standardModal']");
            await Expect(standardModalButton).ToBeVisibleAsync();

            // Click to open the Standard Modal
            await standardModalButton.ClickAsync();

            // Wait for the modal to be visible
            var standardModal = Page.Locator("div#standardModal");
            await Expect(standardModal).ToBeVisibleAsync();

            // Verify the role attribute of the modal (should be dialog)
            var role = await standardModal.GetAttributeAsync("role");
            Assert.Equal("dialog", role);

            // Test if the modal contains the expected content
            var modalContent = standardModal.Locator(".modal-body h4");
            await Expect(modalContent).ToHaveTextAsync("Modal content 1");
        }

        // Test for checking the content and role of the Danger Modal
        [Fact]
        public async Task HasDangerModalWithContentAndRole()
        {
            // Navigate to the test page where modals are defined
            await Page.GotoAsync("http://localhost:8080/test/modal");

            // Wait for the body to be visible to ensure the page has loaded
            await Expect(Page.Locator("body")).ToBeVisibleAsync();

            // Wait for the Danger Modal button to be visible before interacting
            var dangerModalButton = Page.Locator("button[data-bs-target='#dangerModal']");
            await Expect(dangerModalButton).ToBeVisibleAsync();

            // Click to open the Danger Modal
            await dangerModalButton.ClickAsync();

            // Wait for the modal to be visible
            var dangerModal = Page.Locator("#dangerModal");
            await Expect(dangerModal).ToBeVisibleAsync();

            // Verify the role attribute of the modal (should be dialog)
            var role = await dangerModal.GetAttributeAsync("role");
            Assert.Equal("dialog", role);

            // Test if the modal contains the expected content
            var modalContent = dangerModal.Locator(".modal-body h4");
            await Expect(modalContent).ToHaveTextAsync("Modal content 2");
        }

        // Test for checking the content and role of the Warning Modal
        [Fact]
        public async Task HasWarningModalWithContentAndRole()
        {
            // Navigate to the test page where modals are defined
            await Page.GotoAsync("http://localhost:8080/test/modal");

            // Wait for the body to be visible to ensure the page has loaded
            await Expect(Page.Locator("body")).ToBeVisibleAsync();

            // Wait for the Warning Modal button to be visible before interacting
            var warningModalButton = Page.Locator("button[data-bs-target='#warningModal']");
            await Expect(warningModalButton).ToBeVisibleAsync();

            // Click to open the Warning Modal
            await warningModalButton.ClickAsync();

            // Wait for the modal to be visible
            var warningModal = Page.Locator("#warningModal");
            await Expect(warningModal).ToBeVisibleAsync();

            // Verify the role attribute of the modal (should be dialog)
            var role = await warningModal.GetAttributeAsync("role");
            Assert.Equal("dialog", role);

            // Test if the modal contains the expected content
            var modalContent = warningModal.Locator(".modal-body h4");
            await Expect(modalContent).ToHaveTextAsync("Modal content 3");
        }

        // Test for checking the content and role of the Success Modal
        [Fact]
        public async Task HasSuccessModalWithContentAndRole()
        {
            // Navigate to the test page where modals are defined
            await Page.GotoAsync("http://localhost:8080/test/modal");

            // Wait for the body to be visible to ensure the page has loaded
            await Expect(Page.Locator("body")).ToBeVisibleAsync();

            // Wait for the Success Modal button to be visible before interacting
            var successModalButton = Page.Locator("button[data-bs-target='#successModal']");
            await Expect(successModalButton).ToBeVisibleAsync();

            // Click to open the Success Modal
            await successModalButton.ClickAsync();

            // Wait for the modal to be visible
            var successModal = Page.Locator("#successModal");
            await Expect(successModal).ToBeVisibleAsync();

            // Verify the role attribute of the modal (should be dialog)
            var role = await successModal.GetAttributeAsync("role");
            Assert.Equal("dialog", role);

            // Test if the modal contains the expected content
            var modalContent = successModal.Locator(".modal-body h4");
            await Expect(modalContent).ToHaveTextAsync("Modal content 4");
        }
    }
}
