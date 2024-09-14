using System;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Wallet.Model
{
    public interface ISelfWalletModel
    {
        public ReactiveProperty<bool> IsTutorialDisplaying { get; }

        ReactiveProperty<int> AvailableCrystals { get; }
        ReactiveProperty<int> CrystalsByRun { get; }
    }
}