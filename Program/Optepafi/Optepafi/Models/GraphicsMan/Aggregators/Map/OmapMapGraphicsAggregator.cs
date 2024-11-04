using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Optepafi.Models.GraphicsMan.Collectors;
using Optepafi.Models.GraphicsMan.Objects;
using Optepafi.Models.GraphicsMan.Objects.Map;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.Maps;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Shapes;

namespace Optepafi.Models.GraphicsMan.Aggregators.Map;

//TODO: comment
public class OmapMapGraphicsAggregator : IMapGraphicsAggregator<OmapMap>
{
    public static OmapMapGraphicsAggregator Instance { get; } = new();
    private OmapMapGraphicsAggregator() { }
    public void AggregateGraphics(OmapMap map, IGraphicObjectCollector collectorForAggregatedObjects,
        CancellationToken? cancellationToken)
    {
        foreach (OmapMap.Symbol symbol in map.Symbols)
        {
            if (_constructors.ContainsKey(symbol.Code))
            {
                foreach (OmapMap.Object obj in map.Objects[symbol.Code])
                {
                   collectorForAggregatedObjects.AddRange(_constructors[symbol.Code].Construct(obj)); 
                }
            }
        }
    }
    public GraphicsArea GetAreaOf(OmapMap map) => new(new MapCoordinates(map.WesternmostCoords.XPos, map.SouthernmostCoords.YPos), new MapCoordinates(map.EasternmostCoords.XPos, map.NorthernmostCoords.YPos));
    public void AggregateGraphicsOfTrack(IList<MapCoordinates> track, IGraphicObjectCollector collectorForAggregatedObjects) => collectorForAggregatedObjects.Add(SimpleOrienteeringCourseConstructor.Instance_.Construct(track));
    
    private interface IConstructor
    {
        IGraphicObject[] Construct(OmapMap.Object omapObject);
    }
    private Dictionary<decimal, IConstructor> _constructors = new Dictionary<decimal, IConstructor>()
    {
        { 101, ContourConstructor.Instance_},
        { 101.1m, SlopeLineConstructor.Instance_ },
        { 102, IndexContourConstructor.Instance_ }, 
        // { 102.1m, ContourValueConstructor.Instance_ },
        { 103, FormLineConstructor.Instance_ },
        { 103.1m, SlopeLineFormLineConstructor.Instance_ },
        { 104, EarthBankConstructor.Instance_ }, 
        { 104.1m, EarthBankMinSizeConstructor.Instance_ }, 
        { 104.2m, EarthBankTopLineConstructor.Instance_ }, 
        { 104.3m, EarthBankTagLineConstructor.Instance_ }, 
        { 105, EarthWallConstructor.Instance_ }, 
        { 106, RuinedEarthWallConstructor.Instance_ }, 
        { 107, ErosionGullyConstructor.Instance_ }, 
        { 108, SmallErosionGullyConstructor.Instance_ }, 
        { 109, SmallKnollConstructor.Instance_ }, 
        { 110, SmallEllongatedKnollConstructor.Instance_ }, 
        { 111, SmallDepressionConstructor.Instance_ }, 
        { 112, PitConstructor.Instance_ }, 
        { 113, BrokenGroundConstructor.Instance_ }, 
        { 113.1m, BrokenGroundIndividualDotConstructor.Instance_ }, 
        { 114, VeryBrokenGroundConstructor.Instance_ }, 
        { 115, ProminentLandformFeatureConstructor.Instance_ }, 
        { 201, ImpassableCliffConstructor.Instance_ }, 
        { 201.1m, ImpassableCliffMinSizeConstructor.Instance_ }, 
        { 201.3m, ImpassableCliffTopLineConstructor.Instance_ }, 
        { 201.4m, ImpassableCliffTagLineConstructor.Instance_ }, 
        { 202, CliffConstructor.Instance_ }, 
        { 202.1m, CliffMinSizeConstructor.Instance_ }, 
        { 202.2m, CliffWithTagsConstructor.Instance_ }, 
        { 202.3m, CliffWithTagsMinSizeConstructor.Instance_ }, 
        { 203.1m, RockyPitOrCaveConstructor.Instance_ }, 
        { 203.2m, RockyPitOrCaveConstructor.Instance_ }, 
        { 204, BoulderConstructor.Instance_ }, 
        { 204.5m, BoulderOrLargeBoulderConstructor .Instance_ },
        { 205, LargeBoulderConstructor.Instance_ }, 
        { 206, GiganticBoulderConstructor.Instance_ }, 
        { 207, BoulderClusterConstructor.Instance_ }, 
        { 207.1m, BoulderClusterLargeConstructor.Instance_ }, 
        { 208, BoulderFieldConstructor.Instance_ }, 
        { 208.1m, BoulderFieldSingleTriangleConstructor.Instance_ }, 
        { 208.2m, BoulderFieldSingleTriangleEnlargedConstructor.Instance_ }, 
        { 209, DenseBoulderFieldConstructor.Instance_ }, 
        { 210, StonyGroundSlowRunningConstructor.Instance_ }, 
        { 210.1m, StonyGroundIndividualDotConstructor.Instance_ }, 
        { 211, StonyGroundWalkConstructor.Instance_ }, 
        { 212, StonyGroundFightConstructor.Instance_ }, 
        { 213, SandyGroundConstructor.Instance_ }, 
        { 214, BareRockConstructor.Instance_ }, 
        { 215, TrenchConstructor.Instance_ }, 
        { 301, UncrossableBodyOfWaterFullColWithBankLineConstructor.Instance_ }, 
        { 301.1m, UncrossableBodyOfWaterFullColConstructor.Instance_ }, 
        { 301.2m, UncrossableBodyOfWaterDominantWithBankLineConstructor.Instance_ }, 
        { 301.3m, UncrossableBodyOfWaterDominantConstructor.Instance_ }, 
        { 301.4m, UncrossableBodyOfWaterBankLineConstructor.Instance_ }, 
        { 302, ShallowBodyOfWaterWithSolidOutlineConstructor.Instance_ }, 
        { 302.1m, ShallowBodyOfWaterConstructor.Instance_ }, 
        { 302.2m, ShallowBodyOfWaterSolidOutlineConstructor.Instance_ }, 
        { 302.3m, ShallowBodyOfWaterDashedOutlineConstructor.Instance_ }, 
        { 302.5m, SmallShallowBodyOfWaterConstructor.Instance_ }, 
        { 303, WaterHoleConstructor.Instance_ }, 
        { 304, CrossableWatercourseConstructor.Instance_ }, 
        { 305, SmallCrossableWatercourseConstructor.Instance_ }, 
        { 306, SeasonalWaterChannelConstructor.Instance_ },
        { 307, UncrossableMarshWithOutlineConstructor.Instance_ }, 
        { 307.1m, UncrossableMarshConstructor.Instance_ }, 
        { 307.2m, UncrossableMarshOutlineConstructor.Instance_ }, 
        { 308, MarshConstructor.Instance_ }, 
        { 308.1m, MarshMinSizeConstructor.Instance_ }, 
        { 309, NarrowMarshConstructor.Instance_ }, 
        { 310, IndistinctMarshConstructor.Instance_ }, 
        { 310.1m, IndistinctMarshMinSizeConstructor.Instance_ }, 
        { 311, WellFountainWaterTankConstructor.Instance_ }, 
        { 312, SpringConstructor.Instance_ }, 
        { 313, ProminentWaterFeatureConstructor.Instance_ }, 
        { 401, OpenLandConstructor.Instance_ }, 
        { 402, OpenLandWithTreesConstructor.Instance_ }, 
        { 402.1m, OpenLandWithBushesConstructor.Instance_ }, 
        { 403, RoughOpenLandConstructor.Instance_ }, 
        { 404, RoughOpenLandWithTreesConstructor.Instance_ }, 
        { 404.1m, RoughOpenLandWithBushesConstructor.Instance_ }, 
        { 405, ForestConstructor.Instance_ }, 
        { 406, VegetationSlowConstructor.Instance_ }, 
        { 406.1m, VegetationSlowNormalOneDirConstructor.Instance_ },
        { 407, VegetationSlowGoodVisibilityConstructor.Instance_ }, 
        { 408, VegetationWalkConstructor.Instance_ }, 
        { 408.1m, VegetationWalkNormalOneDirConstructor.Instance_ }, 
        { 408.2m, VegetationWalkSlowOneDirConstructor.Instance_ }, 
        { 409, VegetationWalkGoodVisibilityConstructor.Instance_ }, 
        { 410, VegetationFightConstructor.Instance_ }, 
        { 410.1m, VegetationFightNormalOneDirConstructor.Instance_ }, 
        { 410.2m, VegetationFightSlowOneDirConstructor.Instance_ }, 
        { 410.3m, VegetationFightWalkOneDirConstructor.Instance_ }, 
        { 410.4m, VegetationFightMinWidthConstructor.Instance_ }, 
        { 412, CultivatedLandConstructor.Instance_ }, 
        { 412.1m, CultivatedLandBlackPatternConstructor.Instance_ },
        { 413, OrchardConstructor.Instance_ }, 
        { 413.1m, OrchardRoughOpenLandConstructor.Instance_ }, 
        { 414, VineyardConstructor.Instance_ }, 
        { 414.1m, VineyardRoughOpenLandConstructor.Instance_ }, 
        { 415, DistinctCultivationBoundaryConstructor.Instance_ }, 
        { 416, DistinctVegetationBoundaryConstructor.Instance_ }, 
        { 416.1m, DistinctVegetationBoundaryGreenDashedLineConstructor.Instance_ }, 
        { 417, ProminentLargeTreeConstructor.Instance_ }, 
        { 418, ProminentBushConstructor.Instance_ }, 
        { 419, ProminentVegetationFeatureConstructor.Instance_ }, 
        { 501, PavedAreaWithBoundingLineConstructor.Instance_ }, 
        { 501.1m, PavedAreaConstructor.Instance_ }, 
        { 501.2m, PavedAreaBoundingLineConstructor.Instance_ }, 
        { 502, WideRoadMinWidthConstructor.Instance_ }, 
        { 502.2m, RoadWithTwoCarriagewaysConstructor.Instance_ }, 
        { 503, RoadConstructor.Instance_ }, 
        { 504, VehicleTrackConstructor.Instance_ },  
        { 505, FootpathConstructor.Instance_ }, 
        { 506, SmallFootpathConstructor.Instance_ }, 
        { 507, LessDistinctSmallFootpathConstructor.Instance_ }, 
        { 508, NarrowRideConstructor.Instance_ }, 
        { 508.1m, NarrowRideEasyConstructor.Instance_ }, 
        { 508.2m, NarrowRideNormalConstructor.Instance_ }, 
        { 508.3m, NarrowRideSlowConstructor.Instance_ }, 
        { 508.4m, NarrowRideWalkConstructor.Instance_ }, 
        { 509, RailwayConstructor.Instance_ }, 
        { 510, PowerLineConstructor.Instance_ }, 
        { 511, MajorPowerLineMinWidthConstructor.Instance_ }, 
        { 511.1m, MajorPowerLineConstructor.Instance_ }, 
        { 511.2m, MajorPowerLineLargeCarryingMastConstructor.Instance_ }, 
        { 512, BridgeTunnelConstructor.Instance_ }, 
        { 512.1m, BridgeTunnelMinSizeConstructor.Instance_ }, 
        { 512.2m, FootBridgeConstructor.Instance_ }, 
        { 513, WallConstructor.Instance_ }, 
        { 514, RuinedWallConstructor.Instance_ }, 
        { 515, ImpassableWallConstructor.Instance_ }, 
        { 516, FenceConstructor.Instance_ }, 
        { 517, RuinedFenceConstructor.Instance_ }, 
        { 518, ImpassableFenceConstructor.Instance_ }, 
        { 519, CrossingPointConstructor.Instance_ }, 
        { 520, NotEnterableAreaConstructor.Instance_ }, 
        { 520.1m, NotEnterableAreaSolidColourBoundingLineConstructor.Instance_ }, 
        { 520.2m, NotEnterableAreaStripesConstructor.Instance_ }, 
        { 520.3m, NotEnterableAreaStripesBoundingLineConstructor.Instance_ }, 
        { 521, BuildingConstructor.Instance_ }, 
        { 521.1m, BuildingMinSizeConstructor.Instance_ }, 
        { 521.2m, LargeBuildingWithOutlineConstructor.Instance_ }, 
        { 521.3m, LargeBuildingConstructor.Instance_ }, 
        { 521.4m, LargeBuildingOutlineConstructor.Instance_ }, 
        { 522, CanopyWithOutlineConstructor.Instance_ }, 
        { 522.1m, CanopyConstructor.Instance_ }, 
        { 522.2m, CanopyOutlineConstructor.Instance_ }, 
        { 523, RuinConstructor.Instance_ }, 
        { 523.1m, RuinMinSizeConstructor.Instance_ }, 
        { 524, HighTowerConstructor.Instance_ }, 
        { 525, SmallTowerConstructor.Instance_ }, 
        { 526, CairnConstructor.Instance_ }, 
        { 527, FodderRackConstructor.Instance_ }, 
        { 528, ProminentLineFeatureConstructor.Instance_ }, 
        { 529, ProminentImpassableLineFeatureConstructor.Instance_ }, 
        { 530, ProminentManMadeFeatureRingConstructor.Instance_ }, 
        { 531, ProminentManMadeFeatureXConstructor.Instance_ }, 
        { 532, StairwayConstructor.Instance_ }, 
        { 532.1m, StairWayWithoutBorderLinesConstructor.Instance_ }, 
        { 601.1m, MagneticNorthLineConstructor.Instance_ }, 
        { 601.2m, NorthLinesPatternConstructor.Instance_ }, 
        { 601.3m, MagneticNorthLineBlueConstructor.Instance_ }, 
        { 601.4m, NorthLinesPatternBlueConstructor.Instance_ }, 
        { 602, RegistrationMarkConstructor.Instance_ }, 
        { 603, SpotHeightDotConstructor.Instance_ }, 
        // { 603.1m, SpotHeightTextConstructor.Instance_ }, 
        // { 704, ControlNumberConstructor.Instance_ }, 
        { 799, SimpleOrienteeringCourseConstructor.Instance_ }, 
    };

    private static Polygon GetPolygonFrom((MapCoordinates coords, byte type)[] typedCoords)
    {
        List<Segment> collectedSegments = new();
        int i = 0;
        while(i < typedCoords.Length)
        {
            switch (typedCoords[i].type % 32)
            {
                case 0:
                    if (i + 1 >= typedCoords.Length) return new Polygon(collectedSegments);
                    collectedSegments.Add(new LineSegment(typedCoords[++i].coords));
                    break;
                case 1:
                    if (i + 3 >= typedCoords.Length) return new Polygon(collectedSegments);
                    collectedSegments.Add(new CubicBezierCurveSegment(typedCoords[++i].coords, typedCoords[++i].coords, typedCoords[++i].coords));
                    break;
                case 2:
                case 18:
                    return new Polygon(collectedSegments);
            } ;
        } 
        return new Polygon(collectedSegments);
    }

    private static Utils.Shapes.Path GetPathFrom((MapCoordinates coords, byte type)[] typedCoords)
    {
        List<Segment> collectedSegments = new();
        var startPoint = typedCoords[0].coords;
        int i = 0;
        while(i < typedCoords.Length)
        {
            switch (typedCoords[i].type % 32)
            {
                case 0:
                    if (i + 1 >= typedCoords.Length) return new Utils.Shapes.Path(startPoint, collectedSegments);
                    collectedSegments.Add(new LineSegment(typedCoords[++i].coords));
                    break;
                case 1:
                    if (i + 3 >= typedCoords.Length) return new Utils.Shapes.Path(startPoint, collectedSegments);
                    collectedSegments.Add(new CubicBezierCurveSegment(typedCoords[++i].coords, typedCoords[++i].coords, typedCoords[++i].coords));
                    break;
                case 2:
                case 18:
                    return new Utils.Shapes.Path(startPoint, collectedSegments);
            } ;
        } 
        return new Utils.Shapes.Path(startPoint, collectedSegments);
    }
    
    private class ContourConstructor : IConstructor
    {
        public static ContourConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) => 
            [new Contour_101(GetPathFrom(omapObject.TypedCoords))];
    }

    private class SlopeLineConstructor : IConstructor
    {
        public static SlopeLineConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new SlopeLine_101_1(omapObject.TypedCoords[0].Item1, omapObject.SymbolRotation)];
    }

    private class IndexContourConstructor : IConstructor
    {
        public static IndexContourConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) => 
            [new IndexContour_102(GetPathFrom(omapObject.TypedCoords))];
    }

    // private class ContourValueConstructor : IConstructor
    // {
        // public static ContourValueConstructor Instance_ { get; } = new();

        // public IGraphicObject Construct(OmapMap.Object omapObject)
        // {
            
        // }

    // }

    private class FormLineConstructor : IConstructor
    {
        public static FormLineConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) => 
            [new FormLine_103(GetPathFrom(omapObject.TypedCoords))];

    } 
    private class SlopeLineFormLineConstructor : IConstructor
    {
        public static SlopeLineFormLineConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new SlopeLineFormLine_103_1(omapObject.TypedCoords[0].Item1, omapObject.SymbolRotation)];

    } 
    private class EarthBankConstructor : IConstructor
    {
        public static EarthBankConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new EarthBank_104(GetPathFrom(omapObject.TypedCoords))];
    } 
    private class EarthBankMinSizeConstructor : IConstructor
    {
        public static EarthBankMinSizeConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new EarthBankMinSize_104_1(omapObject.TypedCoords[0].Item1, omapObject.SymbolRotation)];
    } 
    private class EarthBankTopLineConstructor : IConstructor
    {
        public static EarthBankTopLineConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new EarthBankTopLine_104_2(GetPathFrom(omapObject.TypedCoords))];
    } 
    private class EarthBankTagLineConstructor : IConstructor
    {
        public static EarthBankTagLineConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new EarthBankTagLine_104_3(GetPathFrom(omapObject.TypedCoords))];
    } 
    private class EarthWallConstructor : IConstructor
    {
        public static EarthWallConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new EarthWall_105(GetPathFrom(omapObject.TypedCoords))];
    } 
    private class RuinedEarthWallConstructor : IConstructor
    {
        public static RuinedEarthWallConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new RuinedEarthWall_106(GetPathFrom(omapObject.TypedCoords))];
    } 
    private class ErosionGullyConstructor : IConstructor
    {
        public static ErosionGullyConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new ErosionGully_107(GetPathFrom(omapObject.TypedCoords))];
    } 
    private class SmallErosionGullyConstructor : IConstructor
    {
        public static SmallErosionGullyConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new SmallErosionGully_108(GetPathFrom(omapObject.TypedCoords))];
    } 
    private class SmallKnollConstructor : IConstructor
    {
        public static SmallKnollConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new SmallKnoll_109(omapObject.TypedCoords[0].Item1)];
    } 
    private class SmallEllongatedKnollConstructor : IConstructor
    {
        public static SmallEllongatedKnollConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new SmallEllongatedKnoll_110(omapObject.TypedCoords[0].Item1, omapObject.SymbolRotation)];
    } 
    private class SmallDepressionConstructor : IConstructor
    {
        public static SmallDepressionConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new SmallDepression_111(omapObject.TypedCoords[0].Item1)];
    } 
    private class PitConstructor : IConstructor
    {
        public static PitConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new Pit_112(omapObject.TypedCoords[0].Item1)];
    } 
    private class BrokenGroundConstructor : IConstructor
    {
        public static BrokenGroundConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new BrokenGround_113(GetPolygonFrom(omapObject.TypedCoords), omapObject.SymbolRotation)];
    } 
    private class BrokenGroundIndividualDotConstructor : IConstructor
    {
        public static BrokenGroundIndividualDotConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new BrokenGroundIndividualDot_113_1(omapObject.TypedCoords[0].Item1)];
    } 
    private class VeryBrokenGroundConstructor : IConstructor
    {
        public static VeryBrokenGroundConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new VeryBrokenGround_114(GetPolygonFrom(omapObject.TypedCoords), omapObject.SymbolRotation)];
    } 
    private class ProminentLandformFeatureConstructor : IConstructor
    {
        public static ProminentLandformFeatureConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new ProminentLandformFeature_115(omapObject.TypedCoords[0].Item1)];
    } 
    private class ImpassableCliffConstructor : IConstructor
    {
        public static ImpassableCliffConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new ImpassableCliff_201(GetPathFrom(omapObject.TypedCoords))];
    }
    private class ImpassableCliffMinSizeConstructor : IConstructor
    {
        public static ImpassableCliffMinSizeConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new ImpassableCliffMinSize_201_1(omapObject.TypedCoords[0].Item1, omapObject.SymbolRotation)];
    }
    private class ImpassableCliffTopLineConstructor : IConstructor
    {
        public static ImpassableCliffTopLineConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new ImpassableCliffTopLine_201_3(GetPathFrom(omapObject.TypedCoords))];
    }
    private class ImpassableCliffTagLineConstructor : IConstructor
    {
        public static ImpassableCliffTagLineConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new ImpassableCliffTagLine_201_4(GetPathFrom(omapObject.TypedCoords))];
    }
    private class CliffConstructor : IConstructor
    {
        public static CliffConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new Cliff_202(GetPathFrom(omapObject.TypedCoords))];
    }
    private class CliffMinSizeConstructor : IConstructor
    {
        public static CliffMinSizeConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new CliffMinSize_202_1(omapObject.TypedCoords[0].Item1, omapObject.SymbolRotation)];
    }
    private class CliffWithTagsConstructor : IConstructor
    {
        public static CliffWithTagsConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new CliffWithTags_202_2(GetPathFrom(omapObject.TypedCoords))];
    }
    private class CliffWithTagsMinSizeConstructor : IConstructor
    {
        public static CliffWithTagsMinSizeConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new CliffWithTagsMinSize_202_3(omapObject.TypedCoords[0].Item1, omapObject.SymbolRotation)];
    }
    private class RockyPitOrCaveConstructor : IConstructor
    {
        public static RockyPitOrCaveConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new RockyPitOrCave_203_1_2(omapObject.TypedCoords[0].Item1, omapObject.SymbolRotation)];
    }
    private class BoulderConstructor : IConstructor
    {
        public static BoulderConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new Boulder_204(omapObject.TypedCoords[0].Item1)];
    }
    private class BoulderOrLargeBoulderConstructor : IConstructor
    {
        public static BoulderOrLargeBoulderConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new BoulderOrLargeBoulder_204_5(omapObject.TypedCoords[0].Item1)];
    }
    private class LargeBoulderConstructor : IConstructor
    {
        public static LargeBoulderConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new LargeBoulder_205(omapObject.TypedCoords[0].Item1)];
    }
    private class GiganticBoulderConstructor : IConstructor
    {
        public static GiganticBoulderConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new GiganticBoulder_206(GetPolygonFrom(omapObject.TypedCoords))];
    }
    private class BoulderClusterConstructor : IConstructor
    {
        public static BoulderClusterConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new BoulderCluster_207(omapObject.TypedCoords[0].Item1)];
    }
    private class BoulderClusterLargeConstructor : IConstructor
    {
        public static BoulderClusterLargeConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new BoulderClusterLarge_207_1(omapObject.TypedCoords[0].Item1)];
    }
    private class BoulderFieldConstructor : IConstructor
    {
        public static BoulderFieldConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new BoulderField_208(GetPolygonFrom(omapObject.TypedCoords), omapObject.SymbolRotation)];
    }

    private class BoulderFieldSingleTriangleConstructor : IConstructor
    {
        public static BoulderFieldSingleTriangleConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new BoulderFieldSingleTriangle_208_1(omapObject.TypedCoords[0].Item1, omapObject.SymbolRotation)];
    }
    private class BoulderFieldSingleTriangleEnlargedConstructor : IConstructor
    {
        public static BoulderFieldSingleTriangleEnlargedConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new BoulderFieldSingleTriangleEnlarged_208_2(omapObject.TypedCoords[0].Item1, omapObject.SymbolRotation)];
    }
    private class DenseBoulderFieldConstructor : IConstructor
    {
        public static DenseBoulderFieldConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new DenseBoulderField_209(GetPolygonFrom(omapObject.TypedCoords), omapObject.SymbolRotation)];
    }
    private class StonyGroundSlowRunningConstructor : IConstructor
    {
        public static StonyGroundSlowRunningConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new StonyGroundSlowRunning_210(GetPolygonFrom(omapObject.TypedCoords), omapObject.SymbolRotation)];
    }
    private class StonyGroundIndividualDotConstructor : IConstructor
    {
        public static StonyGroundIndividualDotConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new StonyGroundIndividualDot_210_1(omapObject.TypedCoords[0].Item1)];
        

    }
    private class StonyGroundWalkConstructor : IConstructor
    {
        public static StonyGroundWalkConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new StonyGroundWalk_211(GetPolygonFrom(omapObject.TypedCoords), omapObject.SymbolRotation)];
    }
    private class StonyGroundFightConstructor : IConstructor
    {
        public static StonyGroundFightConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new StonyGroundFight_212(GetPolygonFrom(omapObject.TypedCoords), omapObject.SymbolRotation)];
    }
    private class SandyGroundConstructor : IConstructor
    {
        public static SandyGroundConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new SandyGround_213(GetPolygonFrom(omapObject.TypedCoords))];
    }
    private class BareRockConstructor : IConstructor
    {
        public static BareRockConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new BareRock_214(GetPolygonFrom(omapObject.TypedCoords))];
    }
    private class TrenchConstructor : IConstructor
    {
        public static TrenchConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new Trench_215(GetPathFrom(omapObject.TypedCoords))];
    }
    private class UncrossableBodyOfWaterFullColWithBankLineConstructor : IConstructor
    {
        public static UncrossableBodyOfWaterFullColWithBankLineConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new UncrossableBodyOfWaterFullCol_301_1(GetPolygonFrom(omapObject.TypedCoords)),
            new UncrossableBodyOfWaterBankLine_301_4(GetPathFrom(omapObject.TypedCoords))];
    }
    private class UncrossableBodyOfWaterFullColConstructor : IConstructor
    {
        public static UncrossableBodyOfWaterFullColConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new UncrossableBodyOfWaterFullCol_301_1(GetPolygonFrom(omapObject.TypedCoords))];
    }
    private class UncrossableBodyOfWaterDominantWithBankLineConstructor : IConstructor
    {
        public static UncrossableBodyOfWaterDominantWithBankLineConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new UncrossableBodyOfWaterDominant_301_3(GetPolygonFrom(omapObject.TypedCoords)),
            new UncrossableBodyOfWaterBankLine_301_4(GetPathFrom(omapObject.TypedCoords))];
    }
    private class UncrossableBodyOfWaterDominantConstructor : IConstructor
    {
        public static UncrossableBodyOfWaterDominantConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new UncrossableBodyOfWaterDominant_301_3(GetPolygonFrom(omapObject.TypedCoords))];
    }
    private class UncrossableBodyOfWaterBankLineConstructor : IConstructor
    {
        public static UncrossableBodyOfWaterBankLineConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new UncrossableBodyOfWaterBankLine_301_4(GetPathFrom(omapObject.TypedCoords))];
    }
    private class ShallowBodyOfWaterWithSolidOutlineConstructor : IConstructor
    {
        public static ShallowBodyOfWaterWithSolidOutlineConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new ShallowBodyOfWater_302_1(GetPolygonFrom(omapObject.TypedCoords)),
            new ShallowBodyOfWaterSolidOutline_302_2(GetPathFrom(omapObject.TypedCoords))];
    }
    private class ShallowBodyOfWaterConstructor : IConstructor
    {
        public static ShallowBodyOfWaterConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new ShallowBodyOfWater_302_1(GetPolygonFrom(omapObject.TypedCoords))];
    }
    private class ShallowBodyOfWaterSolidOutlineConstructor : IConstructor
    {
        public static ShallowBodyOfWaterSolidOutlineConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new ShallowBodyOfWaterSolidOutline_302_2(GetPathFrom(omapObject.TypedCoords))];
    }
    private class ShallowBodyOfWaterDashedOutlineConstructor : IConstructor
    {
        public static ShallowBodyOfWaterDashedOutlineConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new ShallowBodyOfWaterDashedOutline_302_3(GetPathFrom(omapObject.TypedCoords))];
    }
    private class SmallShallowBodyOfWaterConstructor : IConstructor
    {
        public static SmallShallowBodyOfWaterConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new SmallShallowBodyOfWater_302_5(GetPolygonFrom(omapObject.TypedCoords))];
    }
    private class WaterHoleConstructor : IConstructor
    {
        public static WaterHoleConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new WaterHole_303(omapObject.TypedCoords[0].Item1)];
    }
    private class CrossableWatercourseConstructor : IConstructor
    {
        public static CrossableWatercourseConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new CrossableWatercourse_304(GetPathFrom(omapObject.TypedCoords))];
    }
    private class SmallCrossableWatercourseConstructor : IConstructor
    {
        public static SmallCrossableWatercourseConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new SmallCrossableWatercourse_305(GetPathFrom(omapObject.TypedCoords))];
    }
    private class SeasonalWaterChannelConstructor : IConstructor
    {
        public static SeasonalWaterChannelConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new SeasonalWaterChannel_306(GetPathFrom(omapObject.TypedCoords))];
    }
    private class UncrossableMarshWithOutlineConstructor : IConstructor
    {
        public static UncrossableMarshWithOutlineConstructor Instance_ { get; } = new();

        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new UncrossableMarsh_307_1(GetPolygonFrom(omapObject.TypedCoords)),
            new UncrossableMarshOutline_307_2(GetPathFrom(omapObject.TypedCoords))];
    }
    private class UncrossableMarshConstructor : IConstructor
    {
        public static UncrossableMarshConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new UncrossableMarsh_307_1(GetPolygonFrom(omapObject.TypedCoords))];
    }
    private class UncrossableMarshOutlineConstructor : IConstructor
    {
        public static UncrossableMarshOutlineConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new UncrossableMarshOutline_307_2(GetPathFrom(omapObject.TypedCoords))];
    }
    private class MarshConstructor : IConstructor
    {
        public static MarshConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new Marsh_308(GetPolygonFrom(omapObject.TypedCoords))];
    }
    private class MarshMinSizeConstructor : IConstructor
    {
        public static MarshMinSizeConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new MarshMinSize_308_1(omapObject.TypedCoords[0].Item1)];
    }
    private class NarrowMarshConstructor : IConstructor
    {
        public static NarrowMarshConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new NarrowMarsh_309(GetPathFrom(omapObject.TypedCoords))];
    }
    private class IndistinctMarshConstructor : IConstructor
    {
        public static IndistinctMarshConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new IndistinctMarsh_310(GetPolygonFrom(omapObject.TypedCoords))];
    }
    private class IndistinctMarshMinSizeConstructor : IConstructor
    {
        public static IndistinctMarshMinSizeConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new IndistinctMarshMinSize_310_1(omapObject.TypedCoords[0].Item1)];
    }
    private class WellFountainWaterTankConstructor : IConstructor
    {
        public static WellFountainWaterTankConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new WellFountainWaterTank_311(omapObject.TypedCoords[0].Item1)];
    }

    private class SpringConstructor : IConstructor
    {
        public static SpringConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new Spring_312(omapObject.TypedCoords[0].Item1)];
    }
    private class ProminentWaterFeatureConstructor : IConstructor
    {
        public static ProminentWaterFeatureConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new ProminentWaterFeature_313(omapObject.TypedCoords[0].Item1)];
    }
    private class OpenLandConstructor : IConstructor
    {
        public static OpenLandConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new OpenLand_401(GetPolygonFrom(omapObject.TypedCoords))];
    }
    private class OpenLandWithTreesConstructor : IConstructor
    {
        public static OpenLandWithTreesConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new OpenLandWithTrees_402(GetPolygonFrom(omapObject.TypedCoords))];
    }
    private class OpenLandWithBushesConstructor : IConstructor
    {
        public static OpenLandWithBushesConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new OpenLandWithBushes_402_1(GetPolygonFrom(omapObject.TypedCoords))];
    }
    private class RoughOpenLandConstructor : IConstructor
    {
        public static RoughOpenLandConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new RoughOpenLand_403(GetPolygonFrom(omapObject.TypedCoords))];
    }
    private class RoughOpenLandWithTreesConstructor : IConstructor
    {
        public static RoughOpenLandWithTreesConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new RoughOpenLandWithTrees_404(GetPolygonFrom(omapObject.TypedCoords))];
    }
    private class RoughOpenLandWithBushesConstructor : IConstructor
    {
        public static RoughOpenLandWithBushesConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new RoughOpenLandWithBushes_404_1(GetPolygonFrom(omapObject.TypedCoords))];
    }
    private class ForestConstructor : IConstructor
    {
        public static ForestConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new Forest_405(GetPolygonFrom(omapObject.TypedCoords))];
    }
    private class VegetationSlowConstructor : IConstructor
    {
        public static VegetationSlowConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new VegetationSlow_406(GetPolygonFrom(omapObject.TypedCoords))];
    }
    private class VegetationSlowNormalOneDirConstructor : IConstructor
    {
        public static VegetationSlowNormalOneDirConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new VegetationSlowNormalOneDir_406_1(GetPolygonFrom(omapObject.TypedCoords), omapObject.SymbolRotation)];
    }
    private class VegetationSlowGoodVisibilityConstructor : IConstructor
    {
        public static VegetationSlowGoodVisibilityConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new VegetationSlowGoodVisibility_407(GetPolygonFrom(omapObject.TypedCoords))];
    }
    private class VegetationWalkConstructor : IConstructor
    {
        public static VegetationWalkConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new VegetationWalk_408(GetPolygonFrom(omapObject.TypedCoords))];
    }
    private class VegetationWalkNormalOneDirConstructor : IConstructor
    {
        public static VegetationWalkNormalOneDirConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new VegetationWalkNormalOneDir_408_1(GetPolygonFrom(omapObject.TypedCoords), omapObject.SymbolRotation)];
    }
    private class VegetationWalkSlowOneDirConstructor : IConstructor
    {
        public static VegetationWalkSlowOneDirConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new VegetationWalkSlowOneDir_408_2(GetPolygonFrom(omapObject.TypedCoords), omapObject.SymbolRotation)];
    }
    private class VegetationWalkGoodVisibilityConstructor : IConstructor 
    {
        public static VegetationWalkGoodVisibilityConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new VegetationWalkGoodVisibility_409(GetPolygonFrom(omapObject.TypedCoords))];
    }

    private class VegetationFightConstructor : IConstructor
    {
        public static VegetationFightConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new VegetationFight_410(GetPolygonFrom(omapObject.TypedCoords))];
    }
    private class VegetationFightNormalOneDirConstructor : IConstructor
    {
        public static VegetationFightNormalOneDirConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new VegetationFightNormalOneDir_410_1(GetPolygonFrom(omapObject.TypedCoords), omapObject.SymbolRotation)];
        

    }
    private class VegetationFightSlowOneDirConstructor : IConstructor
    {
        public static VegetationFightSlowOneDirConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new VegetationFightSlowOneDir_410_2(GetPolygonFrom(omapObject.TypedCoords), omapObject.SymbolRotation)];
    }
    private class VegetationFightWalkOneDirConstructor : IConstructor
    {
        public static VegetationFightWalkOneDirConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new VegetationFightWalkOneDir_410_3(GetPolygonFrom(omapObject.TypedCoords), omapObject.SymbolRotation)];
    }
    private class VegetationFightMinWidthConstructor : IConstructor
    {
        public static VegetationFightMinWidthConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new VegetationFightMinWidth_410_4(GetPathFrom(omapObject.TypedCoords))];
    }
    private class CultivatedLandConstructor : IConstructor
    {
        public static CultivatedLandConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new OpenLand_401(GetPolygonFrom(omapObject.TypedCoords)),
            new CultivatedLandBlackPattern_412_1(GetPolygonFrom(omapObject.TypedCoords))];
    }

    private class CultivatedLandBlackPatternConstructor : IConstructor
    {
        public static CultivatedLandBlackPatternConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new CultivatedLandBlackPattern_412_1(GetPolygonFrom(omapObject.TypedCoords))];
    }
    private class OrchardConstructor : IConstructor 
    {
        public static OrchardConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new Orchard_413(GetPolygonFrom(omapObject.TypedCoords), omapObject.SymbolRotation)];
    }
    private class OrchardRoughOpenLandConstructor : IConstructor
    {
        public static OrchardRoughOpenLandConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new OrchardRoughOpenLand_413_1(GetPolygonFrom(omapObject.TypedCoords), omapObject.SymbolRotation)];
    }
    private class VineyardConstructor : IConstructor
    {
        public static VineyardConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new Vineyard_414(GetPolygonFrom(omapObject.TypedCoords), omapObject.SymbolRotation)];
    }
    private class VineyardRoughOpenLandConstructor : IConstructor
    {
        public static VineyardRoughOpenLandConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new VineyardRoughOpenLand_414_1(GetPolygonFrom(omapObject.TypedCoords), omapObject.SymbolRotation)];
    }
    private class DistinctCultivationBoundaryConstructor : IConstructor
    {
        public static DistinctCultivationBoundaryConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new DistinctCultivationBoundary_415(GetPathFrom(omapObject.TypedCoords))];
    }
    private class DistinctVegetationBoundaryConstructor : IConstructor
    {
        public static DistinctVegetationBoundaryConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new DistinctVegetationBoundary_416(GetPathFrom(omapObject.TypedCoords))];
    }
    private class DistinctVegetationBoundaryGreenDashedLineConstructor : IConstructor
    {
        public static DistinctVegetationBoundaryGreenDashedLineConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new DistinctVegetationBoundaryGreenDashedLine_416_1(GetPathFrom(omapObject.TypedCoords))];
    }
    private class ProminentLargeTreeConstructor : IConstructor
    {
        public static ProminentLargeTreeConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new ProminentLargeTree_417(omapObject.TypedCoords[0].Item1)];
        

    }
    private class ProminentBushConstructor : IConstructor
    {
        public static ProminentBushConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new ProminentBush_418(omapObject.TypedCoords[0].Item1)];
    }
    private class ProminentVegetationFeatureConstructor : IConstructor
    {
        public static ProminentVegetationFeatureConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new ProminentVegetationFeature_419(omapObject.TypedCoords[0].Item1)];
    }
    private class PavedAreaWithBoundingLineConstructor : IConstructor
    {
        public static PavedAreaWithBoundingLineConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new PavedArea_501_1(GetPolygonFrom(omapObject.TypedCoords)),
            new PavedAreaBoundingLine_501_2(GetPathFrom(omapObject.TypedCoords))];
    }
    private class PavedAreaConstructor : IConstructor
    {
        public static PavedAreaConstructor Instance_ { get; } = new();

        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new PavedArea_501_1(GetPolygonFrom(omapObject.TypedCoords))];
    }
    private class PavedAreaBoundingLineConstructor : IConstructor
    {
        public static PavedAreaBoundingLineConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new PavedAreaBoundingLine_501_2(GetPathFrom(omapObject.TypedCoords))];
    }
    private class WideRoadMinWidthConstructor : IConstructor
    {
        public static WideRoadMinWidthConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new WideRoadMinWidth_502(GetPathFrom(omapObject.TypedCoords))];
    }
    private class RoadWithTwoCarriagewaysConstructor : IConstructor
    {
        public static RoadWithTwoCarriagewaysConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new RoadWithTwoCarriageways_502_2(GetPathFrom(omapObject.TypedCoords))];
    }
    private class RoadConstructor : IConstructor
    {
        public static RoadConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new Road_503(GetPathFrom(omapObject.TypedCoords))];
    }
    private class VehicleTrackConstructor : IConstructor
    {
        public static VehicleTrackConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new VehicleTrack_504(GetPathFrom(omapObject.TypedCoords))];
    }
    private class FootpathConstructor : IConstructor
    {
        public static FootpathConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new Footpath_505(GetPathFrom(omapObject.TypedCoords))];
    }
    private class SmallFootpathConstructor : IConstructor
    {
        public static SmallFootpathConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new SmallFootpath_506(GetPathFrom(omapObject.TypedCoords))];
    }
    private class LessDistinctSmallFootpathConstructor : IConstructor
    {
        public static LessDistinctSmallFootpathConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new LessDistinctSmallFootpath_507(GetPathFrom(omapObject.TypedCoords))];
    }
    private class NarrowRideConstructor : IConstructor
    {
        public static NarrowRideConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new NarrowRide_508(GetPathFrom(omapObject.TypedCoords))];
    }
    private class NarrowRideEasyConstructor : IConstructor
    {
        public static NarrowRideEasyConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new NarrowRideEasy_508_1(GetPathFrom(omapObject.TypedCoords))];
        

    }
    private class NarrowRideNormalConstructor : IConstructor
    {
        public static NarrowRideNormalConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new NarrowRideNormal_508_2(GetPathFrom(omapObject.TypedCoords))];
    }
    private class NarrowRideSlowConstructor : IConstructor
    {
        public static NarrowRideSlowConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new NarrowRideSlow_508_3(GetPathFrom(omapObject.TypedCoords))];
    }
    private class NarrowRideWalkConstructor : IConstructor
    {
        public static NarrowRideWalkConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new NarrowRideWalk_508_4(GetPathFrom(omapObject.TypedCoords))];
    }
    private class RailwayConstructor : IConstructor
    {
        public static RailwayConstructor  Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new Railway_509(GetPathFrom(omapObject.TypedCoords))];
    }
    private class PowerLineConstructor : IConstructor
    {
        public static PowerLineConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new PowerLine_510(GetPathFrom(omapObject.TypedCoords))];
    }
    private class MajorPowerLineMinWidthConstructor : IConstructor
    {
        public static MajorPowerLineMinWidthConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new MajorPowerLineMinWidth_511(GetPathFrom(omapObject.TypedCoords))];
    }
    private class MajorPowerLineConstructor : IConstructor
    {
        public static MajorPowerLineConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new MajorPowerLine_511_1(GetPathFrom(omapObject.TypedCoords))];
    }
    private class MajorPowerLineLargeCarryingMastConstructor : IConstructor
    {
        public static MajorPowerLineLargeCarryingMastConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new MajorPowerLineLargeCarryingMasts_511_2(GetPathFrom(omapObject.TypedCoords))];
    }
    private class BridgeTunnelConstructor : IConstructor
    {
        public static BridgeTunnelConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new BridgeTunnel_512(omapObject.TypedCoords[0].Item1, omapObject.SymbolRotation)];
    }
    private class BridgeTunnelMinSizeConstructor : IConstructor
    {
        public static BridgeTunnelMinSizeConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new BridgeTunnelMinSize_512_1(omapObject.TypedCoords[0].Item1, omapObject.SymbolRotation)];
    }
    private class FootBridgeConstructor : IConstructor
    {
        public static FootBridgeConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new FootBridge_512_2(omapObject.TypedCoords[0].Item1, omapObject.SymbolRotation)];
    }
    private class WallConstructor : IConstructor
    {
        public static WallConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new Wall_513(GetPathFrom(omapObject.TypedCoords))];
    }
    private class RuinedWallConstructor : IConstructor
    {
        public static RuinedWallConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new RuinedWall_514(GetPathFrom(omapObject.TypedCoords))];
    }
    private class ImpassableWallConstructor : IConstructor
    {
        public static ImpassableWallConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new ImpassableWall_515(GetPathFrom(omapObject.TypedCoords))];
    }
    private class FenceConstructor : IConstructor
    {
        public static FenceConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new Fence_516(GetPathFrom(omapObject.TypedCoords))];
    }
    private class RuinedFenceConstructor : IConstructor
    {
        public static RuinedFenceConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new RuinedFence_517(GetPathFrom(omapObject.TypedCoords))];
    }
    private class ImpassableFenceConstructor : IConstructor
    {
        public static ImpassableFenceConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new ImpassableFence_518(GetPathFrom(omapObject.TypedCoords))];
    }
    private class CrossingPointConstructor : IConstructor
    {
        public static CrossingPointConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new CrossingPoint_519(omapObject.TypedCoords[0].Item1, omapObject.SymbolRotation)];
    }
    private class NotEnterableAreaConstructor : IConstructor
    {
        public static NotEnterableAreaConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new NotEnterableArea_520(GetPolygonFrom(omapObject.TypedCoords))];
    }
    private class NotEnterableAreaSolidColourBoundingLineConstructor : IConstructor
    {
        public static NotEnterableAreaSolidColourBoundingLineConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new NotEnterableAreaSolidColourBoundingLine_520_1(GetPathFrom(omapObject.TypedCoords))];
    }
    private class NotEnterableAreaStripesConstructor : IConstructor
    {
        public static NotEnterableAreaStripesConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new NotEnterableAreaStripes_502_2(GetPolygonFrom(omapObject.TypedCoords))];
    }
    private class NotEnterableAreaStripesBoundingLineConstructor : IConstructor
    {
        public static NotEnterableAreaStripesBoundingLineConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new NotEnterableAreaStripesBoundingLine_520_3(GetPathFrom(omapObject.TypedCoords))];
    }
    private class BuildingConstructor : IConstructor
    {
        public static BuildingConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new Building_521(GetPolygonFrom(omapObject.TypedCoords))];
    }
    private class BuildingMinSizeConstructor : IConstructor
    {
        public static BuildingMinSizeConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new BuildingMinSize_521_1(omapObject.TypedCoords[0].Item1, omapObject.SymbolRotation)];
    }
    private class LargeBuildingWithOutlineConstructor : IConstructor
    {
        public static LargeBuildingWithOutlineConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new LargeBuilding_521_3(GetPolygonFrom(omapObject.TypedCoords)),
            new LargeBuildingOutline_521_4(GetPathFrom(omapObject.TypedCoords))];
    }
    private class LargeBuildingConstructor : IConstructor
    {
        public static LargeBuildingConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new LargeBuilding_521_3(GetPolygonFrom(omapObject.TypedCoords))];
    }
    private class LargeBuildingOutlineConstructor : IConstructor
    {
        public static LargeBuildingOutlineConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new LargeBuildingOutline_521_4(GetPathFrom(omapObject.TypedCoords))];
    }
    private class CanopyWithOutlineConstructor : IConstructor
    {
        public static CanopyWithOutlineConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new Canopy_522_1(GetPolygonFrom(omapObject.TypedCoords)),
            new CanopyOutline_522_2(GetPathFrom(omapObject.TypedCoords))];
    }
    private class CanopyConstructor : IConstructor
    {
        public static CanopyConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new Canopy_522_1(GetPolygonFrom(omapObject.TypedCoords))];
    }
    private class CanopyOutlineConstructor : IConstructor
    {
        public static CanopyOutlineConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new CanopyOutline_522_2(GetPathFrom(omapObject.TypedCoords))];
    }
    private class RuinConstructor : IConstructor
    {
        public static RuinConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new Ruin_523(GetPathFrom(omapObject.TypedCoords))];
    }
    private class RuinMinSizeConstructor : IConstructor
    {
        public static RuinMinSizeConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new RuinMinSize_523_1(omapObject.TypedCoords[0].Item1, omapObject.SymbolRotation)];
    }
    private class HighTowerConstructor : IConstructor
    {
        public static HighTowerConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new HighTower_524(omapObject.TypedCoords[0].Item1)];
    }
    private class SmallTowerConstructor : IConstructor
    {
        public static SmallTowerConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new SmallTower_525(omapObject.TypedCoords[0].Item1)];
    }
    private class CairnConstructor : IConstructor
    {
        public static CairnConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new Cairn_526(omapObject.TypedCoords[0].Item1)];
    }
    private class FodderRackConstructor : IConstructor
    {
        public static FodderRackConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new FodderRack_527(omapObject.TypedCoords[0].Item1)];
    }
    private class ProminentLineFeatureConstructor : IConstructor
    {
        public static ProminentLineFeatureConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new ProminentLineFeature_528(GetPathFrom(omapObject.TypedCoords))];
    }
    private class ProminentImpassableLineFeatureConstructor : IConstructor
    {
        public static ProminentImpassableLineFeatureConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new ProminentImpassableLineFeature_529(GetPathFrom(omapObject.TypedCoords))];
    }
    private class ProminentManMadeFeatureRingConstructor : IConstructor
    {
        public static ProminentManMadeFeatureRingConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new ProminentManMadeFeatureRing_530(omapObject.TypedCoords[0].Item1)];
    }
    private class ProminentManMadeFeatureXConstructor : IConstructor
    {
        public static ProminentManMadeFeatureXConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new ProminentManMadeFeatureX_531(omapObject.TypedCoords[0].Item1)];
    }
    private class StairwayConstructor : IConstructor
    {
        public static StairwayConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new Stairway_532(GetPathFrom(omapObject.TypedCoords))];
    }
    private class StairWayWithoutBorderLinesConstructor : IConstructor
    {
        public static StairWayWithoutBorderLinesConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new StairWayWithoutBorderLines_532_1(GetPathFrom(omapObject.TypedCoords))];
    }
    private class MagneticNorthLineConstructor : IConstructor
    {
        public static MagneticNorthLineConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new MagneticNorthLine_601_1(GetPathFrom(omapObject.TypedCoords))];
    }
    private class NorthLinesPatternConstructor : IConstructor
    {
        public static NorthLinesPatternConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new NorthLinesPattern_601_2(GetPathFrom(omapObject.TypedCoords))];
    }
    private class MagneticNorthLineBlueConstructor : IConstructor
    {
        public static MagneticNorthLineBlueConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new MagneticNorthLineBlue_601_3(GetPathFrom(omapObject.TypedCoords))];
    }
    private class NorthLinesPatternBlueConstructor : IConstructor
    {
        public static NorthLinesPatternBlueConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new NorthLinesPatternBlue_601_4(GetPathFrom(omapObject.TypedCoords))];
    }
    private class RegistrationMarkConstructor : IConstructor
    {
        public static RegistrationMarkConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new RegistrationMark_602(omapObject.TypedCoords[0].Item1)];
    }
    private class SpotHeightDotConstructor : IConstructor
    {
        public static SpotHeightDotConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new SpotHeightDot_603(omapObject.TypedCoords[0].Item1)];
    }
    // private class SpotHeightTextConstructor : IConstructor
    // {
        // public static SpotHeightTextConstructor Instance_ { get; } = new();
        // public IGraphicObject Construct(OmapMap.Object omapObject) =>
    // }
    // private class ControlNumberConstructor : IConstructor
    // {
        // public static ControlNumberConstructor Instance_ { get; } = new();
        // public IGraphicObject Construct(OmapMap.Object omapObject) =>
    // }
    private class SimpleOrienteeringCourseConstructor : IConstructor
    {
        public static SimpleOrienteeringCourseConstructor Instance_ { get; } = new();
        public IGraphicObject[] Construct(OmapMap.Object omapObject) =>
            [new SimpleOrienteeringCourse_799(GetPathFrom(omapObject.TypedCoords))];

        public IGraphicObject Construct(IList<MapCoordinates> CourseCoordinatesList)
        {
            MapCoordinates firstCoords = CourseCoordinatesList[0];
            CourseCoordinatesList.RemoveAt(0);
            return new SimpleOrienteeringCourse_799(new Utils.Shapes.Path(firstCoords, CourseCoordinatesList.Select(coords => new LineSegment(coords)).ToList<Segment>()));
        }
    }
}