using System;
using System.Collections.Generic;
using System.Linq;
using Optepafi.ModelViews.Converters2Vm.Graphics.MapObjects;
using Optepafi.ModelViews.Converters2Vm.Graphics.MapRepreObjects;
using Optepafi.ModelViews.Converters2Vm.Graphics.PathObjects;
using Optepafi.ModelViews.Converters2Vm.Graphics.SearchingStateObjects;

namespace Optepafi.ModelViews.Converters2Vm.Graphics;

/// <summary>
/// Static class that contains root dictionary of tree hierarchy of dictionaries that contains converters of graphic objects to VieModels.
/// 
/// Application logic works directly with this dictionary when it searches for appropriate converter.  
/// </summary>
public static class GraphicObjects2VmConverters
{
    /// <summary>
    /// Root of tree hierarchy of graphic object to ViewModel converter dictionaries. 
    /// </summary>
    public static Dictionary<Type, IGraphicObjects2VmConverter> Converters =
        new Dictionary<Type, IGraphicObjects2VmConverter>()
            .Concat(MapObjects2VmConverters.Converters)
            .Concat(PathObjects2VmConverters.Converters)
            .Concat(SearchingStateObjects2VmConverters.Converters)
            .Concat(MapRepreObjects2VmConverters.Converters)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
}