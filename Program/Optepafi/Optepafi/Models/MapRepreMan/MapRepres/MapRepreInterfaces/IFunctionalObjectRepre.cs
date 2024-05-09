using Optepafi.Models.MapRepreMan.MapRepres.FunctionalityInterfaces;
using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.MapRepres.MapRepreInterfaces;



public interface IFunctionalObjectRepre<TVertexAttributes, TEdgeAttributes> :
    IObjectRepre<ITemplate<TVertexAttributes, TEdgeAttributes>>,
    IDefinedFunctionalityMapRepre<TVertexAttributes, TEdgeAttributes>,
    IPredecessorRemembering<
        IBasicEdgeCoupledPredecessorRememberingVertex<TVertexAttributes, TEdgeAttributes>, 
        IBasicEdge<IBasicEdgeCoupledPredecessorRememberingVertex<TVertexAttributes, TEdgeAttributes>, TEdgeAttributes>, 
        TVertexAttributes, TEdgeAttributes>,
    IAStarHeuristicEnsuring<IBasicEdgeCoupledPredecessorRememberingVertex<TVertexAttributes, TEdgeAttributes>, 
        TVertexAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    
}
