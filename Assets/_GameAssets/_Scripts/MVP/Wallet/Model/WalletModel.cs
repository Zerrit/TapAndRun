using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.PlayerProgress;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.Wallet.Model
{
    public class WalletModel : IWalletModel, ISelfWalletModel, ISaveLoadable
    {
        public string SaveKey => "Wallet";

        public ReactiveProperty<bool> IsTutorialDisplayed { get; private set; }

        public ReactiveProperty<int> AvailableCrystals { get; private set; }
        public ReactiveProperty<int> CrystalsByLevel { get; private set; }

        public UniTask InitializeAsync(CancellationToken token)
        {
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

        SaveLoadData ISaveLoadable.GetSaveLoadData()
        {
            return new SaveLoadData(SaveKey, new object[] {AvailableCrystals.Value});
        }

        void ISaveLoadable.RestoreValue(SaveLoadData loadData)
        {
            if (loadData?.Data == null || loadData.Data.Length < 1)
            {
                Debug.LogError($"Can't restore Wallet data");
                return;
            }

            AvailableCrystals.Value = (int)loadData.Data[0];
        }
    }
}