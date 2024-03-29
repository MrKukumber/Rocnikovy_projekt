using System.IO;

namespace Optepafi.ModelViews.ModelCreating;

public class PathFindingSessionModelView : SessionModelView
{
    public PFSettingsModelView Settings { get; }
    public PFGraphCreatingModelView GraphCreating { get; }
    public PFRelevanceFeedbackModelView RelevanceFeedback { get; }
    public PFPathFindingModelView PathFinding { get; }
    
    public PathFindingSessionModelView()
    {
        /*Model budu staticke triedy, ktore proste na vyziadanie budu dorucovat sluzby*/
        var settingsIntra = new PFSettingsIntraModelView();
        var graphCreatingIntra = new PFGraphCreatingIntraModelView(settingsIntra);
        var relevanceFeedbackIntra = new PFRelevanceFeedbackIntraModelView(settingsIntra, graphCreatingIntra);
        var pathFindingIntra = new PFPathFindingIntraModelView(relevanceFeedbackIntra);

        Settings = settingsIntra;
        GraphCreating = graphCreatingIntra;
        RelevanceFeedback = relevanceFeedbackIntra;
        PathFinding = pathFindingIntra;

    }

    private class PFSettingsIntraModelView : PFSettingsModelView
    {
        
    }
    
    private class PFGraphCreatingIntraModelView : PFGraphCreatingModelView
    {
        private PFSettingsIntraModelView Settings { get; }

        public PFGraphCreatingIntraModelView(PFSettingsIntraModelView settings)
        {
            Settings = settings;
        }
    }
    
    private class PFRelevanceFeedbackIntraModelView : PFRelevanceFeedbackModelView
    {
        private PFSettingsIntraModelView Settings { get; }
        private PFGraphCreatingIntraModelView GraphCreating { get; }

        public PFRelevanceFeedbackIntraModelView(PFSettingsIntraModelView settings, PFGraphCreatingIntraModelView graphCreating)
        {
            Settings = settings;
            GraphCreating = graphCreating;
        }
    }
    
    private class PFPathFindingIntraModelView : PFPathFindingModelView
    {
        private PFRelevanceFeedbackIntraModelView RelevanceFeedback { get; }

        public PFPathFindingIntraModelView(PFRelevanceFeedbackIntraModelView relevanceFeedback)
        {
            RelevanceFeedback = relevanceFeedback;
        }
    }
}