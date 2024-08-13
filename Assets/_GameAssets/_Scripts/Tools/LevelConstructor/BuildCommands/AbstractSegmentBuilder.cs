using TapAndRun.MVP.Levels.View;
using TapAndRun.MVP.Levels.View.SegmentViews;
using UnityEngine;

namespace TapAndRun.Tools.LevelConstructor.BuildCommands
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