using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.MVP.Wallet.Model;
using TapAndRun.PlayerData;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.Levels.Model
{
    public class LevelsModel : ISelfLevelsModel, ILevelsModel, IProgressable, ISettingable
    {
        public event Action OnLevelChanged;
        public event Action OnLevelFailed;
        public event Action OnCrystalTaken;

        public BoolReactiveProperty IsDisplaying { get; private set; }
        public BoolReactiveProperty IsTutorialDisplaying { get; private set; }

        public TriggerReactiveProperty StartupTrigger { get; private set; }
        public TriggerReactiveProperty ResetLevelTrigger { get; private set; }

        public TriggerReactiveProperty OnTapTrigger { get; private set; }
        public TriggerReactiveProperty OnEnterToInteractPointTrigger { get; private set; }

        public ReactiveProperty<int> CurrentDifficulty { get; private set; }

        public string SaveKey => "Levels";

        public int CurrentLevelId
        {
            get
            {
                if (_currentLevel == NonSelectedLevelId)
                {
                    _currentLevel = LastUnlockedLevelId;
                }

                return _currentLevel;
            }
            set => _currentLevel = Mathf.Clamp(value, 0, LevelCount - 1);
        }
        public int NextLevelId
        {
            get
            {
                if (IsLevelExist(CurrentLevelId + 1))
                {
                    return CurrentLevelId + 1;
                }

                return CurrentLevelId;
            }
        }

        public int LastUnlockedLevelId { get; private set; }
        public int LevelCount { get; set; }

        public int MaxDifficulty { get; } = 3;

        public int CrystalsByRun { get; private set; }

        public bool IsTutorialLevel { get; set; }
        public bool IsTutorialComplete { get; set; }

        private int _currentLevel;

        private const int NonSelectedLevelId = -1;
        private const int MinDifficulty = 1;

        private readonly IWalletModel _walletModel;

        public LevelsModel(IWalletModel walletModel)
        {
            _walletModel = walletModel;
        }

        public UniTask InitializeAsync(CancellationToken token)
        {
            IsDisplaying = new BoolReactiveProperty();
            IsTutorialDisplaying = new BoolReactiveProperty();

            StartupTrigger = new TriggerReactiveProperty();
            ResetLevelTrigger = new TriggerReactiveProperty();
            OnTapTrigger = new TriggerReactiveProperty();
            OnEnterToInteractPointTrigger = new TriggerReactiveProperty();

            CurrentDifficulty = new ReactiveProperty<int>(MinDifficulty);

            _currentLevel = NonSelectedLevelId;

            return UniTask.CompletedTask;
        }

        #region SaveLoad

        SaveableData IProgressable.GetProgressData()
        {
            return new (SaveKey, new object[] {LastUnlockedLevelId});
        }

        void IProgressable.RestoreProgress(SaveableData loadData)
        {
            if (loadData?.Data == null || loadData.Data.Length < 1)
            {
                Debug.LogError($"Can't restore Wallet data");
                return;
            }

            LastUnlockedLevelId = Convert.ToInt32(loadData.Data[0]);
        }

        SaveableData ISettingable.GetSettingsData()
        {
            return new(SaveKey, new object[] {IsTutorialComplete});
        }

        void ISettingable.RestoreSettings(SaveableData data)
        {
            if (data?.Data == null || data.Data.Length < 1)
            {
                Debug.LogError($"Can't restore LevelsModel settings");
                return;
            }

            IsTutorialComplete = Convert.ToBoolean(data.Data[0]);
        }

        #endregion

        public void SelectLevel(int levelId)
        {
            if (!IsLevelExist(levelId))
            {
                throw new Exception("An attempt to call a non-existent level with id: {levelId}");
            }

            ResetDifficulty();

            if (levelId == CurrentLevelId)
            {
                ResetLevelTrigger.Trigger();
            }
            else
            {
                CurrentLevelId = levelId;
                OnLevelChanged?.Invoke();
            }
        }

        public void AddCrystalByRun()
        {
            CrystalsByRun++;
            _walletModel.IncreaseCrystalsByRun(CurrentDifficulty.Value);

            OnCrystalTaken?.Invoke();
        }

        public void LoseLevel()
        {
            ResetDifficulty();
            _walletModel.GainCrystalsByRun();

            OnLevelFailed?.Invoke();
        }

        public void CompleteLevel()
        {
            if (IsTutorialLevel)
            {
                IsTutorialLevel = false;
                IsTutorialComplete = true;
            }

            _walletModel.SaveCrystals();
            CurrentDifficulty.Value = Mathf.Clamp(++CurrentDifficulty.Value, MinDifficulty, MaxDifficulty);

            if (LevelCount - 1 <= CurrentLevelId)
            {
                Debug.Log("Уровни закончились");
                return;
            }

            if (LastUnlockedLevelId == CurrentLevelId)
            {
                LastUnlockedLevelId++;
            }

            CurrentLevelId++;
        }

        private void ResetDifficulty()
        {
            CurrentDifficulty.Value = MinDifficulty;
        }

        public bool IsLevelExist(int levelId)
        {
            return (levelId < LevelCount);
        }
    }
}