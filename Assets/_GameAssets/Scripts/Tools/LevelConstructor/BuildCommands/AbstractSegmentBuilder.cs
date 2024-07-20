using TapAndRun.MVP.Levels.View;
using UnityEngine;

namespace TapAndRun.Tools.LevelConstructor.BuildCommands
{
    public abstract class AbstractSegmentBuilder
    {
        protected AbstractSegmentView _instance;

        protected LevelView _level;
        
        public abstract void Build();

        public virtual void Debuild()
        {
            _level.Segments.Remove(_instance);
            _level.FinishSegment = _level.Segments[^1];
            
            if (_instance)
            {
                Object.DestroyImmediate(_instance.gameObject);
            }
        }
    }
}