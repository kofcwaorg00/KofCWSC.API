﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KofCWSC.API.Models;

public partial class TblCorrMemberOfficeVM
{
    [Key]
    public int Id { get; set; }

    public int MemberId { get; set; }

    public int OfficeId { get; set; }
    
    public string OfficeDescription { get; set; }
    public bool PrimaryOffice { get; set; }

    public int? Year { get; set; }

}
