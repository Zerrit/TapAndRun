using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Settings.Model
{
    public class SettingsModel : ISelfSettingsModel, ISettingsModel
    {
        public ReactiveProperty<bool> IsDisplaying { get; set; }

        public ReactiveProperty<bool> AudioStatus { get; private set; }
        public ReactiveProperty<bool> VibroStatus { get; private set; }
        public ReactiveProperty<string> Language { get; private set; }

        public UniTask InitializeAsync(CancellationToken token)
        {
            IsDisplaying = new ReactiveProperty<bool>(false);
            
            //TODO Try Load Data

            AudioStatus = new ReactiveProperty<bool>(true);
            VibroStatus = new ReactiveProperty<bool>(true);
            Language = new ReactiveProperty<string>("en");

            return UniTask.CompletedTask;
        }
    }
}