using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.PlayerData;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.Settings.Model
{
    public class SettingsModel : ISelfSettingsModel, ISettingsModel, ISettingable
    {
        public string SaveKey => "Settings";

        public ReactiveProperty<bool> IsDisplaying { get; set; }

        public ReactiveProperty<bool> AudioStatus { get; private set; }
        public ReactiveProperty<bool> VibroStatus { get; private set; }
        public ReactiveProperty<string> Language { get; private set; }

        public UniTask InitializeAsync(CancellationToken token)
        {
            IsDisplaying = new ReactiveProperty<bool>();

            AudioStatus = new ReactiveProperty<bool>(true);
            VibroStatus = new ReactiveProperty<bool>(true);
            Language = new ReactiveProperty<string>("en");

            return UniTask.CompletedTask;
        }

        #region SaveLoad

        public SaveableData GetSettingsData()
        {
            return new (SaveKey, new object[] 
            {
                AudioStatus.Value, 
                VibroStatus.Value, 
                Language.Value
            });
        }

        public void RestoreSettings(SaveableData data)
        {
            if (data?.Data == null || data.Data.Length < 1)
            {
                Debug.LogError($"Can't restore SettingsModel settings");
                return;
            }

            AudioStatus.Value = Convert.ToBoolean(data.Data[0]);
            VibroStatus.Value = Convert.ToBoolean(data.Data[1]);
            Language.Value = Convert.ToString(data.Data[2]);
        }

        #endregion
    }
}