using TapAndRun.Architecture.GameStates;
using VContainer;

namespace TapAndRun.Factories.GameStates
{
    public class GameStateFactory : IGameStateFactory
    {
        private readonly IObjectResolver _resolver;

        public GameStateFactory(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        public T CreateState<T>() where T : IGameState
        {
            return _resolver.Resolve<T>();
        }
    }
}