using System;

namespace TapAndRun.MVP.Levels.Model
{
    public interface ILevelModel
    {
        event Action OnPlayerClicked;
        
        void Initialize();

        void LoadLevel();

        void StartLevel();
    }
}