using TapAndRun.Interfaces;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.MainMenu.Model
{
    public interface IMainMenuModel : IInitializableAsync
    {
        ReactiveProperty<bool> IsDisplaying { get; }

        TriggerReactiveProperty PlayTrigger { get; }
        TriggerReactiveProperty SkinShopTrigger { get; }
    }
}