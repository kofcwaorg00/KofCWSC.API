using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KofCWSC.API.Models;
//[JsonConverter(typeof(JsonStringEnumConverter))]
public partial class Roster
{
    public string? State { get; set; }

    public int? Co { get; set; }

    public int? Mbr { get; set; }

    public int? Asm { get; set; }

    public string? F5 { get; set; }

    public DateTime? Dob { get; set; }

    public DateTime? _1st { get; set; }

    public string? _2nd { get; set; }

    public string? _3rd { get; set; }

    public DateTime? _4th { get; set; }

    public double? YrsOfService { get; set; }

    public string? ReEntryDate { get; set; }

    public string? F13 { get; set; }

    public string? F14 { get; set; }

    public string? F15 { get; set; }

    public string? F16 { get; set; }

    public string? F17 { get; set; }

    public string? F18 { get; set; }

    public double? F19 { get; set; }

    public string? F20 { get; set; }

    public string? F21 { get; set; }

    public string? F22 { get; set; }
}


