using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Wallet.Model
{
    public interface IWalletTutorial
    {
        BoolReactiveProperty IsTutorialDisplaying { get; }
        TriggerReactiveProperty OnTutorialClickTrigger { get; }
    }
}