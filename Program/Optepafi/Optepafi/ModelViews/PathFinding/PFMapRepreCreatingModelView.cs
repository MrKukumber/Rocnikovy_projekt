using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan;
using Optepafi.Models.MapRepreMan.MapRepreReps;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.ModelViews.PathFinding;

public abstract class PFMapRepreCreatingModelView : ModelViewBase
{
    protected PFMapRepreCreatingModelView(){}
    
    public enum ElevDataPrerequisiteCheckResult {InOrder, ElevDataForMapNotPresent, MapNotSupportedByElevDataType, Cancelled}
    public abstract ElevDataPrerequisiteCheckResult CheckMapRequirementsForElevData(CancellationToken ct);
    public abstract Task CreateMapRepreAsync(IProgress<string> progressInfo, IProgress<GraphCreationReport> mapCreationProgress, CancellationToken cancellationToken);
    public abstract void CleanMapRepre();
}
public partial class PathFindingSessionModelView : SessionModelView
{
    private class PFMapRepreCreatingIntraModelView : PFMapRepreCreatingModelView
    {
        private PFSettingsIntraModelView Settings { get; }

        private bool _useElevData;
        public override ElevDataPrerequisiteCheckResult CheckMapRequirementsForElevData(CancellationToken ct)
        {
            IMapFormat<IMap> mapFormat= MapManager.Instance.GetFormat(Map);
            switch (MapRepreManager.Instance.DoesNeedElevData(Template, mapFormat, MapRepreRepresentative))
            {
                case MapRepreManager.NeedsElevDataIndic.No:
                    if (ct.IsCancellationRequested) return ElevDataPrerequisiteCheckResult.Cancelled;
                    _useElevData = false;
                    return ElevDataPrerequisiteCheckResult.InOrder;
                case MapRepreManager.NeedsElevDataIndic.Yes:
                    if (ct.IsCancellationRequested) return ElevDataPrerequisiteCheckResult.Cancelled;
                    if (Map is IGeoLocatedMap geoLocatedMap)
                        switch (ElevDataManager.Instance.AreElevDataOfTypeObtainableFor(geoLocatedMap, ElevDataType, ct))
                        {
                            case ElevDataManager.ElevDataObtainability.Obtainable:
                                _useElevData = true;
                                return ElevDataPrerequisiteCheckResult.InOrder;
                            case ElevDataManager.ElevDataObtainability.ElevDataNotPresent:
                                return ElevDataPrerequisiteCheckResult.ElevDataForMapNotPresent;
                            case ElevDataManager.ElevDataObtainability.NotSupportedMap:
                                return ElevDataPrerequisiteCheckResult.MapNotSupportedByElevDataType;
                            case ElevDataManager.ElevDataObtainability.Cancelled:
                                return ElevDataPrerequisiteCheckResult.Cancelled;
                            default:
                                throw new InvalidEnumArgumentException();
                        }
                    else return ElevDataPrerequisiteCheckResult.MapNotSupportedByElevDataType;
                case MapRepreManager.NeedsElevDataIndic.NotNecessary:
                    if (ct.IsCancellationRequested) return ElevDataPrerequisiteCheckResult.Cancelled;
                    if (Map is IGeoLocatedMap glm)
                        _useElevData = (ElevDataManager.Instance.AreElevDataOfTypeObtainableFor(glm, ElevDataType, ct)) switch
                        {
                            ElevDataManager.ElevDataObtainability.Obtainable => true,
                            _ => false
                        };
                    else _useElevData = false;
                    return ElevDataPrerequisiteCheckResult.InOrder;
                case null:
                    throw new InvalidOperationException( "Given Map`s format and Template is not usable combination for MapRepreRepresentative.");
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        public override async Task CreateMapRepreAsync(IProgress<string> progressInfo, IProgress<GraphCreationReport> mapCreationProgress, CancellationToken cancellationToken)
        {
            if (_useElevData)
            {
                if (Map is IGeoLocatedMap geoLocatedMap)
                {
                    progressInfo.Report("Acquiring elevation data"); //TODO: localize
                    IElevData elevData = await Task.Run(() =>
                        ElevDataManager.Instance.GetElevDataOfTypeFor(geoLocatedMap, ElevDataType, cancellationToken));
                    if (cancellationToken.IsCancellationRequested) return;
                    
                    progressInfo.Report("Creating map representation"); //TODO: localize
                    MapRepresentation = await Task.Run(() => 
                        MapRepreManager.Instance.CreateMapRepre(Template, geoLocatedMap, MapRepreRepresentative, elevData, mapCreationProgress, cancellationToken));
                    if (cancellationToken.IsCancellationRequested) MapRepresentation = null;
                }
                else throw new InvalidOperationException("There is some error in prerequisites check method, that allows _useElevData to be set to true, when map is not even IGeoLocatedMap.");
            }
            else
            {
                progressInfo.Report("Creating map representation"); //TODO: localize
                MapRepresentation = await Task.Run(() =>
                    MapRepreManager.Instance.CreateMapRepre(Template, Map, MapRepreRepresentative, mapCreationProgress, cancellationToken));
                if (cancellationToken.IsCancellationRequested) MapRepresentation = null;
            }
        }

        public override void CleanMapRepre()
        {
            MapRepresentation = null;
        }

        public PFMapRepreCreatingIntraModelView(PFSettingsIntraModelView settings)
        {
            Settings = settings;
        }

        private ITemplate Template => Settings.Template ?? throw new ArgumentNullException( nameof(Settings.Template), "Template should be set before using PFMapRepreCreatingModelView");
        private IMap Map => Settings.Map ?? throw new  ArgumentNullException(nameof(Settings.Map), "Map should be set before using PFMapRepreCreatingModelView");
        private IMapRepreRepresentative<IMapRepre> MapRepreRepresentative => Settings.MapRepreRepresentative ?? throw new ArgumentNullException( nameof(Settings .MapRepreRepresentative), "Map representation representative should be set before using PFMapRepreCreatingModelView");
        private IElevDataType ElevDataType => Settings.ElevDataType ?? throw new ArgumentNullException( nameof(Settings.ElevDataType), "Elevation data type should be set before using PFMapRepreCreatingModelView");
        public IMapRepre? MapRepresentation { get; private set; }

    }
}
