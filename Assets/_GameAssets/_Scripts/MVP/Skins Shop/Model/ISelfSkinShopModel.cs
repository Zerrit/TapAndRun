using System.Collections.Generic;
using TapAndRun.Configs;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Skins_Shop.Model
{
    public interface ISelfSkinShopModel
    {
        BoolReactiveProperty IsDisplaying { get; }

        TriggerReactiveProperty BackTrigger { get; }

        bool IsAssortmentPrepeared { set; }
        SkinData CurrentSkinsData { get; set; }

        void SelectCurrentSkin();
        bool TryBuyCurrentSkin();
        bool IsSkinSelected();
        bool IsUnlocked();
        bool IsCanPurchase();
    }
}