using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Optepafi.Models.ElevationDataMan;

public interface IRegion
{
    public string Name { get; }
    public IRegion[] SubRegions { get; }
    public List<GeoCoordinate>? Geometry { get => null; }
    public bool IsDownloaded { get; set; }   
}