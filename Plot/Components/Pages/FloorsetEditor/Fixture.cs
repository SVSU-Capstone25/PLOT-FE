/*
        The Fixture class stores data for a fixture.

        Author: 
    */
public class Fixture
{

    public string Name { get; set; } = string.Empty;
    public string Length { get; set; } = string.Empty;
    public string Width { get; set; } = string.Empty;
    public string? LFCapacity { get; set; } = string.Empty;
    public string HangerStack { get; set; } = string.Empty;
    public string? TotalLF { get; set; } = string.Empty;
    public string FixtureImg { get; set; } = string.Empty;

    private static string TempImage = "iVBORw0KGgoAAAANSUhEUgAAAQAAAAEACAIAAADTED8xAAADMElEQVR4nOzVwQnAIBQFQYXff81RUkQCOyDj1YOPnbXWPmeTRef+/3O/OyBjzh3CD95BfqICMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMO0TAAD//2Anhf4QtqobAAAAAElFTkSuQmCC";

    public static SortedDictionary<string, Fixture> fixtures = new()
    {
        ["Single Hung Rack"] = new Fixture("Single Hung Rack", "5", "2", "1", "data:image/png;base64," + TempImage),
        ["Double Hung Rack"] = new Fixture("Double Hung Rack", "3", "2", "2", "data:image/png;base64," + TempImage),
        ["Shelf"] = new Fixture("Shelf", "2", "1", "1", "data:image/png;base64," + TempImage),

    };

    /* Fixture constructor to initialize the values */
    public Fixture(string name, string length, string width, string hangerStack, string fixtureImg)
    {
        try
        {
            this.Name = name;
            this.Length = length;
            this.Width = width;
            this.LFCapacity = Length.Equals(width) && Length.Equals("") ? "" : (int.Parse(Length) * int.Parse(Width)).ToString() + ""; ;
            this.HangerStack = hangerStack;
            this.TotalLF = LFCapacity.Equals(hangerStack) && LFCapacity.Equals("") ? "" : (int.Parse(LFCapacity) * int.Parse(HangerStack)).ToString() + "";
            this.FixtureImg = fixtureImg;
        }
        catch (FormatException e)
        {
            Console.WriteLine(e.GetBaseException());
        }
    }

    /*
        The HasNullProperties function returns true if any property is empty.
    */
    public bool HasNullProperties() =>
    GetType().GetProperties().Any(p =>
    {
        var pValue = p.GetValue(this);
        return pValue == null || pValue.Equals("");
    });

    /*
        The UpdateLFCapacity function updates the linear feet capacity of the fixture.
    */
    public void UpdateLFCapacity()
    {
        try
        {
            LFCapacity = (int.Parse(Length) * int.Parse(Width)) + "";
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetBaseException());
        }
    }

    /*
        The UpdateTotalLF function updates the total linear feet of the fixture.
    */
    public void UpdateTotalLF()
    {
        try
        {
            if (LFCapacity != null)
            {
                TotalLF = (int.Parse(LFCapacity) * int.Parse(HangerStack)) + "";
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetBaseException());
        }
    }


    /*
        The ToString function returns the fixture in string form.
    */
    public override string ToString()
    {
        return Name + ", " +
                Length + ", " +
                Width + ", " +
                LFCapacity + ", " +
                HangerStack + ", " +
                TotalLF + ", " +
                FixtureImg;
    }
}