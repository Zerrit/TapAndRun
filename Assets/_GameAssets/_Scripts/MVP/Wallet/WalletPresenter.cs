using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;
using TapAndRun.MVP.Wallet.Model;
using TapAndRun.MVP.Wallet.View;

namespace TapAndRun.MVP.Wallet
{
    public class WalletPresenter : IInitializableAsync
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
            _walletModel.AvailableCrystals.SubscribeAndUpdate(_walletView.UpdateAvailableCrystals);
            _walletModel.CrystalsByLevel.SubscribeAndUpdate(_walletView.UpdateCrystalsByLevel);

            return UniTask.CompletedTask;
        }
    }
}