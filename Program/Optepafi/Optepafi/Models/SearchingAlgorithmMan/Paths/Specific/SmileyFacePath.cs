using System.Collections.Generic;
using Optepafi.Models.MapMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.Utils;

namespace Optepafi.Models.SearchingAlgorithmMan.Paths.Specific;

/// <summary>
/// Represents path generated by <c>SmileyFaceDrawer</c> algorithm.
/// 
/// It bears collection path segments which defines where smiley faces were drawn.  
/// This type (just like associated algorithm) is just demonstrative path for presenting application functionality.  
/// For more information on paths see <see cref="IPath{TVertexAttributes,TEdgeAttributes}"/>.  
/// </summary>
/// <typeparam name="TVertexAttributes">Type of vertex attributes, which can be included for extraction of information in later aggregations.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of edge attributes, which can be included for extraction of information in later aggregations.</typeparam>
public class SmileyFacePath<TVertexAttributes, TEdgeAttributes> : IPath<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{

    /// <summary>
    /// Initialization of blank smiley face path.
    /// </summary>
    public SmileyFacePath()
    {
        PathSegments = new();
    }
    /// <summary>
    /// Initialize new smiley face path instance by providing first path segments defining coordinates.
    /// </summary>
    /// <param name="legStart">First defining coordinate associated with start of some leg.</param>
    /// <param name="legFinish">Second defining coordinate associated with end of some leg.</param>
    public SmileyFacePath(MapCoordinate legStart, MapCoordinate legFinish) : this([(legStart, legFinish)]){}
    
    /// <summary>
    /// Initialize new smiley face path instance by provided list of path segments.
    /// </summary>
    /// <param name="pathSegments">List of path segments by which is instance initialized.</param>
    public SmileyFacePath(List<(MapCoordinate legStart, MapCoordinate legFinish)> pathSegments)
    {
        PathSegments = pathSegments;
    }
    /// <summary>
    /// Paths segments which defines where smiley faces were drawn.
    /// </summary>
    public List<(MapCoordinate legStart, MapCoordinate legFinish)> PathSegments { get; }
    
    ///<inheritdoc cref="IPath{TVertexAttributes,TEdgeAttributes}.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IPathGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
    {
        return genericVisitor.GenericVisit<SmileyFacePath<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>(this, otherParams);
    }
    
    /// <summary>
    /// Method for merging two smiley face paths together.
    /// 
    /// Method adds range of provided paths segments to paths segments of this instance. This method supports "fluent syntax".  
    /// </summary>
    /// <param name="smileyFacePath">Smiley face path whose segments will be merged whit local ones.</param>
    /// <returns>"This" updated by merged path.</returns>
    public SmileyFacePath<TVertexAttributes, TEdgeAttributes> MergeWith( SmileyFacePath<TVertexAttributes, TEdgeAttributes> smileyFacePath)
    {
        PathSegments.AddRange(smileyFacePath.PathSegments);
        return this;
    }
    
}