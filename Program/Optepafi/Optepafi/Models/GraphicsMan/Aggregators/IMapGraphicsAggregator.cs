using System.Collections.Generic;
using System.Threading;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.Models.Graphics.GraphicsAggregators;

public interface IMapGraphicsAggregator<in TMap> : IGraphicsAggregator
    where TMap : IMap
{
    public void AggregateGraphics(TMap map, IGraphicsObjectCollector collectorForAggregatedObjects, CancellationToken? cancellationToken);
    public GraphicsArea GetAreaOf(TMap map);
    public void AggregateGraphicsOfTrack(IList<MapCoordinate> track, IGraphicsObjectCollector collectorForAggregatedObjects);
}