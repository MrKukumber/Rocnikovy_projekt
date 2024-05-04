using System;
using System.Threading;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan.MapRepreRepres;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.MapRepreConstrs;

public sealed class ElevDataDependentConstr<TTemplate, TMap, TMapRepre, TVertexAttributes, TEdgeAttributes> : 
    IElevDataDependentConstr<TTemplate, TMap, TMapRepre> 
    where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
    where TMap : IMap 
    where TMapRepre : IMapRepreWithDefinedFunctionality<TTemplate, TVertexAttributes, TEdgeAttributes>, IConstrElevDataDepMapRepre<TTemplate, TMap>, new()
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    public TTemplate UsedTemplate { get; }
    public IMapFormat<TMap> UsedMapFormat { get; }
    public bool RequiresElevData { get; } = true;
    public ElevDataDependentConstr(TTemplate usedTemplate, IMapFormat<TMap> usedMapFormat)
    {
        UsedTemplate = usedTemplate;
        UsedMapFormat = usedMapFormat;
    }
    public TMapRepre? ConstructMapRepre(TTemplate template, TMap map, ElevData elevData, IMapRepreRepresentativ<IMapRepresentation> mapRepreRep,
        IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken)
    {
        TMapRepre mapRepre = new TMapRepre()
        {
            MapRepreRep = mapRepreRep,
        };
        mapRepre.FillUp(template, map, elevData, progress, cancellationToken);
        return mapRepre;
    }
}