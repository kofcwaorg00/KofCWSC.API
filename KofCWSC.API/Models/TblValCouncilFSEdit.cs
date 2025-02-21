﻿using Microsoft.Identity.Client;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KofCWSC.API.Models;

public partial class TblValCouncilFSEdit
{
    [DisplayName("No")]
    public int CNumber { get; set; }
    public string Status { get; set; }
    public string? CLocation { get; set; }
    public string? CName { get; set; }
    public int District { get; set; }
    public string? AddInfo1 { get; set; }
    public string? AddInfo2 { get; set; }
    public string? AddInfo3 { get; set; }

    [DisplayName("Address")]
    public string PhyAddress { get; set; } = "";

    [DisplayName("City")]
    public string PhyCity { get; set; } = "";

    [DisplayName("State")]
    public string PhyState { get; set; } = "";

    [DisplayName("Postal Code")]
    public string PhyPostalCode { get; set; } = "";

    [DisplayName("Address")]
    public string MailAddress { get; set; } = "";

    [DisplayName("City")]
    public string MailCity { get; set; } = "";

    [DisplayName("State")]
    public string MailState { get; set; } = "";

    [DisplayName("Postal Code")]
    public string MailPostalCode { get; set; } = "";

    [DisplayName("Address")]
    public string MeetAddress { get; set; } = "";

    [DisplayName("City")]
    public string MeetCity { get; set; } = "";

    [DisplayName("State")]
    public string MeetState { get; set; } = "";

    [DisplayName("Postal Code")]
    public string MeetPostalCode { get; set; } = "";

    [DisplayName("Day of Week (for example 4th Tue)")]
    public string? BMeetDOW { get; set; }

    [DisplayName("Meeting Time (for example 7:30pm)")]
    public string? BMeetTime { get; set; }

    [DisplayName("Day of Week (for example 4th Tue)")]
    public string? OMeetDOW { get; set; }

    [DisplayName("Meeting Time (for example 7:30pm)")]
    public string? OMeetTime { get; set; }

    [DisplayName("Day of Week (for example 4th Tue)")]
    public string? SMeetDOW { get; set; }

    [DisplayName("Meeting Time (for example 7:30pm)")]
    public string? SMeetTime { get; set; }

    public DateTime Updated { get; set; }
    public int UpdatedBy { get; set; }
    public string? FSAddress { get; set; }
    public string? FSCity { get; set; }
    public string? FSState { get; set; }
    public string? FSPostalCode { get; set; }
}
