using System.Collections.Generic;
using Optepafi.Models.Graphics.Objects;

namespace Optepafi.Models.GraphicsMan;

public interface IGraphicsObjectCollector
{

    public void Add<TGraphicObject>(TGraphicObject graphicObject) where TGraphicObject : IGraphicObject;
    public void AddRange(IEnumerable<IGraphicObject> graphicObjects);
}