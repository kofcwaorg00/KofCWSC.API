using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace KofCWSC.API.Models;

public partial class TblValCouncil
{
    [DisplayName("No")]
    public int CNumber { get; set; }
    [DisplayName("City")]
    public string? CLocation { get; set; }
    [DisplayName("Council Name")]
    public string? CName { get; set; }
    [DisplayName("Dist")]
    public int? District { get; set; }

    public string? AddInfo1 { get; set; }

    public string? AddInfo2 { get; set; }

    public string? AddInfo3 { get; set; }

    public bool? LiabIns { get; set; }

    [DisplayName("Diocese")]
    public string? DioceseId { get; set; }
    [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? Chartered { get; set; }

    public string? WebSiteUrl { get; set; }

    public string? BulletinUrl { get; set; }

    public decimal? Arbalance { get; set; }

    public string Status { get; set; } = null!;

    public string? PhyAddress {  get; set; }
    public string? PhyCity { get; set; }
    public string? PhyState { get; set; }
    public string? PhyPostalCode { get; set; }
    public string? MailAddress { get; set; }
    public string? MailCity { get; set; }
    public string? MailState { get; set; }
    public string? MailPostalCode { get; set; }
    public string? MeetAddress { get; set; }
    public string? MeetCity { get; set; }
    public string? MeetState { get; set; }
    public string? MeetPostalCode { get; set; }
    public string? BMeetDOW { get; set; }
    public string? BMeetTime { get; set; }
    public string? OMeetDOW { get; set; }
    public string? OMeetTime { get; set; }
    public string? SMeetDOW { get; set; }
    public string? SMeetTime { get; set; }
}
