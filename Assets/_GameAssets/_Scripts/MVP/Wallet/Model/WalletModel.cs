using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.PlayerData;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.Wallet.Model
{
    public class WalletModel : ISelfWalletModel, IWalletModel, IWalletTutorial, IProgressable
    {
        public string SaveKey => "Wallet";

        public ReactiveProperty<int> AvailableCrystals { get; private set; }
        public ReactiveProperty<int> CrystalsByRun { get; private set; }
        
        public ReactiveProperty<bool> IsTutorialDisplaying { get; private set; }
        public TriggerReactiveProperty OnTutorialClickTrigger { get; private set; }

        private int _crystalsByLevel;
        private int _crystalsByCompletedLevels;

        public UniTask InitializeAsync(CancellationToken token)
        {
            AvailableCrystals = new ReactiveProperty<int>(50);
            CrystalsByRun = new ReactiveProperty<int>(0);

            IsTutorialDisplaying = new ReactiveProperty<bool>();
            OnTutorialClickTrigger = new TriggerReactiveProperty();

            return UniTask.CompletedTask;
        }

        #region SaveLoad
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
        #endregion

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
            _crystalsByLevel = 0;
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