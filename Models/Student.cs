using System;
using System.Collections.Generic;

namespace MyDotnetProject.Models;

public partial class Student
{
    public long StudentId { get; set; }

    public string? StudentEmail { get; set; }

    public string? StudentName { get; set; }
}
