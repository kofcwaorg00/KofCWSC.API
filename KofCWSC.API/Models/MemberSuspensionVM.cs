using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace KofCWSC.API.Models;

public partial class MemberSuspensionVM
{
    public int Id { get; set; }

    [DisplayName("Member ID")]
    public int KofCid { get; set; }

    [DisplayName("Member Name")]
    public string Name { get; set; }

    public string? Comment { get; set; }
}
