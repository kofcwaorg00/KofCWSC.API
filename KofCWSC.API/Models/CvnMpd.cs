using System;
using System.Collections.Generic;

namespace KofCWSC.API.Models;

public partial class CvnMpd
{
    public int Id { get; set; }

    public int MemberId { get; set; }

    public int Council { get; set; }

    public int District { get; set; }

    public string Group { get; set; } = null!;

    public string Office { get; set; } = null!;

    public string Payee { get; set; } = null!;

    public int? CheckNumber { get; set; }

    public DateOnly? CheckDate { get; set; }

    public bool? Day1D1 { get; set; }

    public bool? Day2D1 { get; set; }

    public bool? Day3D1 { get; set; }

    public bool? Day1D2 { get; set; }

    public bool? Day2D2 { get; set; }

    public bool? Day3D2 { get; set; }

    public string? Day1GD1 { get; set; }

    public string? Day2GD1 { get; set; }

    public string? Day3GD1 { get; set; }

    public string? Day1GD2 { get; set; }

    public string? Day2GD2 { get; set; }

    public string? Day3GD2 { get; set; }

    public int? Miles { get; set; }

    public decimal? CheckTotal { get; set; }

    public string Location { get; set; } = null!;
    public bool PayMe { get; set; }
    public string? CouncilStatus { get; set; }
    public int GroupID { get; set; }
    public string? CouncilLocation { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }
    public string? Memo { get; set; }
}
