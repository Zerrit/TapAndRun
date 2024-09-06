using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TapAndRun.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TapAndRun.MVP.Skins_Shop.Views
{
    public class SkinShopView : ScreenView
    {
        [field:SerializeField] public Text SkinName { get; private set; }
        [field:SerializeField] public Button BackButton { get; private set; }
        [field:SerializeField] public Button LeftButton { get; private set; }
        [field:SerializeField] public Button RightButton { get; private set; }
        [field:SerializeField] public ShopButton ShopButton { get; private set; }






    }
}