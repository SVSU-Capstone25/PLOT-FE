/*
    Filename:- SlideOut.cs
    Part of Project: Plot

    File Purpose:
    The purpose of this file is to create and run tests for the SlideOut component.

    Program Purpose:
    The purpose of PLOT is to allow users to easily create, manage,
    and allocate floorsets for Plato's Closet.

    Author: Blake Pearsall (2/13/2025)
*/

using Microsoft.Playwright.Xunit;
using Xunit;

namespace PlaywrightTests;

public class SlideOutTest : PageTest
{
    [Fact]
    public async Task HasSlideOut()
    {
        // Go to test page.
        await Page.GotoAsync("http://localhost:8080/");

        // Expect the page to have a body visibile.
        await Expect(Page.Locator("body")).ToBeVisibleAsync();

        var slideOut = Page.Locator("#TestSlideOut");

        // Expect a button to be on the page.
        await Expect(slideOut).ToBeVisibleAsync();
    }

    [Fact]
    public async Task ToggleSlideOut()
    {
        // Go to test page.
        await Page.GotoAsync("http://localhost:8080/");

        // Locate the toggle button.
        var toggleButton = Page.Locator("#toggle-btn");

        // Click the toggle button to open the SlideOut.
        await toggleButton.ClickAsync();

        // Verify that the SlideOut is not collapsed.
        var slideOut = Page.Locator(".collapse-sidebar");
        await Expect(slideOut).Not.ToHaveClassAsync("collapse-sidebar collapsed");

        // Click the toggle button to close the SlideOut.
        await toggleButton.ClickAsync();

        // Verify that the SlideOut is collapsed.
        await Expect(slideOut).ToHaveClassAsync("collapse-sidebar collapsed");
    }

}