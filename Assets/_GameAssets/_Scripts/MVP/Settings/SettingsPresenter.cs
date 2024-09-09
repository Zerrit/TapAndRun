using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Configs;
using TapAndRun.Factories.LangButtons;
using TapAndRun.Interfaces;
using TapAndRun.MVP.Settings.Model;
using TapAndRun.MVP.Settings.Views;
using TapAndRun.Services.Audio;
using UnityEngine.UI;

namespace TapAndRun.MVP.Settings
{
    public class SettingsPresenter : IInitializableAsync, IDecomposable
    {
        private readonly ILangButtonFactory _langButtonFactory;
        private readonly IAudioService _audioService;
        private readonly ISelfSettingsModel _model;
        private readonly SettingView _view;

        public SettingsPresenter(ISelfSettingsModel model, ILangButtonFactory langButtonFactory, 
            IAudioService audioService, SettingView view)
        {
            _model = model;
            _view = view;
            _langButtonFactory = langButtonFactory;
            _audioService = audioService;
        }

        public async UniTask InitializeAsync(CancellationToken token)
        {
            _view.AudioToggle.SetState(_model.AudioStatus.Value);
            _view.VibroToggle.SetState(_model.VibroStatus.Value);

            await InitLangPanelAsync(token);

            _model.IsDisplaying.OnChanged += UpdateDisplaying;

            _view.AudioToggle.OnStatusChanged += ChangeAudioStatus;
            _view.VibroToggle.OnStatusChanged += ChangeVibroStatus;

            _view.CloseButton.onClick.AddListener(Close);
            _view.LanguagueButton.onClick.AddListener(_view.LanguagePopup.Show);
            
            await UniTask.CompletedTask;
        }

        private void Close()
        {
            _audioService.PlaySound("Button");
            _model.IsDisplaying.Value = false;
        }

        private async UniTask InitLangPanelAsync(CancellationToken token)
        {
            for (int i = 0; i < _langButtonFactory.GetLangCount(); i++)
            {
                var button = await _langButtonFactory.CreateAsynс(i, _view.LanguagePopup.Content.transform, token);

                if (button.LanguageConfig.Id.Equals(_model.Language.Value))
                {
                    _view.LanguagueButton.image.sprite = button.LanguageConfig.Icon;
                }
                
                button.OnClicked += ChangeLanguage;
            }
            
            _langButtonFactory.Decompose();
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

        private void ChangeAudioStatus()
        {
            _model.AudioStatus.Value = !_model.AudioStatus.Value;

            _view.AudioToggle.Switch(_model.AudioStatus.Value);
        }

        private void ChangeVibroStatus()
        {
            _model.VibroStatus.Value = !_model.VibroStatus.Value;

            _view.VibroToggle.Switch(_model.VibroStatus.Value);
        }

        private void ChangeLanguage(LanguageConfig langConfig)
        {
            _model.Language.Value =  langConfig.Id;
            _view.LanguagueButton.image.sprite = langConfig.Icon;

            _view.LanguagePopup.Hide();
        }

        public void Decompose()
        {
            _model.IsDisplaying.OnChanged -= UpdateDisplaying;

            _view.CloseButton.onClick.RemoveListener(Close);
            _view.LanguagueButton.onClick.RemoveListener(_view.LanguagePopup.Show);

            _view.AudioToggle.OnStatusChanged -= ChangeAudioStatus;
            _view.VibroToggle.OnStatusChanged -= ChangeVibroStatus;
        }
    }
}