using System.Threading;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.ReportMan.Reports;
using Optepafi.Models.SearchingAlgorithmMan.Paths;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.Models.UserModelMan.UserModels.Functionalities;

namespace Optepafi.Models.ReportMan.Aggregators;

/// <summary>
/// Represents aggregator of reports for paths of specific type. The main task of aggregator is to assemble corresponding path report and return it.
/// 
/// Algorithms can not by default extract information from vertex and edge attributes. They are therefore able to generate paths which contains only non processed vertex/edge attributes.  
/// For this reason path report aggregator will also receive corresponding user model, which may be able to extract necessary information from attributes contained in provided path.  
/// The word "may" is important one. No one will ensure, that user model is able to deliver some service. Aggregator should be prepared for user model not containing some functionality and simply not include information dependent on it in aggregated report.  
/// If generated path report should contain some graphics (and generally it will) that is going to be shown to user, <see cref="GraphicsSubManager{TVertexAttributes,TEdgeAttributes}"/> can be used for acquiring of appropriate graphics.  
/// </summary>
/// <typeparam name="TPath">Type of path for which report is aggregated.</typeparam>
/// <typeparam name="TVertexAttributes">Type of vertex attributes which path can contain and user model can use for computing.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of edge attributes which path can contain and user model can use for computing.</typeparam>
public interface IPathReportAggregator<in TPath, TVertexAttributes, TEdgeAttributes> : IReportAggregator
    where TPath : IPath<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    /// <summary>
    /// Aggregates report of provided path.
    /// 
    /// It can ask inserted user model for some information extraction service on vertex/edge attributes. Although user model is not obligated to provide such service. For more on this topi see <see cref="IPathReportAggregator{TSearchingState,TVertexAttributes,TEdgeAttributes}"/>.  
    /// For incidental aggregation of path graphics for report <see cref="GraphicsSubManager{TVertexAttributes,TEdgeAttributes}"/> can be used.  
    /// </summary>
    /// <param name="path">Path which report should be aggregated.</param>
    /// <param name="userModel">User model which can be asked for some information extraction.</param>
    /// <param name="cancellationToken">Cancellation token for cancelling of aggregation.</param>
    /// <returns></returns>
    IPathReport AggregateReport(TPath path, IComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel, CancellationToken? cancellationToken = null);
}