using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.MVP.Wallet.Model;
using TapAndRun.PlayerData;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.Levels.Model
{
    public class LevelsModel : ISelfLevelsModel, ILevelsModel, IProgressable, ISettingsUser
    {
        public string SaveKey => "Levels";

        public event Action OnLevelChanged;
        public event Action OnLevelFailed;
        public event Action OnCrystalTaken;

        public BoolReactiveProperty IsDisplaying { get; private set; }
        public BoolReactiveProperty IsTutorialDisplaying { get; private set; }

        public TriggerReactiveProperty StartupTrigger { get; private set; }
        public TriggerReactiveProperty ResetLevelTrigger { get; private set; }
        
        public TriggerReactiveProperty OnTapTrigger { get; private set; }
        public TriggerReactiveProperty OnEnterToInteractPointTrigger { get; private set; }
        
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

        public int LastUnlockedLevelId { get; private set; }
        public int LevelCount { get; set; }

        public int CurrentDifficulty { get; set; }
        public int MaxDifficulty { get; } = 3; //TODO Вынести в конфиг

        public int CrystalsByRun { get; private set; } // TODO Для возможной статистики

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

            _currentLevel = NonSelectedLevelId;
            CurrentDifficulty = MinDifficulty;

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

        SaveableData ISettingsUser.GetSettingsData()
        {
            return new(SaveKey, new object[] {IsTutorialComplete});
        }

        void ISettingsUser.RestoreSettings(SaveableData data)
        {
            if (data?.Data == null || data.Data.Length < 1)
            {
                Debug.LogError($"Can't restore LevelsModel settings");
                return;
            }

            IsTutorialComplete = Convert.ToBoolean(data.Data[0]);
        }
        #endregion

        public void SelectLevel(int levelId) //TODO Проверка доступности уровня (изменить тип на bool)
        {
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
            _walletModel.IncreaseCrystalsByRun(CurrentDifficulty);

            OnCrystalTaken?.Invoke();
        }
        
        public void LoseLevel()
        {
            _walletModel.GainCrystalsByRun();
            CurrentDifficulty = MinDifficulty;

            OnLevelFailed?.Invoke();
        }

        public void CompleteLevel() //TODO Сделать бесконечные уровни
        {
            if (IsTutorialLevel)
            {
                IsTutorialLevel = false;
                IsTutorialComplete = true;
            }
            
            if (LevelCount - 1 <= CurrentLevelId)
            {
                throw new Exception("Уровни закончились");
            }

            if (LastUnlockedLevelId == CurrentLevelId)
            {
                LastUnlockedLevelId++;
            }

            CurrentDifficulty = Mathf.Clamp(++CurrentDifficulty, MinDifficulty, MaxDifficulty);
            CurrentLevelId++;
            
            _walletModel.SaveCrystals();
        }

        public bool CheckLevelExist(int levelId)
        {
            return (levelId < LevelCount);
        }
    }
}