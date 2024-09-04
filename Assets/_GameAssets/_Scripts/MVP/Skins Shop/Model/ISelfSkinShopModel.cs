using System.Collections.Generic;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Skins_Shop.Model
{
    public interface ISelfSkinShopModel
    {
        ReactiveProperty<bool> IsDisplaying { get; set; }
        
        TriggerReactiveProperty BackTrigger { get; }

        bool IsAssortmentPrepeared { set; }
        List<string> UnlockedSkins { get; }
    }
}