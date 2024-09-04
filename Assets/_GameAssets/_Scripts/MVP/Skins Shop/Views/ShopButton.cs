using System.Threading;
using Assets.SimpleLocalization.Scripts;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TapAndRun.MVP.Skins_Shop.Views
{
    public class ShopButton : MonoBehaviour
    {
        [field:SerializeField] public Button Button { get; private set; }

        [SerializeField] private Image _buttonImage;
        [SerializeField] private Text _buttonText;
        [SerializeField] private GameObject _crystalIcon;

        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _selectColor;
        [SerializeField] private Color _availableColor;
        [SerializeField] private Color _unavailableColor;

        [SerializeField] private string _selectedLangKey;
        [SerializeField] private string _selectLangKey;

        private CancellationTokenSource _cts = new();
        
        public void SetSelectedMode()
        {
            Button.interactable = false;
            _crystalIcon.SetActive(false);

            _buttonImage.color = _selectedColor;
            _buttonText.text = LocalizationManager.Localize(_selectedLangKey);
        }

        public void SetSelectMode()
        {
            Button.interactable = true;
            _crystalIcon.SetActive(false);

            _buttonImage.color = _selectColor;
            _buttonText.text = LocalizationManager.Localize(_selectLangKey);
        }
        
        public void SetBuyMode(int price, bool isEnough)
        {
            Button.interactable = true;
            _crystalIcon.SetActive(true);

            _buttonText.text = price.ToString();

            if (isEnough)
            {
                _buttonImage.color = _availableColor;
            }
            else
            {
                _buttonImage.color = _unavailableColor;
            }
        }

        public void PlayFail()
        {
            ResetToken();
            
            Button.transform.DOShakePosition(1f)
                .AwaitForComplete(TweenCancelBehaviour.CompleteAndCancelAwait, _cts.Token);
        }

        private void ResetToken()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
        }
    }
    
}