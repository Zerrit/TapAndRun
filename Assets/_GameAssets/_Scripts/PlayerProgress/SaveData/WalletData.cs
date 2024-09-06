using TapAndRun.Services.Progress;

namespace TapAndRun.PlayerProgress.SaveData
{
    public class WalletData : SaveLoadData
    {
        public WalletData(string key, int availableCrystals)
            : base(key, new object[] {availableCrystals}) { }
    }
}