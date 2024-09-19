using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Wallet.Model
{
    public interface ISelfWalletModel
    {
        ReactiveProperty<int> AvailableCrystals { get; }
        ReactiveProperty<int> CrystalsByRun { get; }

        BoolReactiveProperty IsTutorialDisplaying { get; }
        TriggerReactiveProperty OnTutorialClickTrigger { get; }
    }
}