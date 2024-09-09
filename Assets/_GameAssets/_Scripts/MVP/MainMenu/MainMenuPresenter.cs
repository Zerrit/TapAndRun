using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Factories.LevelButtons;
using TapAndRun.Interfaces;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.MainMenu.Model;
using TapAndRun.MVP.MainMenu.Views;
using TapAndRun.MVP.Settings.Model;
using TapAndRun.MVP.Skins_Shop.Model;
using TapAndRun.Services.Audio;

namespace TapAndRun.MVP.MainMenu
{
    public class MainMenuPresenter : IInitializableAsync, IDecomposable
    {
        private CancellationTokenSource _cts;

        private readonly ISelfMainMenuModel _model;
        private readonly ILevelsModel _levelsModel;
        private readonly ILevelButtonFactory _levelButtonFactory;
        private readonly ISettingsModel _settingsModel;
        private readonly IAudioService _audioService;
        private readonly MainMenuView _view;
        private readonly LevelSelectView _levelSelectView;

        public MainMenuPresenter(ISelfMainMenuModel model, ILevelsModel levelsModel, ILevelButtonFactory levelButtonFactory,
            ISettingsModel settingsModel, IAudioService audioService, MainMenuView view, LevelSelectView levelSelectView)
        {
            _model = model;
            _levelsModel = levelsModel;
            _levelButtonFactory = levelButtonFactory;
            _view = view;
            _levelSelectView = levelSelectView;
            _settingsModel = settingsModel;
            _audioService = audioService;
        }

        public async UniTask InitializeAsync(CancellationToken token)
        {
            _cts = new CancellationTokenSource();

            await FillLevelSelectAsync(token);
            
            _model.IsDisplaying.OnChanged += UpdateDisplaying;

            _view.PlayButton.onClick.AddListener(StartPlay);
            _view.SettingsButton.onClick.AddListener(OpenSettings);
            _view.LevelSelectButton.onClick.AddListener(OpenLevelSelect);
            _view.SkinsShopButton.onClick.AddListener(_model.SkinShopTrigger.Trigger);
            _levelSelectView.BackButton.onClick.AddListener(CloseLevelSelect);

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

        private void StartPlay()
        {
            _audioService.PlaySound("Play");
            _model.PlayTrigger.Trigger();
        }
        
        private void OpenSettings()
        {
            _audioService.PlaySound("Button");
            _settingsModel.IsDisplaying.Value = true;
        }

        private void OpenLevelSelect()
        {
            _audioService.PlaySound("SwooshIn");
            _levelSelectView.UpdateButtons(_levelsModel.LastUnlockedLevelId);

            _levelSelectView.Show();
        }

        private void CloseLevelSelect()
        {
            _audioService.PlaySound("SwooshOut");
            _levelSelectView.Hide();
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
                    _levelsModel.SelectLevel(levelButton.LevelId);
                    await _levelSelectView.HideAsync(token);
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
            
            _levelButtonFactory.Decompose();
        }

        public void Decompose()
        {
            _cts?.Cancel();
            _cts?.Dispose();

            _model.IsDisplaying.OnChanged -= UpdateDisplaying;

            _view.PlayButton.onClick.RemoveListener(StartPlay);
            _view.SettingsButton.onClick.RemoveListener(OpenSettings);
            _view.LevelSelectButton.onClick.RemoveListener(OpenLevelSelect);
            _view.SkinsShopButton.onClick.RemoveListener(_model.SkinShopTrigger.Trigger);
            _levelSelectView.BackButton.onClick.RemoveListener(CloseLevelSelect);
        }
    }
}