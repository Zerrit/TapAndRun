using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Architecture.GameStates;
using TapAndRun.Interfaces;
using TapAndRun.MVP.Character;
using TapAndRun.MVP.Character.Model;
using TapAndRun.MVP.CharacterCamera;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Levels.Presenter;
using TapAndRun.MVP.Lose;
using TapAndRun.MVP.Lose.Model;
using TapAndRun.MVP.MainMenu;
using TapAndRun.MVP.MainMenu.Model;
using TapAndRun.MVP.Settings;
using TapAndRun.MVP.Settings.Model;
using TapAndRun.MVP.Wallet;
using TapAndRun.MVP.Wallet.Model;
using TapAndRun.Services.Audio;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TapAndRun.Architecture
{
    public class EntryPoint : MonoBehaviour, IAsyncStartable
    {
        private List<IInitializableAsync> _initializationQueue;

        private GameStateMachine _gameStateMachine;

        [Inject]
        public void Construct(
            ISettingsModel settingsModel,SettingsPresenter settingsPresenter,
            IAudioService audioService,
            ICharacterModel characterModel, CharacterPresenter characterPresenter,
            ICameraModel cameraModel, CameraPresenter cameraPresenter,
            ILevelsModel levelsModel, LevelsPresenter levelsPresenter,
            IMainMenuModel mainMenuModel, MainMenuPresenter mainMenuPresenter,
            ILoseModel loseModel, LosePresenter losePresenter,
            IWalletModel walletModel, WalletPresenter walletPresenter,
            GameStateMachine gameStateMachine)
        {
            _initializationQueue = new List<IInitializableAsync>
            {
                settingsModel,
                settingsPresenter,
                audioService,
                characterModel,
                characterPresenter,
                cameraModel,
                cameraPresenter,
                levelsModel,
                levelsPresenter,
                mainMenuModel,
                mainMenuPresenter,
                loseModel,
                losePresenter,
                walletModel,
                walletPresenter
            };

            _gameStateMachine = gameStateMachine;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            // Initialization of all game entities in the specified order //
            foreach (var entity in _initializationQueue)
            {
                await entity.InitializeAsync(cancellation);
            }
            
            // Initialization and start Game State Machine //
            _gameStateMachine.Initialize();
            _gameStateMachine.StartMachineAsync<MainMenuState>(cancellation).Forget();
        }
    }
}