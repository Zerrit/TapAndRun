using System;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Wallet.Model
{
    public interface ISelfWalletModel
    {
        ReactiveProperty<bool> IsTutorialDisplayed { get; }

        ReactiveProperty<int> AvailableCrystals { get; }
        ReactiveProperty<int> CrystalsByLevel { get; }
    }
}