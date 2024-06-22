using System.Collections.Generic;
using System.Linq;
using DynamicData;
using Optepafi.Models.Graphics.Objects;
using Optepafi.Models.Graphics.Sources;
using Optepafi.Models.GraphicsMan;

namespace Optepafi.ModelViews.PathFinding.Utils;

public class EditableGroundGraphicsSource : IGroundGraphicsSource
{
    public EditableGroundGraphicsSource(IGroundGraphicsSource groundGraphicsSource)
    {
        GraphicObjects = new SourceList<IGraphicObject>();
        foreach (var graphicObject in groundGraphicsSource.GraphicObjects.Items)
        {
            GraphicObjects.Add(graphicObject);
        }
        GraphicsArea = groundGraphicsSource.GraphicsArea;
        Editor = new GraphicObjectEditor(GraphicObjects);
    }

    public EditableGroundGraphicsSource(GraphicsArea graphicsArea)
    {
        GraphicObjects = new SourceList<IGraphicObject>();
        GraphicsArea = graphicsArea;
        Editor = new GraphicObjectEditor(GraphicObjects);
    }

    public EditableGroundGraphicsSource(IGraphicsSource graphicsSource, GraphicsArea graphicsArea)
    {
        
        GraphicObjects = new SourceList<IGraphicObject>();
        foreach (var graphicObject in graphicsSource.GraphicObjects.Items)
        {
            GraphicObjects.Add(graphicObject);
        }
        GraphicsArea = graphicsArea;
        Editor = new GraphicObjectEditor(GraphicObjects);
    }
    
    public SourceList<IGraphicObject> GraphicObjects { get; }
    public GraphicsArea GraphicsArea { get; }

    public IGraphicObjectEditor Editor { get; } 
    private class GraphicObjectEditor : IGraphicObjectEditor
    {
        private SourceList<IGraphicObject> _graphicsObjectSourceList;
        private List<IGraphicObject> _addedGraphicObjects = new();

        public GraphicObjectEditor(SourceList<IGraphicObject> graphicsObjectSourceList)
        {
            _graphicsObjectSourceList = graphicsObjectSourceList;
        }

        public void Add(IGraphicObject graphicObject)
        {
            _graphicsObjectSourceList.Add(graphicObject);
            _addedGraphicObjects.Add(graphicObject);
        }

        public void AddRange(IEnumerable<IGraphicObject> graphicObjects)
        {
            _graphicsObjectSourceList.AddRange(graphicObjects);
            _addedGraphicObjects.AddRange(graphicObjects);
        }

        public void Remove(IGraphicObject graphicObject)
        {
            _graphicsObjectSourceList.Remove(graphicObject);
            _addedGraphicObjects.Remove(graphicObject);
        }

        public void Clear()
        {
            _graphicsObjectSourceList.RemoveMany(_addedGraphicObjects);
            _addedGraphicObjects.Clear();
        }

        public bool Contains(IGraphicObject graphicObject)
        {
            return _addedGraphicObjects.Contains(graphicObject);
        }
    }
}