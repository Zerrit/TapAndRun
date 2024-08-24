using TapAndRun.Interfaces;

namespace TapAndRun.MVP.Wallet.Model
{
    public interface IWalletModel : IInitializableAsync
    {
        void IncreaseCrystalsByLevel();
        void GainCrystalsByLevel();
        void ResetCrystalsByLevel();
    }
}