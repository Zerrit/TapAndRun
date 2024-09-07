﻿using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Architecture.GameStates;
using TapAndRun.Interfaces;
using TapAndRun.MVP.Character;
using TapAndRun.MVP.Character.Model;
using TapAndRun.MVP.CharacterCamera;
using TapAndRun.MVP.CharacterCamera.Model;
using TapAndRun.MVP.Levels;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Lose;
using TapAndRun.MVP.Lose.Model;
using TapAndRun.MVP.MainMenu;
using TapAndRun.MVP.MainMenu.Model;
using TapAndRun.MVP.Settings;
using TapAndRun.MVP.Settings.Model;
using TapAndRun.MVP.Skins_Shop;
using TapAndRun.MVP.Skins_Shop.Model;
using TapAndRun.MVP.Wallet;
using TapAndRun.MVP.Wallet.Model;
using TapAndRun.Services.Audio;
using TapAndRun.Services.Localization;
using TapAndRun.Services.Progress;
using TapAndRun.Services.Transition;
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
            IAudioService audioService, ILocalizationService localizationService,
            ICharacterModel characterModel, CharacterPresenter characterPresenter,
            ICameraModel cameraModel, CameraPresenter cameraPresenter,
            ILevelsModel levelsModel, LevelsPresenter levelsPresenter,
            IMainMenuModel mainMenuModel, MainMenuPresenter mainMenuPresenter,
            ILoseModel loseModel, LosePresenter losePresenter,
            IWalletModel walletModel, WalletPresenter walletPresenter,
            ISkinShopModel skinShopModel, SkinShopPresenter skinShopPresenter,
            ITransitionService transitionService, IDataService dataService,
            GameStateMachine gameStateMachine)
        {
            _initializationQueue = new List<IInitializableAsync> //TODO Рассмотреть вариант с добавлением в интерфейс свойства : "Приоритет"...
                                                                 //TODO и обрабатывать их согласно приоритету. Это позволить упростить EntryPoint до перечисления с типом IInitializableAsync
            {
                settingsModel,
                settingsPresenter,
                audioService,
                localizationService,
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
                walletPresenter,
                skinShopModel,
                skinShopPresenter,
                transitionService,
                dataService
            };

            _gameStateMachine = gameStateMachine;
        }

        // Initialization of all game entities in the specified order 
        public async UniTask StartAsync(CancellationToken cancellation)
        {
            foreach (var entity in _initializationQueue)
            {
                await entity.InitializeAsync(cancellation);
            }

            _gameStateMachine.Initialize();
            _gameStateMachine.StartMachineAsync<MainMenuState>(cancellation).Forget();
        }
    }
}