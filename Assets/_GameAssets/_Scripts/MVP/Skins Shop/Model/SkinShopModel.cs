using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Skins_Shop.Model
{
    public class SkinShopModel : ISelfSkinShopModel, ISkinShopModel
    {
        public ReactiveProperty<bool> IsDisplaying { get; set; }

        public TriggerReactiveProperty BackTrigger { get; private set; }

        public bool IsAssortmentPrepeared { get; set; }
        public List<string> UnlockedSkins { get; private set; }

        public UniTask InitializeAsync(CancellationToken token)
        {
            IsDisplaying = new ReactiveProperty<bool>();
            BackTrigger = new TriggerReactiveProperty();

            UnlockedSkins = new List<string> 
                {"Chinchilla"};
            
            return UniTask.CompletedTask;
        }
    }
}