using System.Collections.Generic;
using TapAndRun.Factories;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Levels.View;

namespace TapAndRun.MVP.Levels.Presenter
{
    public class LevelsPresenter
    {
        private readonly LevelsModel _model;
        private readonly LevelFactory _levelFactory;
        private readonly Queue<LevelView> _levels; 
        
        public LevelsPresenter(LevelsModel model, LevelFactory levelFactory)
        {
            _model = model;
        }

        public void Initialize()
        {
            
        }
    }
}