using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Lose.Model
{
    public interface ISelfLoseModel
    {
        SimpleReactiveProperty<bool> IsDisplaying { get; }

        public void Restart();
        public void BackToHome();
    }
}