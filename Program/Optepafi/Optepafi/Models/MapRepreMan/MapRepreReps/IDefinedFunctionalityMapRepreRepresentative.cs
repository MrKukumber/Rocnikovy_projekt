using System;
using System.Threading;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan.MapRepreConstrs;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.MapRepreReps;

public interface IDefinedFunctionalityMapRepreRepresentativ<out TDefinedFunctionalityMapRepresentation, TVertexAttributes, TEdgeAttributes>
    where TDefinedFunctionalityMapRepresentation : IDefinedFunctionalityMapRepre<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    
    sealed TDefinedFunctionalityMapRepresentation? CreateMapRepre<TTemplate, TMap, TMapRepre>(TTemplate template, TMap map,
        IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken, IMapRepreSourceIndic<ITemplate, IMap, TMapRepre>[] constructors)
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
        where TMap : IMap
        where TMapRepre : IMapRepresentation
    {
        foreach (var constructor in constructors)
        {
            if (constructor is IElevDataIndependentConstr<TTemplate, TMap, TDefinedFunctionalityMapRepresentation> c)
            {
                return c.ConstructMapRepre(template, map, progress, cancellationToken);
            }
        }
        return default;
    }
    
    sealed TDefinedFunctionalityMapRepresentation? CreateMapRepre<TTemplate, TMap, TMapRepre>(TTemplate template, TMap map, IElevData elevData,
        IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken, IMapRepreSourceIndic<ITemplate, IMap, TMapRepre>[] constructors)
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
        where TMap : IMap
        where TMapRepre : IMapRepresentation
    {
        foreach (var constructor in constructors)
        {
            if (constructor is IElevDataDependentConstr<TTemplate, TMap, TDefinedFunctionalityMapRepresentation> c)
            {
                return c.ConstructMapRepre(template, map, elevData, progress, cancellationToken);
            }
        }
        return default;
    }
}