using TapAndRun.Interfaces;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Skins_Shop.Model
{
    public interface ISkinShopModel : IInitializableAsync
    {
        ReactiveProperty<bool> IsDisplaying { get; }
        
        TriggerReactiveProperty BackTrigger { get; }

        bool IsAssortmentPrepeared { get; }
    }
}