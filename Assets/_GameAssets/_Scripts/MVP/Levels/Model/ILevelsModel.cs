using System;

namespace TapAndRun.MVP.Levels.Model
{
    public interface ILevelsModel
    {
        void Initialize();

        void LoadLevel();

        void StartLevel();
    }
}