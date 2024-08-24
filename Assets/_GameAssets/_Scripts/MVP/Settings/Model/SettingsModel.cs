using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Settings.Model
{
    public interface ISettingsModel : IInitializableAsync
    {
        SimpleReactiveProperty<bool> IsDisplaying { get; }
    }

    public interface ISelfSettingsModel
    {
        SimpleReactiveProperty<bool> IsDisplaying { get; }

        bool AudioStatus { get; set; }
        bool VibroStatus { get; set; }
        string Language { get; set; }
    }

    public class SettingsModel : ISettingsModel, ISelfSettingsModel
    {
        public SimpleReactiveProperty<bool> IsDisplaying { get; set; }
        
        public bool AudioStatus { get; set; }
        public bool VibroStatus { get; set; }
        public string Language { get; set; }
        
        public SettingsModel()
        {
        }

        public UniTask InitializeAsync(CancellationToken token)
        {
            IsDisplaying = new SimpleReactiveProperty<bool>(false);
            
            //TODO Try Load Data

            AudioStatus = true;
            VibroStatus = true;
            Language = "en";
            
            return UniTask.CompletedTask;
        }
    }
}