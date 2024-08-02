using System;

namespace TapAndRun.MVP.Levels.Model
{
    public interface ILevelModel
    {
        void Initialize();

        void LoadLevel();

        void StartLevel();
    }
}