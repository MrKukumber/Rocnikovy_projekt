using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Windows.Input;
using ExCSS;
using Optepafi.ModelViews.Main;
using Optepafi.ModelViews.ModelCreating;
using Optepafi.ViewModels.ModelCreating;
using Optepafi.ViewModels.PathFinding;
using Optepafi.Views.ModelCreating;
using Optepafi.Views.PathFinding;
using ReactiveUI;
using PathFindingSessionModelView = Optepafi.ModelViews.PathFinding.PathFindingSessionModelView;

namespace Optepafi.ViewModels.Main;

public class MainMenuViewModel : ViewModelBase
{
    private MainSettingsModelView _mainSettingsModelView;
    
    private const int SessionsMaxCount = 8;
    public ObservableCollection<SessionViewModel> Sessions { get; } = new();
    public MainMenuViewModel(MainSettingsModelView mainSettingsMv)
    {
        _mainSettingsModelView = mainSettingsMv;
        IObservable<bool> isSessionsCountNotMaximal = Sessions.WhenAnyValue(
            s => s.Count,
             count => count < SessionsMaxCount);
        GoToSettingsCommand = ReactiveCommand.Create(() => { });
        CreatePathFindingSessionCommand = ReactiveCommand.Create(() =>
            {
                var pathFindingSession = new PathFindingSessionViewModel(new PathFindingSessionModelView(), _mainSettingsModelView);
                Sessions.Add(pathFindingSession);
                pathFindingSession.WhenAnyObservable(x => x.OnClosedCommand)
                    .Subscribe(_ => Sessions.Remove(pathFindingSession));
                return pathFindingSession;
            },
            isSessionsCountNotMaximal);
        CreateModelCreatingSessionCommand = ReactiveCommand.Create(() =>
            {
                var modelCreatingSession = new ModelCreatingSessionViewModel(new ModelCreatingSessionModelView());
                Sessions.Add(modelCreatingSession);
                modelCreatingSession.WhenAnyObservable(x => x.OnClosedCommand)
                    .Subscribe(_ => Sessions.Remove(modelCreatingSession));
                return modelCreatingSession;
            },
            isSessionsCountNotMaximal);
    }
    
    public ReactiveCommand<Unit,Unit> GoToSettingsCommand { get; }
    public ReactiveCommand<Unit, PathFindingSessionViewModel> CreatePathFindingSessionCommand { get; }
    public ReactiveCommand<Unit, ModelCreatingSessionViewModel> CreateModelCreatingSessionCommand{ get; }
}