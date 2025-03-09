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

using System.Text.RegularExpressions;
using Microsoft.Playwright.Xunit;
using Xunit;

namespace PlaywrightTests;

/// <summary>
/// Tests to verify that the slide out componenet exists.
/// </summary>
public class SlideOutTest : PageTest
{
    [Fact]
    public async Task HasSlideOut()
    {
        // Go to test page.
        await Page.GotoAsync("http://localhost:8080/test/slide-out");

        // Expect the page to have a body visibile.
        await Expect(Page.Locator("body")).ToBeVisibleAsync();

        var slideOut = Page.Locator("#TestSlideOut");

        // Expect a button to be on the page.
        await Expect(slideOut).ToBeVisibleAsync();
    }

    /// <summary>
    /// Tests to verify that the slide out component slides when the toggle button is clicked.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ToggleSlideOut()
    {
        // Go to test page.
        await Page.GotoAsync("http://localhost:8080/test/slide-out");

        // Locate the toggle button.
        var toggleButton = Page.Locator("button#toggle-btn");

        // Click the toggle button to open the SlideOut.
        await toggleButton.ClickAsync();

        // Verify that the SlideOut is not collapsed.
        var slideOut = Page.Locator("#TestSlideOut");
        await Expect(slideOut).Not.ToHaveClassAsync(new Regex("(^|\\s)collapsed(\\s|$)"));

        // Click the toggle button to close the SlideOut.
        await toggleButton.ClickAsync();

        // Verify that the SlideOut is collapsed.
        await Expect(slideOut).ToHaveClassAsync(new Regex("(^|\\s)collapsed(\\s|$)"));
    }

    /// <summary>
    /// Tests to verify that the slide out component has content when it is open.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task SlideOutHasContent()
    {
        // Go to test page.
        await Page.GotoAsync("http://localhost:8080/test/slide-out");

        // Locate the toggle button.
        var toggleButton = Page.Locator("button#toggle-btn");

        // Click the toggle button to open the SlideOut.
        await toggleButton.ClickAsync();

        // Verify that the SlideOut is not collapsed.
        var slideOut = Page.Locator("#TestSlideOut");
        await Expect(slideOut).Not.ToHaveClassAsync(new Regex("(^|\\s)collapsed(\\s|$)"));

        // Verify that the SlideOut has content.
        var slideOutContent = Page.Locator("p");
        await Expect(slideOutContent).ToBeVisibleAsync();
    }

}