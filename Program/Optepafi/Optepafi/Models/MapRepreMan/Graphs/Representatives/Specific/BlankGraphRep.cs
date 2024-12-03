using System;
using System.Threading;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.Graphs.Specific;
using Optepafi.Models.MapRepreMan.Implementations.Representatives;
using Optepafi.Models.MapRepreMan.Implementations.Representatives.Specific.Blank;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.SearchingAlgorithmMan;
using Optepafi.Models.SearchingAlgorithmMan.SearchingAlgorithms;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModelReps;
using Optepafi.Models.UserModelMan.UserModels.Functionalities;
using Optepafi.Models.Utils;

namespace Optepafi.Models.MapRepreMan.Graphs.Representatives.Specific;

/// <summary>
/// Singleton class which represents blank graph.
/// 
/// Its instance is contained in <c>BlankRepreRep</c> so it can be used for creation of represented map representation/graph. 
/// </summary>
public class BlankGraphRep<TVertexAttributes, TEdgeAttributes> : GraphRepresentative<IBlankGraph<TVertexAttributes, TEdgeAttributes>, IBlankGraph<TVertexAttributes, TEdgeAttributes>.Vertex, IBlankGraph<TVertexAttributes, TEdgeAttributes>.Edge>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    public static BlankGraphRep<TVertexAttributes, TEdgeAttributes> Instance { get; } = new();
    private BlankGraphRep() { }
    
    ///<inheritdoc cref="GraphRepresentative{TGraph,TVertex,TEdge}.CreateableImplementationsIndicators"/>
    public override IImplementationIndicator<ITemplate, IMap, IMapRepre>[] CreateableImplementationsIndicators { get; } = [BlankElevDataDepBlankTemplateTextMapImplementationRep.Instance, BlankElevDataIndepBlankTemplateOmapMapImplementationRep.Instance];

    public override bool RevelationForSearchingAlgorithmMan<TUserModelVertexAttributes, TUserModelEdgeAttributes>(SearchingAlgorithmManager searchingAlgorithmMan, IUserModelType<IComputing<ITemplate<TUserModelVertexAttributes, TUserModelEdgeAttributes>, TUserModelVertexAttributes, TUserModelEdgeAttributes>, ITemplate<TUserModelVertexAttributes, TUserModelEdgeAttributes>> userModelType, ISearchingAlgorithm algorithm) => searchingAlgorithmMan.AcceptGraphCreatorsRevelation(this, userModelType, algorithm);

}