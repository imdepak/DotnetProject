using System;
using System.Collections.Generic;

namespace MyDotnetProject.Models;

public partial class Location
{
    public int LocationId { get; set; }

    public string LocationName { get; set; } = null!;

    public int? ParentLocationId { get; set; }

    public virtual ICollection<Location> InverseParentLocation { get; set; } = new List<Location>();

    public virtual Location? ParentLocation { get; set; }
}
