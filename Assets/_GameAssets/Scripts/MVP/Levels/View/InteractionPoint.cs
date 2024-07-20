using DG.Tweening;
using TapAndRun.MVP.Levels.Model;
using UnityEngine;

namespace TapAndRun.MVP.Levels.View
{
    public class InteractionPoint : MonoBehaviour
    {
        [field:SerializeField] public SpriteRenderer Icon { get; private set; }

        public InteractType CommandType { get; private set; }

        public void Activate()
        {
            Icon.DOFade(1, 0);
        }

        public void TurnOff()
        {
            Icon.DOFade(0, .2f);
        }

        public void SetDefault()
        {
            Icon.DOFade(.3f, 0);
        }
    }
}
