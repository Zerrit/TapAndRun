using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.PlayerData;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.Wallet.Model
{
    public class WalletModel : ISelfWalletModel, IWalletModel, IProgressable, IWalletTutorial
    {
        public string SaveKey => "Wallet";

        public ReactiveProperty<bool> IsTutorialDisplaying { get; private set; }

        public ReactiveProperty<int> AvailableCrystals { get; private set; }
        public ReactiveProperty<int> CrystalsByRun { get; private set; }

        private int _crystalsByLevel;
        private int _crystalsByCompletedLevels;

        public UniTask InitializeAsync(CancellationToken token)
        {
            IsTutorialDisplaying = new ReactiveProperty<bool>();
            AvailableCrystals = new ReactiveProperty<int>(50);
            CrystalsByRun = new ReactiveProperty<int>(0);

            return UniTask.CompletedTask;
        }

        SaveableData IProgressable.GetProgressData()
        {
            return new (SaveKey, new object[] {AvailableCrystals.Value + _crystalsByCompletedLevels});
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

            AvailableCrystals.Value -= value;
            return true;
        }

        public void IncreaseCrystalsByRun(int levelsCombo = 1)
        { 
            _crystalsByLevel += levelsCombo; //TODO Добавить подсчёт множителя из конфига

            CrystalsByRun.Value = _crystalsByLevel + _crystalsByCompletedLevels;
        }

        public void SaveCrystals()
        {
            _crystalsByCompletedLevels += _crystalsByLevel;
        }

        public void GainCrystalsByRun()
        {
            AvailableCrystals.Value += _crystalsByCompletedLevels;
            
            CrystalsByRun.Value = 0;
            _crystalsByLevel = 0;
            _crystalsByCompletedLevels = 0;
        }
}
}