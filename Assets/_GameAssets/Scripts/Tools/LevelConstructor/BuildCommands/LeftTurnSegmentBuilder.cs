using TapAndRun.Configs;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Levels.View;
using UnityEngine;

namespace TapAndRun.Tools.LevelConstructor.BuildCommands
{
    public class TurnLeftSegmentBuilder : AbstractSegmentBuilder
    {
        private readonly AbstractSegmentView _segmentPrefab;

        public TurnLeftSegmentBuilder(LevelConstructorToolConfig config, LevelView levelView)
        {
            _segmentPrefab = config.LeftTurnSegment;
            _level = levelView;
        }

        public override void Build()
        {
            var snapPoint = _level.FinishSegment.SnapPoint;

            _instance = Object.Instantiate(_segmentPrefab, snapPoint.position, snapPoint.rotation, _level.transform);
            _instance.transform.Rotate(Vector3.forward, _level.FinishSegment.SnapAngleOffset);

            _level.Segments.Add(_instance);
            _level.FinishSegment = _instance;
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