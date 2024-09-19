using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TapAndRun.MVP.Character.Model;
using TapAndRun.MVP.Wallet.Model;
using TapAndRun.PlayerData;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.Skins_Shop.Model
{
    public class SkinShopModel : ISelfSkinShopModel, ISkinShopModel, IProgressable
    {
        public string SaveKey => "Skins";

        public BoolReactiveProperty IsDisplaying { get; set; }

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
            IsDisplaying = new BoolReactiveProperty();
            BackTrigger = new TriggerReactiveProperty();

            UnlockedSkins = new List<string> 
                {_characterModel.SelectedSkin.Value};

            return UniTask.CompletedTask;
        }

        #region SaveLoad

        SaveableData IProgressable.GetProgressData()
        {
            return new (SaveKey, new object[] {UnlockedSkins});
        }

        void IProgressable.RestoreProgress(SaveableData loadData)
        {
            if (loadData?.Data == null || loadData.Data.Length < 1)
            {
                Debug.LogError($"Can't restore Skins data");
                return;
            }

            var skins = ((JArray)loadData.Data[0]).ToObject<List<string>>();
            UnlockedSkins = skins;
        }

        #endregion

        public void SelectCurrentSkin()
        {
            _characterModel.SelectedSkin.Value = CurrentSkinsData.Id;
        }

        public bool TryBuyCurrentSkin()
        {
            if (IsCanPurchase())
            {
                _walletModel.TrySpend(CurrentSkinsData.Price);
                UnlockedSkins.Add(CurrentSkinsData.Id);
                return true;
            }

            return false;
        }

        public bool IsSkinSelected()
        {
            return _characterModel.SelectedSkin.Value == CurrentSkinsData.Id;
        }

        public bool IsUnlocked()
        {
            return UnlockedSkins.Contains(CurrentSkinsData.Id);
        }

        public bool IsCanPurchase()
        {
            return _walletModel.IsEnough(CurrentSkinsData.Price);
        }
    }
}