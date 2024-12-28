using System;
using System.Collections.Generic;

namespace KofCWSC.API.Models;

public partial class MemberSuspension
{
    public int Id { get; set; }


    public int KofCid { get; set; }

    public string? Comment { get; set; }
}
