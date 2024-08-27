using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Wallet.Model
{
    public class WalletModel : IWalletModel, ISelfWalletModel
    {
        public ReactiveProperty<int> AvailableCrystals { get; private set; }
        public ReactiveProperty<int> CrystalsByLevel { get; private set; }

        public UniTask InitializeAsync(CancellationToken token)
        {
            //TODO Try Load DATA
            
            AvailableCrystals = new ReactiveProperty<int>(0);
            CrystalsByLevel = new ReactiveProperty<int>(0);
            
            return UniTask.CompletedTask;
        }

        public void IncreaseCrystalsByLevel()
        {
            CrystalsByLevel.Value++;
        }

        public void GainCrystalsByLevel()
        {
            AvailableCrystals.Value += CrystalsByLevel.Value;
            CrystalsByLevel.Value = 0;
        }

        public void ResetCrystalsByLevel()
        {
            CrystalsByLevel.Value = 0;
        }
    }
}