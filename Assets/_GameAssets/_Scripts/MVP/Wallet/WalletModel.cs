using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Wallet
{
    public interface IWalletModel
    {
        void IncreaseCrystalsByLevel();
        void GainCrystalsByLevel();
        void ResetCrystalsByLevel();
    }

    public interface ISelfWalletModel
    {
        SimpleReactiveProperty<int> AvailableCrystals { get; }
        SimpleReactiveProperty<int> CrystalsByLevel { get; }
    }

    public class WalletModel : IWalletModel, ISelfWalletModel
    {
        public SimpleReactiveProperty<int> AvailableCrystals { get; }
        public SimpleReactiveProperty<int> CrystalsByLevel { get; }
        
        public WalletModel()
        {
            //TODO Try Load DATA
            
            AvailableCrystals = new SimpleReactiveProperty<int>(0);
            CrystalsByLevel = new SimpleReactiveProperty<int>(0);
        }
        
        public void IncreaseCrystalsByLevel()
        {
            CrystalsByLevel.Value++;
        }

        public void GainCrystalsByLevel()
        {
            AvailableCrystals.Value += CrystalsByLevel.Value;
            CrystalsByLevel.Value = 0;
        }

        public void ResetCrystalsByLevel()
        {
            CrystalsByLevel.Value = 0;
        }
    }
}