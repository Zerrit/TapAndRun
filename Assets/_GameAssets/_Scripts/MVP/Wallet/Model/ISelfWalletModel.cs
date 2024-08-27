using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Wallet.Model
{
    public interface ISelfWalletModel
    {
        ReactiveProperty<int> AvailableCrystals { get; }
        ReactiveProperty<int> CrystalsByLevel { get; }
    }
}