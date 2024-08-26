using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TapAndRun.MVP.Screens.LevelSelect
{
    public class LevelButtonView : MonoBehaviour, IPointerClickHandler
    {
        public event Action<LevelButtonView> OnClicked;
        
        public int LevelId { get; private set; }
        
        [field:SerializeField] public Button Button { get; private set; }
        [field:SerializeField] public Image Background { get; private set; }
        [field:SerializeField] public Image LockIcon { get; private set; }
        [field:SerializeField] public TextMeshProUGUI LevelText { get; private set; }

        [SerializeField] private Color _unlockColor;
        [SerializeField] private Color _lockColor;

        [SerializeField] private Vector2 _lockAnimVector;
        [SerializeField] private float _lockAnimDuration;
        [SerializeField] private int _lockAnimVibrato;

        private CancellationTokenSource _cts = new ();

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke(this);
        }
        
        public void Initialize(int levelId)
        {
            LevelId = levelId;
            LevelText.text = $"{LevelId + 1}";
        }

        public void Lock()
        {
            Background.color = _lockColor;
            LockIcon.gameObject.SetActive(true);
        }

        public void Unlock()
        {
            Background.color = _unlockColor;
            LockIcon.gameObject.SetActive(false);
        }

        public async UniTask PlayLockAsync()
        {
            ResetAnimToken();
            LockIcon.transform.localPosition = Vector3.zero;

            await LockIcon.transform.DOPunchPosition(_lockAnimVector, _lockAnimDuration, _lockAnimVibrato)
                .SetEase(Ease.InBounce)
                .AwaitForComplete(TweenCancelBehaviour.CompleteAndCancelAwait, _cts.Token);
        }

        private void ResetAnimToken()
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();
        }
    }
}