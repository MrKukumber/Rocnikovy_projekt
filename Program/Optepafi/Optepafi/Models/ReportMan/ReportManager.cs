using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Optepafi.Models.ReportMan.Aggregators;
using Optepafi.Models.ReportSubMan.Aggregators;
using Optepafi.Models.SearchingAlgorithmMan.Paths;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.ViewModels.Data.Reports;

namespace Optepafi.Models.ReportMan;

public class ReportManager : 
    ITemplateGenericVisitor<(ReportManager.AggregationResult, IPathReport?), (IPath, IUserModel, CancellationToken?)>,
    IPathGenericVisitor<(ReportManager.AggregationResult, IPathReport?), (IUserModel, CancellationToken?)>

{
    public static ReportManager Instance { get; } = new();
    private ReportManager(){}
    
    public enum AggregationResult {Aggregated, NonGenericPath, NotUsableUserModel, NoUsableAggregator}
    
    public AggregationResult AggregatePathReport(
        IPath path, IUserModel userModel, ITemplate template, out IPathReport? pathReport, CancellationToken? cancellationToken = null)
    {
        var (aggregationResult, report) = template.AcceptGeneric(this, (path, userModel, cancellationToken));
        pathReport = report;
        return aggregationResult;
    }

    (AggregationResult, IPathReport?) ITemplateGenericVisitor<(AggregationResult, IPathReport?), (IPath, IUserModel, CancellationToken?)>.GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes>(TTemplate template,
        (IPath, IUserModel, CancellationToken?) otherParams)
    {
        var (path, userModel, cancellationToken) = otherParams;
        if (path is IPath<TVertexAttributes, TEdgeAttributes> p)
        {
            return p.AcceptGeneric(this, (userModel, cancellationToken));
        }
        return (AggregationResult.NonGenericPath, null);
    }

    (AggregationResult, IPathReport?) IPathGenericVisitor<(AggregationResult, IPathReport?), (IUserModel, CancellationToken?)>.GenericVisit<TPath, TVertexAttributes, TEdgeAttributes>(TPath path,
        (IUserModel, CancellationToken?) otherParams)
    {
        var (userModel, cancellationToken) = otherParams;
        if (userModel is IUsableUserModel<TVertexAttributes, TEdgeAttributes> usableUserModel)
        {
            var pathReport = ReportSubManager<TVertexAttributes, TEdgeAttributes>.Instance.AggregatePathReport(path, usableUserModel, cancellationToken);
            if (pathReport is not null) return (AggregationResult.Aggregated, pathReport);
            return (AggregationResult.NoUsableAggregator, pathReport);
        }
        return (AggregationResult.NotUsableUserModel, null);
    }
}