using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TapAndRun.Configs;
using TapAndRun.MVP.Character.Model;
using TapAndRun.MVP.Wallet.Model;
using TapAndRun.PlayerProgress;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.Skins_Shop.Model
{
    public class SkinShopModel : ISelfSkinShopModel, ISkinShopModel, ISaveLoadable
    {
        public string SaveKey => "Skins";

        public ReactiveProperty<bool> IsDisplaying { get; set; }

        public TriggerReactiveProperty BackTrigger { get; private set; }

        public bool IsAssortmentPrepeared { get; set; }
        public SkinData CurrentSkinsData { get; set; }

        public List<string> UnlockedSkins { get; private set; }

        private readonly ICharacterModel _characterModel;
        private readonly IWalletModel _walletModel;

        public SkinShopModel(ICharacterModel characterModel, IWalletModel walletModel)
        {
            _characterModel = characterModel;
            _walletModel = walletModel;
        }

        public UniTask InitializeAsync(CancellationToken token)
        {
            IsDisplaying = new ReactiveProperty<bool>();
            BackTrigger = new TriggerReactiveProperty();

            UnlockedSkins = new List<string> 
                {"Chinchilla"};
            
            return UniTask.CompletedTask;
        }

        ProgressData ISaveLoadable.GetProgressData()
        {
            return new ProgressData(SaveKey, new object[] {UnlockedSkins});
        }

        void ISaveLoadable.RestoreProgress(ProgressData loadData)
        {
            if (loadData?.Data == null || loadData.Data.Length < 1)
            {
                Debug.LogError($"Can't restore Skins data");
                return;
            }

            var skins = ((JArray)loadData.Data[0]).ToObject<List<string>>();
            UnlockedSkins = skins;
        }

        public void SelectCurrentSkin()
        {
            _characterModel.SelectedSkin.Value = CurrentSkinsData.Name;
        }
        
        public bool TryBuyCurrentSkin()
        {
            if (IsCanPurchase())
            {
                _walletModel.TrySpend(CurrentSkinsData.Price);
                UnlockedSkins.Add(CurrentSkinsData.Name);
                return true;
            }

            return false;
        }

        public bool IsSkinSelected()
        {
            return _characterModel.SelectedSkin.Value == CurrentSkinsData.Name;
        }

        public bool IsUnlocked()
        {
            return UnlockedSkins.Contains(CurrentSkinsData.Name);
        }

        public bool IsCanPurchase()
        {
            return _walletModel.IsEnough(CurrentSkinsData.Price);
        }
    }
}