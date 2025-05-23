/*
    Filename: FixtureModel.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Fixtures

    File Purpose:
    This file contains the object model for the model
    of a fixture.
    
    Class Purpose:
    This record is used as the main model
    for the model of a fixture in the application. This will
    look the same as the schema in the database.

    Written by: Jordan Houlihan
*/

namespace Plot.Data.Models.Fixtures;

public class FixtureModel
{
    public required int TUID { get; set; }
    public string? NAME { get; set; }
    public int? WIDTH { get; set; }
    public int? LENGTH { get; set; }
    public required int LF_CAP { get; set; }
    public int? STORE_TUID { get; set; }
    public byte[]? ICON { get; set; }
}