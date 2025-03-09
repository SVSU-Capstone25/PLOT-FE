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

        var slideOut = Page.Locator("aside#TestSlideOut");

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

        // Verify that the SlideOut is collapsed.
        // 500ms to account for the speed of change
        var slideOut = Page.Locator("aside#TestSlideOut.collapsed");
        await Task.Delay(500);
        await Expect(slideOut).ToBeVisibleAsync();

        // Click the toggle button to open the SlideOut.
        await toggleButton.ClickAsync();

        // Verify that the SlideOut is not collapsed.
        await Expect(slideOut).ToBeHiddenAsync();

        // Click the toggle button to close the SlideOut.
        await toggleButton.ClickAsync();
        
        // Verify that the SlideOut is collapsed.
        // 500ms to account for the speed of change
        await Task.Delay(500);
        await Expect(slideOut).ToBeVisibleAsync();
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
        var slideOut = Page.Locator("aside#TestSlideOut.collapsed");
        await Expect(slideOut).ToBeHiddenAsync();

        // Verify that the SlideOut has content.
        var slideOutContent = Page.Locator("aside#TestSlideOut p");
        await Expect(slideOutContent).ToBeVisibleAsync();
    }

}