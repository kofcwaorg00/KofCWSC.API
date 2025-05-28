﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace KofCWSC.API.Models
{
    [Keyless]
    public class SPGetChairmen
    {
        [Display(Name = "Director/Chairman")]
        public string? Chairmanship { get; set; }
        [Display(Name = "Name")]
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public int Council { get; set; }

        public string? Email2 { get; set; }
        public int MemberID { get; set; }
        public string? Heading { get; set; }
        public string? Photo { get; set; }
        public int? KofCID { get; set; }
    }
}
