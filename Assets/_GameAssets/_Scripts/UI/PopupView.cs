using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TapAndRun.UI
{
    public class PopupView : MonoBehaviour
    {
        [field:SerializeField] public CanvasGroup Fade { get; private set; }
        [field:SerializeField] public Transform Parent { get; private set; }
        
        public void Show()
        {
            Parent.gameObject.SetActive(true);
        }

        public async UniTask ShowAsync(CancellationToken token)
        {
            
        }
        
        public void Hide()
        {
            Parent.gameObject.SetActive(false);
        }
        
        public async UniTask HideAsync(CancellationToken token)
        {
            
        }
    }
}