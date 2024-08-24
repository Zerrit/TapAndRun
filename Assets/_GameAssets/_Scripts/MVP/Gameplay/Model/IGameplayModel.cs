namespace TapAndRun.MVP.Gameplay.Model
{
    public interface IGameplayModel
    {
        void Initialize();

        void LoadLevel();

        void StartLevel();
    }
}