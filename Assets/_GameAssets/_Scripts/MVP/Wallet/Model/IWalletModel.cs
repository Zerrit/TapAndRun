using TapAndRun.Interfaces;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Wallet.Model
{
    public interface IWalletModel : IInitializableAsync
    {
        void IncreaseCrystalsByRun(int levelsCombo = 1);
        void SaveCrystals();
        void GainCrystalsByRun();

        bool IsEnough(int value);
        bool TrySpend(int value);
    }
}