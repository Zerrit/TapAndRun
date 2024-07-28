using TapAndRun.Configs;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Levels.View;

namespace TapAndRun.Tools.LevelConstructor.BuildCommands
{
    public class StartJumpSegmentBuilder : AbstractSegmentBuilder
    {
        public StartJumpSegmentBuilder(LevelView level, LevelConstructorToolConfig config) : base(level)
        {
            _segmentPrefab = config.JumpStartSegment;
        }

        public override void Build()
        {
            base.Build();

            _level.Interactions.Add(InteractType.Jump);
        }
        
        public override void Debuild()
        {
            base.Debuild();

            var lastIndex = _level.Interactions.Count;
            _level.Interactions.RemoveAt(lastIndex - 1);
        }
    }
}