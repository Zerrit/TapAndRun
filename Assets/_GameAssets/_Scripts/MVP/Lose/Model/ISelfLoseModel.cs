using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Lose.Model
{
    public interface ISelfLoseModel
    {
        ReactiveProperty<bool> IsDisplaying { get; }

        public void Restart();
        public void BackToHome();
    }
}