using TapAndRun.Configs;
using TapAndRun.MVP.Levels.View;
using UnityEngine;

namespace TapAndRun.Tools.LevelConstructor.BuildCommands
{
    public class EndJumpSegmentBuilder: AbstractSegmentBuilder
    {
        private readonly AbstractSegmentView _segmentPrefab;

        public EndJumpSegmentBuilder(LevelConstructorToolConfig config, LevelView levelView)
        {
            _segmentPrefab = config.JumpEndSegment;
            _level = levelView;
        }

        public override void Build()
        {
            var snapPoint = _level.FinishSegment.SnapPoint;

            _instance = Object.Instantiate(_segmentPrefab, snapPoint.position, snapPoint.rotation, _level.transform);
            _instance.transform.Rotate(Vector3.forward, _level.FinishSegment.SnapAngleOffset);

            _level.Segments.Add(_instance);
            _level.FinishSegment = _instance;
        }
    }
}