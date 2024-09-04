using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.MainMenu.Model
{
    public interface ISelfMainMenuModel
    {
        ReactiveProperty<bool> IsDisplaying { get; }

        TriggerReactiveProperty PlayTrigger { get; }
        TriggerReactiveProperty SkinShopTrigger { get; }
    }
}