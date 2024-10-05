using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KofCWSC.API.Models;

public partial class DirMain
{
    public string GroupName { get; set; }
    public string? ReportTitle { get; set; }
    public string? ReportSubTitle { get; set; }
    public int ShortForm {  get; set; }
    public string? FullName { get; set; }
    public string? Phone {  get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? Email { get; set; }
    public string? Title { get; set; }
    public int DirSortOrder { get; set; }
}