using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Shapes.Segments;

namespace Optepafi.Models.MapMan.Maps;

//TODO: comment
public abstract class OmapMap : IMap, IPartitionableMap
{
    //TODO: implement
    public string FileName { get; init; }
    public required string FilePath { get; init; }
    
    public virtual TOut AcceptGeneric<TOut, TOtherParams>(IMapGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
    { return genericVisitor.GenericVisit(this, otherParams); }
    public virtual TOut AcceptGeneric<TOut>(IMapGenericVisitor<TOut> genericVisitor)
    { return genericVisitor.GenericVisit(this); }
    
    
    public abstract int Scale { get; }
    public abstract Dictionary<decimal, List<Object>> Objects { get; }
    public abstract List<Symbol> Symbols { get; }
    public record struct Symbol(decimal Code);

    // public record struct Object((MapCoordinates coords, byte type)[] TypedCoords, float SymbolRotation)
    public struct Object
    {
        public (MapCoordinates coords, byte type)[] TypedCoords { get; set; }

        public float SymbolRotation { get; set; }
        public Object((MapCoordinates coords, byte type)[] typedCoords, float symbolRotation)
        {
            TypedCoords = typedCoords;
            SymbolRotation = symbolRotation;
            if (typedCoords is null)
            {
                Console.WriteLine();
            }
        }
        public List<Segment> CollectSegments(bool asPolygon)
        {
            List<Segment> collectedSegments = new();
            if (TypedCoords.Length == 1)
            {
                collectedSegments.Add(new LineSegment(TypedCoords[0].coords));
                return collectedSegments;
            }
            int i = 0;
            while(i < TypedCoords.Length)
            {
                switch (TypedCoords[i].type % 32)
                {
                    case 0:
                        if (i + 1 >= TypedCoords.Length) { i = TypedCoords.Length; break; }
                        collectedSegments.Add(ResolveLineSegment(TypedCoords[i].coords, TypedCoords[++i].coords));
                        break;
                    case 1:
                        if (i + 3 >= TypedCoords.Length) { i = TypedCoords.Length; break; }
                        collectedSegments.Add(ResolveCubicBezierSegment(TypedCoords[i].coords, TypedCoords[++i].coords, TypedCoords[++i].coords, TypedCoords[++i].coords));
                        break;
                    case 2:
                    case 16:    
                    case 18:
                        i = TypedCoords.Length; break;
                    default:
                        i = TypedCoords.Length; break;
                }
            }
            if (asPolygon && TypedCoords[0].coords != TypedCoords.Last().coords)
                collectedSegments.Add(new LineSegment(TypedCoords[0].coords));
            return collectedSegments;
        }
        
        private Segment ResolveLineSegment(MapCoordinates point0, MapCoordinates point1)
            => point0 != point1 ? new LineSegment(point1) : new LineSegment(point1 + new MapCoordinates(1,1)) ;
        private Segment ResolveCubicBezierSegment(MapCoordinates point0, MapCoordinates point1, MapCoordinates point2, MapCoordinates point3)
        {
           if (point0 != point1 && point1 != point2 && point2 != point3) return new CubicBezierCurveSegment(point1, point2, point3); 
           if (point0 == point1 && point1 != point2 && point2 != point3) return new QuadraticBezierCurveSegment(point2, point3);
           if (point0 != point1 && point1 == point2 && point2 != point3) return new QuadraticBezierCurveSegment(point1, point3);
           if (point0 != point1 && point1 != point2 && point2 == point3) return new QuadraticBezierCurveSegment(point1, point2);
           return ResolveLineSegment(point0, point3);
        }
    }
    
    public abstract MapCoordinates NorthernmostCoords { get; }
    public abstract MapCoordinates SouthernmostCoords { get; }
    public abstract MapCoordinates WesternmostCoords { get; }
    public abstract MapCoordinates EasternmostCoords { get; }

    public abstract IMap GetPartitionOfSize(int size, CancellationToken? cancellationToken, out bool wholeMapReturned);
}

public abstract class GeoLocatedOmapMap : OmapMap, IMostNSWECoordQueryableGeoRefMap
{
    //TODO: implement
    public abstract GeoCoordinates RepresentativeLocation { get; }
    public TOut AcceptGeneric<TOut, TOtherParams>(IGeoLocatedMapGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
    { return genericVisitor.GenericVisit(this, otherParams); }
    public override TOut AcceptGeneric<TOut, TOtherParams>(IMapGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
    { return genericVisitor.GenericVisit(this, otherParams); }
    public override TOut AcceptGeneric<TOut>(IMapGenericVisitor<TOut> genericVisitor)
    { return genericVisitor.GenericVisit(this); }
}

