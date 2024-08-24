using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace TapAndRun.MVP.Wallet.View
{
    public class WalletView : MonoBehaviour
    {
        [field:SerializeField] public TextMeshProUGUI AvailableCrystals { get; private set; }
        [field:SerializeField] public TextMeshProUGUI CrystalsByLevel { get; private set; }

        private CancellationTokenSource _cts = new();

        public void UpdateAvailableCrystals(int newValue)
        {
            AvailableCrystals.text = newValue.ToString();
        }

        public void UpdateCrystalsByLevel(int newValue)
        {
            if (newValue <= 0)
            {
                CrystalsByLevel.text = string.Empty;
                return;
            }

            CrystalsByLevel.text = $" +{newValue}";

            ResetTokenSource();
            PlayAnimation(_cts.Token).Forget();
        }

        private async UniTaskVoid PlayAnimation(CancellationToken token)
        {
            await CrystalsByLevel.transform.DOScale(new Vector2(1.3f, 1.3f), 0.1f)
                .SetLoops(2, LoopType.Yoyo)
                .AwaitForComplete(TweenCancelBehaviour.CompleteAndCancelAwait, token);
        }

        private void ResetTokenSource()
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();
        }

        private void OnDestroy()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}