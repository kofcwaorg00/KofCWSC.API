using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KofCWSC.API.Models;

public partial class GetLabelByOffice
{
    public int District { get; set; }
    public string? AltOfficeDescription { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public int Council { get; set; }
    public int? Assembly { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? OfficeDescription { get; set; }
    public int OfficeID { get; set; }
    public string? CouncilName { get; set; }
    public string? FullName { get; set; }
    public string? CSZ { get; set; }
}