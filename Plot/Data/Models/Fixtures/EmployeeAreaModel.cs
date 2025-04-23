/*
    Filename: AddEmployeeAreaModel.cs
    Part of Project: PLOT/PLOT-FE/Plot/Data/Models/Fixtures

    TODO: File Purpose: 

    TODO: Class Purpose:
    
    Written by: Clayton Cook

*/

namespace Plot.Data.Models.Fixtures;

public record EmployeeAreaModel
{
    public int TUID { get; set; }
    public required int FLOORSET_TUID { get; set; }
    public required int X_POS { get; set; }
    public required int Y_POS { get; set; }
}