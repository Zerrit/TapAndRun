namespace TapAndRun.MVP.Wallet
{
    public class WalletPresenter
    {
        private readonly ISelfWalletModel _walletModel;
        private readonly WalletView _walletView;


        public WalletPresenter(ISelfWalletModel walletModel, WalletView walletView)
        {
            _walletModel = walletModel;
            _walletView = walletView;
        }

        public void Initialize()
        {
            _walletModel.AvailableCrystals.SubscribeAndUpdate(_walletView.UpdateAvailableCrystals);
            _walletModel.CrystalsByLevel.SubscribeAndUpdate(_walletView.UpdateCrystalsByLevel);
        }
    }
}