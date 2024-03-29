using Optepafi.ModelViews.Main;
using Optepafi.ModelViews.ModelCreating;

namespace Optepafi.ViewModels.ModelCreating;

public class ModelCreatingSettingsViewModel : ViewModelBase
{
    public MCSettingsModelView SettingsMv { get; }
    public MainParamsManagingModelView MainParamsManaging { get; }

    public ModelCreatingSettingsViewModel(MCSettingsModelView settingsMv)
    {
        SettingsMv = settingsMv;
        MainParamsManaging = MainParamsManagingModelView.Instance;
    }
}