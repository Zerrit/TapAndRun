using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;
using TapAndRun.Services.Audio;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Settings.Model
{
    public interface ISettingsModel : IInitializableAsync
    {
        ReactiveProperty<bool> IsDisplaying { get; }
        
        public ReactiveProperty<bool> AudioStatus { get; }
        public ReactiveProperty<bool> VibroStatus { get; }
    }

    public interface ISelfSettingsModel
    {
        ReactiveProperty<bool> IsDisplaying { get; }

        ReactiveProperty<bool> AudioStatus { get; }
        ReactiveProperty<bool> VibroStatus { get; }
        
        string Language { get; }
    }

    public class SettingsModel : ISelfSettingsModel, ISettingsModel
    {
        public ReactiveProperty<bool> IsDisplaying { get; set; }

        public ReactiveProperty<bool> AudioStatus { get; private set; }
        public ReactiveProperty<bool> VibroStatus { get; private set; }
        public string Language { get; private set; }

        public SettingsModel()
        {

        }

        public UniTask InitializeAsync(CancellationToken token)
        {
            IsDisplaying = new ReactiveProperty<bool>(false);
            
            //TODO Try Load Data

            AudioStatus = new ReactiveProperty<bool>(true);
            VibroStatus = new ReactiveProperty<bool>(true);
            
            Language = "en";
            
            return UniTask.CompletedTask;
        }
    }
}