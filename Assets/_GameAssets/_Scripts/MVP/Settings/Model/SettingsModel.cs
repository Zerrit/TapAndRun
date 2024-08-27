using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Settings.Model
{
    public interface ISettingsModel : IInitializableAsync
    {
        ReactiveProperty<bool> IsDisplaying { get; }
    }

    public interface ISelfSettingsModel
    {
        ReactiveProperty<bool> IsDisplaying { get; }

        bool AudioStatus { get; set; }
        bool VibroStatus { get; set; }
        string Language { get; set; }
    }

    public class SettingsModel : ISettingsModel, ISelfSettingsModel
    {
        public ReactiveProperty<bool> IsDisplaying { get; set; }
        
        public bool AudioStatus { get; set; }
        public bool VibroStatus { get; set; }
        public string Language { get; set; }
        
        public SettingsModel()
        {
        }

        public UniTask InitializeAsync(CancellationToken token)
        {
            IsDisplaying = new ReactiveProperty<bool>(false);
            
            //TODO Try Load Data

            AudioStatus = true;
            VibroStatus = true;
            Language = "en";
            
            return UniTask.CompletedTask;
        }
    }
}