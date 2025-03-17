﻿using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KofCWSC.API.Models;

public partial class CvnImpDelegate
{
    [Display(Name = "Submitter Date")]
    public DateTime? SubmissionDate { get; set; }

    [Display(Name = "Submitter Email")]
    public string? FormSubmitterSEmail { get; set; }
    [Display(Name = "Council Name")]
    public string? CouncilName { get; set; }

    [Display(Name = "Council #")]
    public int? CouncilNumber { get; set; }
    [Display(Name = "Imported First Name")]
    public string? D1FirstName { get; set; }
    [Display(Name = "Imported Middle Name")]
    public string? D1MiddleName { get; set; }
    [Display(Name = "Imported Last Name")]
    public string? D1LastName { get; set; }
    [Display(Name = "Imported Suffix")]
    public string? D1Suffix { get; set; }
    [Display(Name = "Imported KofC ID")]
    public int? D1MemberID { get; set; }
    [Display(Name = "Imported Address")]
    public string? D1Address1 { get; set; }
    [Display(Name = "Imported Address 2")]
    public string? D1Address2 { get; set; }
    [Display(Name = "Imported City")]
    public string? D1City { get; set; }
    [Display(Name = "Imported State")]
    public string? D1State { get; set; }
    [Display(Name = "Imported Zip")]
    public string? D1ZipCode { get; set; }
    [Display(Name = "Imported Phone")]
    public string? D1Phone { get; set; }
    [Display(Name = "Imported Email")]
    public string? D1Email { get; set; }
    [Display(Name = "Imported First Name")]
    public string? D2FirstName { get; set; }
    [Display(Name = "Imported Middle Name")]
    public string? D2MiddleName { get; set; }
    [Display(Name = "Imported Last Name")]
    public string? D2LastName { get; set; }
    [Display(Name = "Imported Suffix")]
    public string? D2Suffix { get; set; }
    [Display(Name = "Imported KofC ID")]
    public int? D2MemberID { get; set; }
    [Display(Name = "Imported Address")]
    public string? D2Address1 { get; set; }
    [Display(Name = "Imported Address 2")]
    public string? D2Address2 { get; set; }
    [Display(Name = "Imported City")]
    public string? D2City { get; set; }
    [Display(Name = "Imported State")]
    public string? D2State { get; set; }
    [Display(Name = "Imported Zip")]
    public string? D2ZipCode { get; set; }
    [Display(Name = "Imported Phone")]
    public string? D2Phone { get; set; }
    [Display(Name = "Imported Email")]
    public string? D2Email { get; set; }
    [Display(Name = "Imported First Name")]
    public string? A1FirstName { get; set; }
    [Display(Name = "Imported Middle Name")]
    public string? A1MiddleName { get; set; }
    [Display(Name = "Imported Last Name")]
    public string? A1LastName { get; set; }
    [Display(Name = "Imported Suffix")]
    public string? A1Suffix { get; set; }
    [Display(Name = "Imported KofC ID")]
    public int? A1MemberID { get; set; }
    [Display(Name = "Imported Address")]
    public string? A1Address1 { get; set; }
    [Display(Name = "Imported Address 2")]
    public string? A1Address2 { get; set; }
    [Display(Name = "Imported City")]
    public string? A1City { get; set; }
    [Display(Name = "Imported State")]
    public string? A1State { get; set; }
    [Display(Name = "Imported Zip")]
    public string? A1ZipCode { get; set; }
    [Display(Name = "Imported Phone")]
    public string? A1Phone { get; set; }
    [Display(Name = "Imported Email")]
    public string? A1Email { get; set; }
    [Display(Name = "Imported First Name")]
    public string? A2FirstName { get; set; }
    [Display(Name = "Imported Middle Name")]
    public string? A2MiddleName { get; set; }
    [Display(Name = "Imported Last Name")]
    public string? A2LastName { get; set; }
    [Display(Name = "Imported Suffix")]
    public string? A2Suffix { get; set; }
    [Display(Name = "Imported KofC ID")]
    public int? A2MemberID { get; set; }
    [Display(Name = "Imported Address")]
    public string? A2Address1 { get; set; }
    [Display(Name = "Imported Address 2")]
    public string? A2Address2 { get; set; }
    [Display(Name = "Imported City")]
    public string? A2City { get; set; }
    [Display(Name = "Imported State")]
    public string? A2State { get; set; }
    [Display(Name = "Imported Zip")]
    public string? A2ZipCode { get; set; }
    [Display(Name = "Imported Phone")]
    public string? A2Phone { get; set; }
    [Display(Name = "Imported Email")]
    public string? A2Email { get; set; }

    [Key]
    public int Id { get; set; }
    public string? Validation { get; set; }
    public string? RecType { get; set; }
    [Display(Name = "Existing Council")]
    public int? ED1Council { get; set; }
    [Display(Name = "Existing First Name")]
    public string? ED1FirstName { get; set; }
    [Display(Name = "Existing Middle Name")]
    public string? ED1MiddleName { get; set; }
    [Display(Name = "Existing Last Name")]
    public string? ED1LastName { get; set; }
    [Display(Name = "Existing Suffix")]
    public string? ED1Suffix { get; set; }
    [Display(Name = "Existing KofC ID")]
    public int? ED1MemberID { get; set; }
    [Display(Name = "Existing Address")]
    public string? ED1Address1 { get; set; }
    [Display(Name = "Existing Address 2")]
    public string? ED1Address2 { get; set; }
    [Display(Name = "Existing City")]
    public string? ED1City { get; set; }
    [Display(Name = "Existing State")]
    public string? ED1State { get; set; }
    [Display(Name = "Existing Zip")]
    public string? ED1ZipCode { get; set; }
    [Display(Name = "Existing Phone")]
    public string? ED1Phone { get; set; }
    [Display(Name = "Existing Email")]
    public string? ED1Email { get; set; }
    [Display(Name = "Existing Council")]
    public int? ED2Council { get; set; }

    [Display(Name = "Existing First Name")]
    public string? ED2FirstName { get; set; }
    [Display(Name = "Existing Middle Name")]
    public string? ED2MiddleName { get; set; }
    [Display(Name = "Existing Last Name")]
    public string? ED2LastName { get; set; }
    [Display(Name = "Existing Suffix")]
    public string? ED2Suffix { get; set; }
    [Display(Name = "Existing KofC ID")]
    public int? ED2MemberID { get; set; }
    [Display(Name = "Existing Address")]
    public string? ED2Address1 { get; set; }
    [Display(Name = "Existing Address 2")]
    public string? ED2Address2 { get; set; }
    [Display(Name = "Existing City")]
    public string? ED2City { get; set; }
    [Display(Name = "Existing State")]
    public string? ED2State { get; set; }
    [Display(Name = "Existing Zip")]
    public string? ED2ZipCode { get; set; }
    [Display(Name = "Existing Phone")]
    public string? ED2Phone { get; set; }
    [Display(Name = "Existing Email")]
    public string? ED2Email { get; set; }
    [Display(Name = "Existing Council")]
    public int? EA1Council { get; set; }

    [Display(Name = "Existing First Name")]
    public string? EA1FirstName { get; set; }
    [Display(Name = "Existing Middle Name")]
    public string? EA1MiddleName { get; set; }
    [Display(Name = "Existing Last Name")]
    public string? EA1LastName { get; set; }
    [Display(Name = "Existing Suffix")]
    public string? EA1Suffix { get; set; }
    [Display(Name = "Existing KofC ID")]
    public int? EA1MemberID { get; set; }
    [Display(Name = "Existing Address")]
    public string? EA1Address1 { get; set; }
    [Display(Name = "Existing Address 2")]
    public string? EA1Address2 { get; set; }
    [Display(Name = "Existing City")]
    public string? EA1City { get; set; }
    [Display(Name = "Existing State")]
    public string? EA1State { get; set; }
    [Display(Name = "Existing Zip")]
    public string? EA1ZipCode { get; set; }
    [Display(Name = "Existing Phone")]
    public string? EA1Phone { get; set; }
    [Display(Name = "Existing Email")]
    public string? EA1Email { get; set; }
    [Display(Name = "Existing Council")]
    public int? EA2Council { get; set; }

    [Display(Name = "Existing First Name")]
    public string? EA2FirstName { get; set; }
    [Display(Name = "Existing Middle Name")]
    public string? EA2MiddleName { get; set; }
    [Display(Name = "Existing Last Name")]
    public string? EA2LastName { get; set; }
    [Display(Name = "Existing Suffix")]
    public string? EA2Suffix { get; set; }
    [Display(Name = "Existing KofC ID")]
    public int? EA2MemberID { get; set; }
    [Display(Name = "Existing Address")]
    public string? EA2Address1 { get; set; }
    [Display(Name = "Existing Address 2")]
    public string? EA2Address2 { get; set; }
    [Display(Name = "Existing City")]
    public string? EA2City { get; set; }
    [Display(Name = "Existing State")]
    public string? EA2State { get; set; }
    [Display(Name = "Existing Zip")]
    public string? EA2ZipCode { get; set; }
    [Display(Name = "Existing Phone")]
    public string? EA2Phone { get; set; }
    [Display(Name = "Existing Email")]
    public string? EA2Email { get; set; }
}
