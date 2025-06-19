﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KofCWSC.API.Models;

public partial class AspNetUser
{
    public string Id { get; set; } = null!;
    [Display(Name = "User Name")]
    public string? UserName { get; set; }

    public string? NormalizedUserName { get; set; }

    public string? Email { get; set; }

    public string? NormalizedEmail { get; set; }
    [Display(Name = "Email Confirmed")]
    public bool EmailConfirmed { get; set; }

    public string? PasswordHash { get; set; }

    public string? SecurityStamp { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public string? PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = null!;
    [Display(Name = "Member ID")]
    public int KofCmemberId { get; set; }
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = null!;

    public string? ProfilePictureUrl { get; set; }

    //[ForeignKey("KofCID")]
    //public virtual TblMasMember MasMember { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? Wife {  get; set; }
    public int? Council {  get; set; }
    [Display(Name = "Member Verified")]
    public bool MemberVerified { get; set; }
    [Display(Name = "Membership Card URL")]
    public string? MembershipCardUrl { get; set; }

}
