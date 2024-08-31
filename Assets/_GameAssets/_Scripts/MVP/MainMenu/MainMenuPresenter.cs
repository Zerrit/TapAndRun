using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Factories.LevelButtons;
using TapAndRun.Interfaces;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.MainMenu.Model;
using TapAndRun.MVP.MainMenu.Views;
using TapAndRun.MVP.Settings.Model;
using UnityEngine;

namespace TapAndRun.MVP.MainMenu
{
    public class MainMenuPresenter : IInitializableAsync, IDisposable
    {
        private CancellationTokenSource _cts;

        private readonly ISelfMainMenuModel _model;
        private readonly ILevelsModel _levelsModel;
        private readonly ILevelButtonFactory _levelButtonFactory;
        private readonly ISettingsModel _settingsModel;
        private readonly MainMenuView _view;
        private readonly LevelSelectView _levelSelectView;

        public MainMenuPresenter(ISelfMainMenuModel model, ILevelsModel levelsModel, ILevelButtonFactory levelButtonFactory,
            ISettingsModel settingsModel, MainMenuView view, LevelSelectView levelSelectView)
        {
            _model = model;
            _levelsModel = levelsModel;
            _levelButtonFactory = levelButtonFactory;
            _view = view;
            _levelSelectView = levelSelectView;
            _settingsModel = settingsModel;
        }

        public async UniTask InitializeAsync(CancellationToken token)
        {
            _cts = new CancellationTokenSource();

            await FillLevelSelectAsync(token);
            
            _model.IsDisplaying.OnChanged += UpdateDisplaying;
            _view.PlayButton.onClick.AddListener(_model.StartGame);
            _view.SettingsButton.onClick.AddListener(OpenSettings);
            _view.LevelSelectButton.onClick.AddListener(OpenLevelSelect);
            _levelSelectView.BackButton.onClick.AddListener(()=> _levelSelectView.Hide());

            await UniTask.CompletedTask;
        }

        private void UpdateDisplaying(bool status)
        {
            if (status)
            {
                _view.Show();
            }
            else
            {
                _view.Hide();
            }
        }

        private void OpenSettings()
        {
            _settingsModel.IsDisplaying.Value = true;
        }

        private void OpenLevelSelect()
        {
            _levelSelectView.UpdateButtons(_levelsModel.LastUnlockedLevelId);

            _levelSelectView.Show();
        }

        private void HandleLevelSelection(LevelButtonView levelButton)
        {
            HandleLevelSelectionAsync(_cts.Token).Forget();
            
            async UniTaskVoid HandleLevelSelectionAsync(CancellationToken token)
            {
                if (levelButton.LevelId > _levelsModel.LastUnlockedLevelId)
                {
                    levelButton.PlayLockAsync().Forget();
                }
                else
                {
                    _levelsModel.PrepareLevel(levelButton.LevelId);
                    await _levelSelectView.HideAsync(token); //TODO Возможно стоит добавить ожидание окончания создания уровня
                }
            }
        }

        private async UniTask FillLevelSelectAsync(CancellationToken token)
        {
            for (int i = 0; i < _levelsModel.LevelCount; i++)
            {
                var button = await _levelButtonFactory.CreateAsynс(_levelSelectView.ButtonsContainer, token);

                _levelSelectView.ButtonList.Add(button);
                button.Initialize(i);

                button.OnClicked += HandleLevelSelection;
            }
        }

        public void Dispose()
        {
            _model.IsDisplaying.OnChanged -= UpdateDisplaying;
            _view.PlayButton.onClick.RemoveListener(_model.StartGame);
            _view.SettingsButton.onClick.RemoveListener(OpenSettings);
        }
    }
}