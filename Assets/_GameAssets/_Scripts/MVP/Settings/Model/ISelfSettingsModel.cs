using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Settings.Model
{
    public interface ISelfSettingsModel
    {
        ReactiveProperty<bool> IsDisplaying { get; }

        ReactiveProperty<bool> AudioStatus { get; }
        ReactiveProperty<bool> VibroStatus { get; }
        ReactiveProperty<string> Language { get; }
    }
}