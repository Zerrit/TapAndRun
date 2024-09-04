using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.MainMenu.Model
{
    public class SelfMainMenuModel : ISelfMainMenuModel, IMainMenuModel
    {
        public ReactiveProperty<bool> IsDisplaying { get; private set; }

        public TriggerReactiveProperty PlayTrigger { get; private set; }
        public TriggerReactiveProperty SkinShopTrigger { get; private set; }
        
        public UniTask InitializeAsync(CancellationToken token)
        {
            IsDisplaying = new ReactiveProperty<bool>();
            PlayTrigger = new TriggerReactiveProperty();
            SkinShopTrigger = new TriggerReactiveProperty();

            return UniTask.CompletedTask;
        }
    }
}