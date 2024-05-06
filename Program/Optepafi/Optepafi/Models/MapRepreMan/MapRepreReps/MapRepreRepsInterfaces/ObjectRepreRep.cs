using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapFormats;
using Optepafi.Models.MapMan.Maps;
using Optepafi.Models.MapRepreMan.MapRepreConstrs;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.MapRepreMan.MapRepres.MapRepreImplementations.ObjectRepres;
using Optepafi.Models.MapRepreMan.MapRepres.MapRepreInterfaces;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.MapRepreMan.MapRepreReps.MapRepreRepsInterfaces;

public class ObjectRepreRep : IMapRepreRepresentativ<IObjectRepre>
{
    public static ObjectRepreRep Instance { get; } = new ();
    private ObjectRepreRep(){}
    
    public string MapRepreName { get; } = ""; //TODO: vymysliet pekne meno
    public IMapRepreConstructor<ITemplate, IMap, IObjectRepre>[] MapRepreConstrs { get; } =
    {
        new ElevDataDependentConstr<Orienteering_ISOM_2017_2, OMAP, FunctionalObjectRepreOrienteeringOmap,Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>(Orienteering_ISOM_2017_2.Instance, OMAPFormat.Instance)
    };

    public IDefinedFunctionalityMapRepreRepresentativ<IDefinedFunctionalityMapRepre<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>
        GetDefinedFunctionalityMapRepreRepresentative<TVertexAttributes, TEdgeAttributes>()
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes
    {
        return FunctionalObjectRepreRep<TVertexAttributes, TEdgeAttributes>.Instance;
    }
}
