using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;
using TapAndRun.MVP.Levels.Model;
using UnityEngine;

namespace TapAndRun.Tutorials
{
    public class TapTutorial : IInitializableAsync, IDecomposable
    {
        private readonly ILevelsModel _levelsModel;

        public TapTutorial(ILevelsModel levelsModel)
        {
            _levelsModel = levelsModel;
        }
        
        public UniTask InitializeAsync(CancellationToken token)
        {
            if (!_levelsModel.IsTutorialComplete)
            {
                _levelsModel.OnEnterToInteractPointTrigger.OnTriggered += ShowTutorial;
            }

            return UniTask.CompletedTask;
        }
        
        private void ShowTutorial()
        {
            Time.timeScale = 0.2f;
            _levelsModel.IsTutorialDisplaying.Value = true;

            _levelsModel.OnTapTrigger.OnTriggered += HideTutorial;
            _levelsModel.OnLevelFailed += HideTutorial;
        }

        private void HideTutorial()
        {
            _levelsModel.OnTapTrigger.OnTriggered -= HideTutorial;
            _levelsModel.OnLevelFailed -= HideTutorial;
            
            _levelsModel.IsTutorialDisplaying.Value = false;
            Time.timeScale = 1f;
        }

        public void Decompose()
        {
            _levelsModel.OnEnterToInteractPointTrigger.OnTriggered -= ShowTutorial;
        }
    }
}