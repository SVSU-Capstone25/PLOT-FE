// Author : Tristan Calay
// Represents an instance of a fixture object applied onto the grid

public class FixtureInstance : Fixture
{
    public int editorID { get; init; }
    // Tristan Calay 4/4/25
    // Added tracking for notes and category.
    public string selectedSuperCategory { get; set; } = string.Empty;
    public string selectedSubCategory { get; set; } = string.Empty;

    public string notes { get; set; } = string.Empty;

    public int gridX { get; set; } = -1;
    public int gridY { get; set; } = -1;


    public FixtureInstance(Fixture fixture, int id) : base(fixture.Name, fixture.Length, fixture.Width, fixture.HangerStack, fixture.FixtureImg)
    {
        this.editorID = id;
    }

    public void setNewPosition(int x, int y)
    {
        Console.WriteLine("Moving fixture " + editorID);
        gridX = x;
        gridY = y;
        Console.WriteLine("New Position: " + gridX + ", " + gridY);
    }
}