using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan.MapRepreConstrs;

public interface IMapRepreConstructor< out TTemplate, out TMap, out TMapRepre> 
    where TTemplate : ITemplate where TMap : IMap  where TMapRepre : IMapRepresentation<TTemplate>
{
    TTemplate UsedTemplate { get; }
    IMapFormat<TMap> UsedMapFormat { get; }
    bool RequiresElevData { get; }
}