using System;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Wallet.Model
{
    public interface ISelfWalletModel
    {
        ReactiveProperty<int> AvailableCrystals { get; }
        ReactiveProperty<int> CrystalsByRun { get; }

        ReactiveProperty<bool> IsTutorialDisplaying { get; }
        TriggerReactiveProperty OnTutorialClickTrigger { get; }
    }
}