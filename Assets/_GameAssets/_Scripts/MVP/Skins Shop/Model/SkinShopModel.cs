using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Configs;
using TapAndRun.MVP.Character.Model;
using TapAndRun.MVP.Wallet.Model;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Skins_Shop.Model
{
    public class SkinShopModel : ISelfSkinShopModel, ISkinShopModel
    {
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