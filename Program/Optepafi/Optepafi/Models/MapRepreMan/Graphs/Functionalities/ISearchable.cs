using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.Utils;

namespace Optepafi.Models.MapRepreMan.Graphs.Functionalities;

/// <summary>
/// Represents searchable graph. 
/// </summary>
/// <typeparam name="TVertex">Type of vertices used by graph.</typeparam>
/// <typeparam name="TEdge">Type of edges used by graph.</typeparam>
/// <typeparam name="TVertexAttributes">Type of attributes used in vertices of a graph.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of attributes used in edges of a graph.</typeparam>
public interface ISearchable<out TVertex, out TEdge, out  TVertexAttributes, out TEdgeAttributes>
    where TVertex : IBasicVertex<TEdge, TVertexAttributes>
    where TEdge : IBasicEdge<TVertex, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    /// <summary>
    /// Returns vertex of graph which corresponds to provided location <see cref="MapCoordinates"/>.
    /// </summary>
    /// <param name="coords">Coordinate for which vertex is returned.</param>
    /// <returns>Corresponding vertex for provided <c>MapCoordinate</c>.</returns>
    TVertex GetVertexFor(MapCoordinates coords);
}

