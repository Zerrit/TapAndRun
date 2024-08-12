using TMPro;
using UnityEngine;

namespace TapAndRun.MVP.Screens.Wallet
{
    public class WalletView : MonoBehaviour
    {
        [field:SerializeField] public TextMeshProUGUI AvailableCrystals { get; private set; }
        [field:SerializeField] public TextMeshProUGUI CrystalsByLevel { get; private set; }

        public void UpdateAvailableCrystals(int newValue)
        {
            AvailableCrystals.text = newValue.ToString();
        }
        
        public void UpdateCrystalsByLevel(int newValue)
        {
            if (newValue <= 0)
            {
                CrystalsByLevel.text = "";
                return;
            }

            CrystalsByLevel.text = $" +{newValue}";
        }
    }
}