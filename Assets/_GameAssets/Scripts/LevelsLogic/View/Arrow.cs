using UnityEngine;
using DG.Tweening;
using TapAndRun.Character.Model;
using TapAndRun.Level;

public class Arrow : MonoBehaviour
{
    public ArrowType _commandType;
    public SpriteRenderer sprite;

    public void Activate()
    {
        sprite.DOFade(1, 0);
    }

    public void TurnOff()
    {
        sprite.DOFade(0, .2f);
    }

    public void SetDefault()
    {
        sprite.DOFade(.3f, 0);
    }
}
