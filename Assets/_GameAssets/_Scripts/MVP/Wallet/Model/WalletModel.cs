using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Wallet.Model
{
    public class WalletModel : IWalletModel, ISelfWalletModel
    {
        public ReactiveProperty<bool> IsTutorialDisplayed { get; private set; }

        public ReactiveProperty<int> AvailableCrystals { get; private set; }
        public ReactiveProperty<int> CrystalsByLevel { get; private set; }

        public UniTask InitializeAsync(CancellationToken token)
        {
            //TODO Try Load DATA

            IsTutorialDisplayed = new ReactiveProperty<bool>();
            AvailableCrystals = new ReactiveProperty<int>(50);
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

        public bool IsEnough(int value)
        {
            return AvailableCrystals.Value >= value;
        }

        public bool TrySpend(int value)
        {
            if (!IsEnough(value))
            {
                return false;
            }
            else
            {
                AvailableCrystals.Value -= value;
                return true;
            }
        }
    }
}