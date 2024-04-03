using System.Collections.ObjectModel;
using System.Reactive;
using System.Windows.Input;
using Optepafi.Models;
using Optepafi.Models.ElevationDataMan;
using Optepafi.ModelViews;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

public class ElevConfigViewModel : ViewModelBase
{
    private IElevSource currentElevSource = null;//TODO:tuna bude nieco ine ale zatial aby tam nebol null
    public ReactiveCommand<Unit, IElevSource> ReturnCommand { get; }
    public ElevConfigViewModel()
    {
        ReturnCommand = ReactiveCommand.Create(() => currentElevSource);
    }
    
}
