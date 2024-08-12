using TapAndRun.Configs;
using TapAndRun.MVP.Levels.View;
using TapAndRun.MVP.Levels.View.SegmentViews;

namespace TapAndRun.Tools.LevelConstructor.BuildCommands
{
    public class RoadSegmentBuilder : AbstractSegmentBuilder
    {
        public RoadSegmentBuilder(LevelView level, LevelConstructorToolConfig config) : base(level)
        {
            _segmentPrefab = config.RoadSegment;
        }
        
        public override void Build()
        {
            base.Build();

            var roadSegment = _instance as RoadSegmentView;

            if (roadSegment)
            {
                _level.Crystals.AddRange(roadSegment.Crystals);
            }
        }
    }
}