using TapAndRun.Architecture.GameStates;

namespace TapAndRun.Factories.GameStates
{
    public interface IGameStateFactory
    {
        T CreateState<T>() where T : IGameState;
    }
}