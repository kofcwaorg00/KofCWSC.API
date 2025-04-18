﻿using System;
using System.Collections.Generic;

namespace KofCWSC.API.Models;

public partial class LogCorrMemberOfficeVM
{
    public int Id { get; set; }

    public string ChangeType { get; set; }

    public DateTime ChangeDate { get; set; }

    public int MemberId { get; set; }

    public int OfficeId { get; set; }
    public string OfficeDescription { get; set; }

    public int Year { get; set; }

    public bool Processed { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int KofCID { get; set; }
}
