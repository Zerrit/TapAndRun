using TapAndRun.Configs;
using TapAndRun.MVP.Levels.View;

namespace TapAndRun.Tools.LevelConstructor.BuildCommands
{
    public class RoadSegmentBuilder : AbstractSegmentBuilder
    {
        public RoadSegmentBuilder(LevelView level, LevelConstructorToolConfig config) : base(level)
        {
            _segmentPrefab = config.RoadSegment;
        }
    }
}