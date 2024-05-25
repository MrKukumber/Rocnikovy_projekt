using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Input;
using Avalonia.Controls;
using Optepafi.Models;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.ParamsMan;
using Optepafi.Models.ParamsMan.Params;
using Optepafi.Models.SearchingAlgorithmMan;
using Optepafi.ModelViews.Main;
using Optepafi.ViewModels.DataViewModels;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

public class MainSettingsViewModel : ViewModelBase
{
    // private ISearchingAlgorithm _selectedAlgorithm;
    private ElevDataDistributionViewModel? _currentElevDataDistribution;
    private CultureInfo _currentCulture;
    
    private readonly ParamsManager _paramsManager = ParamsManager.Instance;
    private readonly MainSettingsModelView _mainSettingsMv;
    public MainSettingsViewModel(MainSettingsModelView mainSettingsMv)
    {
        _mainSettingsMv = mainSettingsMv;

        _currentElevDataDistribution = mainSettingsMv.CurrentElevDataDistribution;
        _currentCulture = mainSettingsMv.CurrentCulture;
        Assets.Localization.Local.Culture = _currentCulture;
        
        this.WhenAnyValue(x => x.CurrentCulture)
            .Subscribe(currentCulture => _mainSettingsMv.CurrentCulture = currentCulture);
        this.WhenAnyValue(x => x.CurrentElevDataDistribution)
            .Subscribe(currentElevDataDistribution => _mainSettingsMv.CurrentElevDataDistribution = currentElevDataDistribution); 
        
        
        ElevConfigInteraction = new Interaction<ElevConfigViewModel, ElevDataDistributionViewModel?>();
        ElevConfig = new ElevConfigViewModel(_currentElevDataDistribution);
        OpenElevConfigCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            CurrentElevDataDistribution = await ElevConfigInteraction.Handle(ElevConfig);
        });
        GoToMainMenuCommand = ReactiveCommand.Create(() => { });
        
        
    }
    
    // public ObservableCollection<ISearchingAlgorithm> Algorithms { get; }
    // public ISearchingAlgorithm? DefaultAlgorithm
    // {
        // get => _selectedAlgorithm;
        // set => this.RaiseAndSetIfChanged(ref _selectedAlgorithm, value);
    // }

    public ElevDataDistributionViewModel? CurrentElevDataDistribution
    {
        get => _currentElevDataDistribution;
        set => this.RaiseAndSetIfChanged(ref _currentElevDataDistribution, value);
    }

    public ObservableCollection<CultureInfo> Cultures { get; } = [CultureInfo.GetCultureInfo("en-GB"), CultureInfo.GetCultureInfo("sk-SK")];
    public CultureInfo CurrentCulture
    {
        get => _currentCulture;
        set
        {
            Assets.Localization.Local.Culture = value;
            this.RaiseAndSetIfChanged(ref _currentCulture, value);
        }
        
    }
    
    public ReactiveCommand<Unit,Unit> GoToMainMenuCommand { get; }
    public ReactiveCommand<Unit,Unit> OpenElevConfigCommand { get; }
    public Interaction<ElevConfigViewModel, ElevDataDistributionViewModel?> ElevConfigInteraction { get; }
    private ElevConfigViewModel ElevConfig { get; }
}