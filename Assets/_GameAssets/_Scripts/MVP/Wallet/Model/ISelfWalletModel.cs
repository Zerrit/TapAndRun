using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Wallet.Model
{
    public interface ISelfWalletModel
    {
        SimpleReactiveProperty<int> AvailableCrystals { get; }
        SimpleReactiveProperty<int> CrystalsByLevel { get; }
    }
}