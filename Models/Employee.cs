using System;
using System.Collections.Generic;

namespace MyDotnetProject.Models;

public partial class Employee
{
    public long Id { get; set; }

    public string? Email { get; set; }

    public string EmpName { get; set; } = null!;
}
