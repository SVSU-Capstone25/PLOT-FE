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
    // Tristan Calay 4/4/25
    // Added tracking for notes and category.
    public string selectedSuperCategory { get; set; } = string.Empty;
    public string selectedSubCategory { get; set; } = string.Empty;
    public string notes { get; set; } = string.Empty;

    public static SortedDictionary<string, Fixture> fixtures = new()
    {
        ["Single Hung Rack"] = new Fixture("Single Hung Rack", "5", "2", "1", "img/LayoutPhotoExample.png"),
        ["Double Hung Rack"] = new Fixture("Double Hung Rack", "3", "2", "2", "img/LayoutPhotoExample.png"),
        ["Shelf"] = new Fixture("Shelf", "2", "1", "1", "img/LayoutPhotoExample.png"),

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
                selectedSuperCategory + " " + selectedSubCategory + ", " +
                notes + ", " +
                FixtureImg;
    }
}