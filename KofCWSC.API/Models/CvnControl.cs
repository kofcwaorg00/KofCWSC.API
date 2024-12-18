using System;
using System.Collections.Generic;

namespace KofCWSC.API.Models;

public partial class CvnControl
{
    public int Id { get; set; }

    public string LocationString { get; set; } = null!;

    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
}
