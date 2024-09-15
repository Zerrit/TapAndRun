using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Wallet.Model
{
    public interface IWalletTutorial
    {
        ReactiveProperty<bool> IsTutorialDisplaying { get; }
        TriggerReactiveProperty OnTutorialClickTrigger { get; }
    }
}