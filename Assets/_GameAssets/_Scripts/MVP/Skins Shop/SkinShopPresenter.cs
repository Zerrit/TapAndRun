using TapAndRun.MVP.Skins_Shop.Model;
using TapAndRun.MVP.Skins_Shop.Views;

namespace TapAndRun.MVP.Skins_Shop
{
    public class SkinShopPresenter
    {
        private readonly ISelfSkinShopModel _model;
        private readonly SkinShopView _view;

        public SkinShopPresenter(ISelfSkinShopModel model, SkinShopView view)
        {
            _model = model;
            _view = view;
        }
    }
}