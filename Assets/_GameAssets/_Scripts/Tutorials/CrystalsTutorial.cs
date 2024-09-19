using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Wallet.Model;
using UnityEngine;

namespace TapAndRun.Tutorials
{
    public class CrystalsTutorial : IInitializableAsync, IDecomposable
    {
        private readonly ILevelsModel _levelsModel;
        private readonly IWalletTutorial _walletTutorial;

        public CrystalsTutorial(ILevelsModel levelsModel, IWalletTutorial walletTutorial)
        {
            _levelsModel = levelsModel;
            _walletTutorial = walletTutorial;
        }

        public UniTask InitializeAsync(CancellationToken token)
        {
            if (!_levelsModel.IsTutorialComplete)
            {
                _levelsModel.OnCrystalTaken += ShowTutorial;
            }

            return UniTask.CompletedTask;
        }
 
        private void ShowTutorial()
        {
            Time.timeScale = 0f;
            _walletTutorial.IsTutorialDisplaying.Value = true;

            _walletTutorial.OnTutorialClickTrigger.OnTriggered += HideTutorial;
            _levelsModel.OnLevelFailed += HideTutorial;
        }

        private void HideTutorial()
        {
            _levelsModel.OnCrystalTaken -= ShowTutorial; 
            _walletTutorial.OnTutorialClickTrigger.OnTriggered -= HideTutorial;
            _levelsModel.OnLevelFailed -= HideTutorial;

            _walletTutorial.IsTutorialDisplaying.Value = false;
            Time.timeScale = 1f;
        }

        public void Decompose()
        {
            _levelsModel.OnCrystalTaken -= ShowTutorial;
        }
    }
}