using TapAndRun.Configs;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Levels.View;
using UnityEngine;

namespace TapAndRun.Tools.LevelConstructor.BuildCommands
{
    public class RightTurnSegmentBuilder : AbstractSegmentBuilder
    {
        public RightTurnSegmentBuilder(LevelView level, LevelConstructorToolConfig config) : base(level)
        {
            _segmentPrefab = config.RightTurnSegment;
        }

        public override void Build()
        {
            base.Build();

            _level.Interactions.Add(InteractType.TurnRight);
        }
        
        public override void Debuild()
        {
            base.Debuild();

            var lastIndex = _level.Interactions.Count;
            _level.Interactions.RemoveAt(lastIndex - 1);
        }
    }
}