using TapAndRun.Configs;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Levels.View;
using UnityEngine;

namespace TapAndRun.Tools.LevelConstructor.BuildCommands
{
    public class TurnLeftSegmentBuilder : AbstractSegmentBuilder
    {
        public TurnLeftSegmentBuilder(LevelView level, LevelConstructorToolConfig config) : base(level)
        {
            _segmentPrefab = config.LeftTurnSegment;
        }

        public override void Build()
        {
            base.Build();

            _level.Interactions.Add(InteractType.TurnLeft);
        }
        
        public override void Debuild()
        {
            base.Debuild();

            var lastIndex = _level.Interactions.Count;
            _level.Interactions.RemoveAt(lastIndex - 1);
        }
    }
}