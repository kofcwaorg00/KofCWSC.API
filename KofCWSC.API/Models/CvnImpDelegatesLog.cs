using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KofCWSC.API.Models;

public partial class CvnImpDelegatesLog
{
    [Key]
    public int Id { get; set; }

    public Guid? Guid { get; set; }

    public DateTime? Rundate { get; set; }

    public string? Type { get; set; }

    public int? MemberId { get; set; }

    public string? Data { get; set; }
}
