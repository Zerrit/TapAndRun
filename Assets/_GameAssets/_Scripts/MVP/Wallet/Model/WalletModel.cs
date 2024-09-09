using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.PlayerData;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.Wallet.Model
{
    public class WalletModel : IWalletModel, ISelfWalletModel, IProgressable
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

        SaveableData IProgressable.GetProgressData()
        {
            return new SaveableData(SaveKey, new object[] {AvailableCrystals.Value});
        }

        void IProgressable.RestoreProgress(SaveableData loadData)
        {
            if (loadData?.Data == null || loadData.Data.Length < 1)
            {
                Debug.LogError($"Can't restore Wallet data");
                return;
            }

            AvailableCrystals.Value = Convert.ToInt32(loadData.Data[0]);
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