using TapAndRun.Configs;
using TapAndRun.MVP.Levels.View;
using UnityEngine;

namespace TapAndRun.Tools.LevelConstructor.BuildCommands
{
    public class EndJumpSegmentBuilder: AbstractSegmentBuilder
    {
        public EndJumpSegmentBuilder(LevelView level, LevelConstructorToolConfig config) : base(level)
        {
            _segmentPrefab = config.JumpEndSegment;
        }
    }
}