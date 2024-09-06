using System.Collections.Generic;
using TapAndRun.Configs;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Skins_Shop.Model
{
    public interface ISelfSkinShopModel
    {
        ReactiveProperty<bool> IsDisplaying { get; set; }
        
        TriggerReactiveProperty BackTrigger { get; }

        bool IsAssortmentPrepeared { set; }
        SkinData CurrentSkinsData { get; set; }
        List<string> UnlockedSkins { get; }

        void SelectCurrentSkin();
        bool TryBuyCurrentSkin();
        bool IsSkinSelected();
        bool IsUnlocked();
        bool IsCanPurchase();
    }
}