using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TapAndRun.UI
{
    public abstract class ScreenView : MonoBehaviour
    {
        [field:SerializeField] public Transform Parent { get; private set; }
        
        public virtual void Show()
        {
            Parent.gameObject.SetActive(true);
        }

        public async UniTask ShowAsync(CancellationToken token)
        {
            
        }
        
        public virtual void Hide()
        {
            Parent.gameObject.SetActive(false);
        }
        
        public async UniTask HideAsync(CancellationToken token)
        {
            
        }
    }
}