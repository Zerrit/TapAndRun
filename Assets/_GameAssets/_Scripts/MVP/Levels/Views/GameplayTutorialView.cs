using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TapAndRun.MVP.Levels.Views
{
    public class GameplayTutorialView : MonoBehaviour
    {
        [SerializeField] private Image _cursor;

        private void Start()
        {
            PlayTapAnimAsync();
        }

        public void PlayTapAnimAsync()
        {
            _cursor.transform.DOScale(new Vector3(0.8f, 0.8f, 1f), 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine)
                .AwaitForComplete(TweenCancelBehaviour.KillAndCancelAwait, destroyCancellationToken);
        }
    }
}