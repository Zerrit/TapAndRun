using TapAndRun.MVP.Levels.Views;
using UnityEngine;

namespace TapAndRun.Editor.LevelConstructor.BuildCommands
{
    public abstract class AbstractSegmentBuilder
    {
        protected readonly LevelView _level;

        protected AbstractSegmentBuilder(LevelView level)
        {
            _level = level;
        }

        public virtual void Build() { }

        public virtual void Debuild()
        {
            var segmentsCount = _level.Segments.Count;
            var lastSegment = _level.Segments[segmentsCount - 1];
            
            _level.Segments.RemoveAt(segmentsCount - 1);
            
            Object.DestroyImmediate(lastSegment.gameObject);
        }
    }
}