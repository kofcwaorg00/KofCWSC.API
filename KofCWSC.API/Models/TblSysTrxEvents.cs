using System;
using System.Collections.Generic;

namespace KofCWSC.API.Models;

public partial class TblSysTrxEvents
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime? Begin { get; set; }

    public DateTime? End { get; set; }

    public bool? isAllDay { get; set; }

    public string? AttachUrl { get; set; }

    public string? AddedBy { get; set; }

    public DateTime? DateAdded { get; set; }
}
