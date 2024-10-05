using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KofCWSC.API.Models;

public partial class DirSupremeContacts
{
    public string GroupName { get; set; }
    public string? Prefix { get; set; }
    public string? FirstName { get; set; }
    public string? MI { get; set; }
    public string? NickName { get; set; }
    public string? LastName { get; set; }
    public string? Suffix { get; set; }
    public string? Phone {  get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? FaxNumber { get; set; }
    public string? WifesName { get; set; }
    public string? Title { get; set; }
    public int DirSortOrder { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public int ShortForm {  get; set; }
}