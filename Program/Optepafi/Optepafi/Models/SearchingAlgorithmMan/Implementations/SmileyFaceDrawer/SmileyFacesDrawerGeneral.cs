using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.Graphs.Representatives;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.MapRepreMan.VerticesAndEdges;
using Optepafi.Models.ReportMan;
using Optepafi.Models.ReportMan.Reports;
using Optepafi.Models.SearchingAlgorithmMan.Paths;
using Optepafi.Models.SearchingAlgorithmMan.Paths.Specific;
using Optepafi.Models.SearchingAlgorithmMan.SearchingStates.Specific;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModelReps;
using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.Models.UserModelMan.UserModels.Functionalities;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Configurations;

namespace Optepafi.Models.SearchingAlgorithmMan.Implementations.SmileyFaceDrawer;

/// <summary>
/// Basic implementation of <c>SmileyFaceDrawer</c> algorithm.
///
/// It simulates drawing of bunch of smiley faces according to provided track.  
/// It report progress of drawing an in the end returns path which defines positions of "drawn" smiley faces.  
/// This type is just demonstrative algorithm implementation for presenting application functionality.  
/// For more information on implementations of searching algorithms see <see cref="ISearchingAlgorithmImplementation{TConfiguration}"/>.  
/// </summary>
public class SmileyFacesDrawerGeneral : ISearchingAlgorithmImplementation<NullConfiguration>
{
    public static SmileyFacesDrawerGeneral Instance { get; } = new();
    private SmileyFacesDrawerGeneral(){}

    /// <inheritdoc cref="ISearchingAlgorithmImplementISearchingAlgorithmImplementation{TConfiguration}"/>
    /// <remarks>
    /// Used graph does not have to provide no special functionality.
    /// </remarks>
    public bool DoesRepresentUsableGraph<TVertex, TEdge, TVertexAttributes, TEdgeAttributes>(
        IGraphRepresentative<IGraph<TVertex, TEdge>, TVertex, TEdge> graphRepresentative) where TVertex : IVertex where TEdge : IEdge where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
    {
        if (graphRepresentative is IGraphRepresentative<IGraph<TVertex, TEdge>, TVertex, TEdge>) 
            return true;
        return false;
    }

    /// <inheritdoc cref="ISearchingAlgorithmImplementISearchingAlgorithmImplementation{TConfiguration}l{TVertexAttributes,TEdgeAttributes}"/>
    /// <remarks>
    /// Used user model does not have to provide no special functionality.
    /// </remarks>
    public bool DoesRepresentUsableUserModel<TTemplate, TVertexAttributes, TEdgeAttributes>(IUserModelType<IComputing<TTemplate, TVertexAttributes, TEdgeAttributes>, TTemplate> userModelType)
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes> where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
    {
        if (userModelType is IUserModelType<IComputing<TTemplate, TVertexAttributes, TEdgeAttributes>, TTemplate>)
            return true;
        return false;
    }

    
    /// <inheritdoc cref="ISearchingAlgorithmImplementISearchingAlgorithmImplementation{TConfiguration}utes,TEdgeAttributes}"/>
    /// <remarks>
    /// Used graph does not have to provide no special functionality.
    /// </remarks>
    public bool IsUsableGraph<TVertex, TEdge, TVertexAttributes, TEdgeAttributes>(IGraph<TVertex, TEdge> graph) where TVertex : IVertex where TEdge : IEdge where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
    {
        if (graph is IGraph<TVertex, TEdge>)
            return true;
        return false;
    }


    /// <inheritdoc cref="ISearchingAlgorithmImplementISearchingAlgorithmImplementation{TConfiguration}tributes,TEdgeAttributes}"/>
    /// <remarks>
    /// Used user models do not have to provide no special functionality.
    /// </remarks>
    public bool IsUsableUserModel<TVertexAttributes, TEdgeAttributes>(IUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>> userModel) where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
    {
        if (userModel is IComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>)
            return true;
        return false;
    }

    /// <inheritdoc cref="ISearchingAlgorithmImplementISearchingAlgorithmImplementation{TConfiguration}butes,TEdgeAttributes}"/>
    /// <remarks>
    /// If more then one user model is provided, path is computed only with the first one and resulting path is then returned multiple times according to count of user models.  
    /// It does not matter because drawing of smiley faces does not depend of user model.  
    /// Searching is done using <c>ExecutorSearch</c> method.  
    /// </remarks>
    public IPath<TVertexAttributes, TEdgeAttributes>[] SearchForPaths<TVertexAttributes, TEdgeAttributes, TVertex, TEdge>(Leg[] track, IGraph<TVertex, TEdge> graph, IList<IUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>>> userModels,
        NullConfiguration configuration, IProgress<ISearchingReport>? progress, CancellationToken? cancellationToken) where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes where TVertex : IVertex where TEdge : IEdge
    {
        if (userModels.Count == 0) return [];
        var drawnFacePaths = ExecutorSearch(track, graph, userModels[0], configuration, progress, cancellationToken);
        return Enumerable.Repeat(drawnFacePaths, userModels.Count).ToArray();
    }

    /// <inheritdoc cref="ISearchingAlgorithmImplementISearchingAlgorithmImplementation{TConfiguration}butes,TEdgeAttributes}"/>
    /// <remarks>
    /// Drawing is performed by cycle when in each iteration one smiley face is drawn for specified leg.  
    /// Drawing progress is continuously reported so that it could be shown to user.  
    /// By refreshing to new searching state after each iteration we achieve showing only objects of currently drawn smiley face.  
    /// During drawing the path is continuously constructed and in the end it is returned as result.  
    /// </remarks>
    public IPath<TVertexAttributes, TEdgeAttributes> ExecutorSearch<TVertexAttributes, TEdgeAttributes, TVertex, TEdge>(Leg[] track, IGraph<TVertex, TEdge> graph,
        IUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>> userModel, NullConfiguration configuration, IProgress<ISearchingReport>? progress, CancellationToken? cancellationToken) where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes where TVertex : IVertex where TEdge : IEdge
    {
        int drawingDuration = 500; SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes>.SmileyFaceObject[] allFacialObjectsExceptLeftEye =
        [
            SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes>.SmileyFaceObject.RightEye,
            SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes>.SmileyFaceObject.Nose,
            SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes>.SmileyFaceObject.Mouth
        ];
        SmileyFacePath<TVertexAttributes, TEdgeAttributes> drawnFacePaths = new SmileyFacePath<TVertexAttributes, TEdgeAttributes>();
        for(int i = 0; i < track.Length; i++)
        {
            Thread.Sleep(drawingDuration/2);
            if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) return drawnFacePaths;
            Thread.Sleep(drawingDuration/2);
            if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) return drawnFacePaths;
            SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes> drawingState = new SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes>(SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes>.SmileyFaceObject.LeftEye, track[i], i+1);
            ISearchingReport? report =  ReportSubManager<TVertexAttributes, TEdgeAttributes>.Instance.AggregateSearchingReport(drawingState, userModel);
            if(report is not null && progress is not null) progress.Report(report);
            foreach (var smileyFaceObject in allFacialObjectsExceptLeftEye)
            {
                Thread.Sleep(drawingDuration/2);
                if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) return drawnFacePaths;
                Thread.Sleep(drawingDuration/2);
                if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) return drawnFacePaths;
                drawingState.Add(smileyFaceObject, track[i], i+1);
                report = ReportSubManager<TVertexAttributes, TEdgeAttributes>.Instance.AggregateSearchingReport(drawingState, userModel);
                if(report is not null && progress is not null) progress.Report(report);
            }
            drawnFacePaths.MergeWith( new SmileyFacePath<TVertexAttributes, TEdgeAttributes>(track[i].Start, track[i].Finish));
        }
        Thread.Sleep(drawingDuration/2);
        return drawnFacePaths;
    }
}