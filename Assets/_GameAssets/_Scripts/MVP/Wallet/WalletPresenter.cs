using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;
using TapAndRun.MVP.Wallet.Model;
using TapAndRun.MVP.Wallet.View;

namespace TapAndRun.MVP.Wallet
{
    public class WalletPresenter : IInitializableAsync, IDecomposable
    {
        private readonly ISelfWalletModel _walletModel;
        private readonly WalletView _walletView;

        public WalletPresenter(ISelfWalletModel walletModel, WalletView walletView)
        {
            _walletModel = walletModel;
            _walletView = walletView;
        }

        public UniTask InitializeAsync(CancellationToken token)
        {
            _walletModel.AvailableCrystals.Subscribe(_walletView.UpdateAvailableCrystals, true);
            _walletModel.CrystalsByRun.Subscribe(_walletView.UpdateCrystalsByLevel, true);
            _walletModel.IsTutorialDisplaying.Subscribe(UpdateTutorialDisplaying);

            return UniTask.CompletedTask;
        }

        private void UpdateTutorialDisplaying(bool isDislaying)
        {
            if (isDislaying)
            {
                _walletView.WalletTutorialView.Show();
                _walletView.WalletTutorialView.BackgroundButton.onClick.AddListener(_walletModel.OnTutorialClickTrigger.Trigger);
            }
            else
            {
                _walletView.WalletTutorialView.BackgroundButton.onClick.RemoveListener(_walletModel.OnTutorialClickTrigger.Trigger);
                _walletView.WalletTutorialView.Hide();
            }
        }

        public void Decompose()
        {
            _walletModel.AvailableCrystals.Unsubscribe(_walletView.UpdateAvailableCrystals);
            _walletModel.CrystalsByRun.Unsubscribe(_walletView.UpdateCrystalsByLevel);
            _walletModel.IsTutorialDisplaying.Unsubscribe(UpdateTutorialDisplaying);
        }
    }
}