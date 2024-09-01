using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Skins_Shop.Model
{
    public interface ISelfSkinShopModel
    {
        ReactiveProperty<bool> IsDisplaying { get; set; }
        
    }

    public interface ISkinShopModel : IInitializableAsync
    {
        ReactiveProperty<bool> IsDisplaying { set; }
    }

    public class SkinShopModel : ISelfSkinShopModel, ISkinShopModel
    {
        public ReactiveProperty<bool> IsDisplaying { get; set; }

        public SkinShopModel()
        {
            
        }

        public UniTask InitializeAsync(CancellationToken token)
        {
            
            return UniTask.CompletedTask;
        }
    }
}