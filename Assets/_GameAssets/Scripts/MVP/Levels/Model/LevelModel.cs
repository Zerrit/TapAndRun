using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TapAndRun.Character.Commands;
using TapAndRun.MVP.Character.Model;

namespace TapAndRun.MVP.Levels.Model
{
    public class LevelsModel
    {
        public event Action OnLevelChanged;
        public event Action OnLevelCompleted;
        public int CurrentLevelId { get; private set; }

        public int LastUnlockedLevelId { get; private set; }
        public int LevelCount { get; private set; }
        public bool IsLevelBuild { private get; set; }
        
        private Queue<ICommand> _currentLevelCommands;

        private readonly CharacterModel _characterModel;

        public LevelsModel(CharacterModel characterModel)
        {
            _characterModel = characterModel;
        }

        public void Initialize()
        {
            LevelCount = 2;
            LastUnlockedLevelId = 1;
            CurrentLevelId = 0;
            
            OnLevelChanged?.Invoke();
        }

        public async UniTask ChangeLevelAsync(int levelId)
        {
            CurrentLevelId = levelId;
            OnLevelChanged?.Invoke();

            await UniTask.WaitUntil(() => IsLevelBuild);

            IsLevelBuild = false;
        }

        public void CompleteLevel()
        {
            if (LevelCount - 1 > CurrentLevelId)
            {
                if (LastUnlockedLevelId == CurrentLevelId)
                {
                    LastUnlockedLevelId++;
                }

                CurrentLevelId++;
                OnLevelCompleted?.Invoke();
            }
        }
        
        public void SetCommands(IEnumerable<InteractType> interactions)
        {
            
        }
        
        
        private int currentArrowId;
        //private int currentLevelId;

        //private Level newestLevel;

        /*public void CreateStartLevel(int levelId)
        {
            newestLevel = levelConstructor.ConstructStartLevel(levelId);

            currentArrowId = 0;
            //currentLevelId = 0;


            levels.Add(newestLevel);

            foreach (Arrow arr in newestLevel.arrow)
            {
                arrows.Add(arr);
                //player.commands.Add(arr._commandType);
            }

            arrows[currentArrowId].Activate();

            GameManager.instance.SetDifficulty(levels[0].difficulty);
            player.StartCharacter();

            CreateNextLevel();
        }

        public void CreateNextLevel()
        {
            newestLevel = levelConstructor.ConstructNextLevel(newestLevel);
            newestLevel.transform.Translate(new Vector3(0, 3.95f, 0), Space.Self);

            levels.Add(newestLevel);

            foreach (Arrow arr in newestLevel.arrow)
            {
                arrows.Add(arr);
                //player.commands.Add(arr._commandType);
            }

            if (levels.Count > 3) ClearLastLevel();
        }

        public void RestartLevel() // Рестарт сцены с удаление лишних уровней и перемещением персонажа в в начало текущего уровня
        {
            if (levels.Count > 2)
            {
                ClearLastLevel();
            }
            levels[0].ResetArrows();
            levels[1].ResetArrows();

            currentArrowId = 0;
            arrows[currentArrowId].Activate();
            player.MoveToStart(levels[0].transform.position, levels[0].levelRotation);

            int diff = levels[0].difficulty;
            GameManager.instance.SetDifficulty(diff);
            player.ChangeSpeedDifficulty(diff);
        }

        private void ClearLastLevel() // Удаляет самый старый уровень на сцене (Удаляет команды удалённой сцены из листа комманд персонажа и пройденые стрелки)
        {
            int skippedArrow = levels[0].arrow.Count;

            arrows.RemoveRange(0, skippedArrow);
            currentArrowId -= skippedArrow;
            levels[0].DeleteLevel();
            levels.RemoveAt(0);

            player.ClearOldCommands(skippedArrow);
        }

        public void ClearAllLevels() // Перемещает персонажа в нулевое значение, после чего удаляет все существующие уровни
        {
            player.SetDefaultPos();

            while (levels.Count > 0)
            {
                ClearLastLevel();
            }
        }

        public void SwitchArrow(ArrowType commandType)
        {
            arrows[currentArrowId].TurnOff();
            currentArrowId++;

            arrows[currentArrowId].Activate();
        }*/
    }
}