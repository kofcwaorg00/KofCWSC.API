using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KofCWSC.API.Models;


public partial class FileStorageVM
{
    public int Id { get; set; }

    public string FileName { get; set; } = null!;

    public long Length { get; set; }

    public string ContentType { get; set; } = null!;
}
