using TapAndRun.Interfaces;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Wallet.Model
{
    public interface IWalletModel : IInitializableAsync
    {
        ReactiveProperty<bool> IsTutorialDisplayed { get; }

        void IncreaseCrystalsByLevel();
        void GainCrystalsByLevel();
        void ResetCrystalsByLevel();

        bool IsEnough(int value);
        bool TrySpend(int value);
    }
}