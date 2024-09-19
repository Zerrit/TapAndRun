using System;
using TapAndRun.MVP.Levels.Views;
using TapAndRun.MVP.Levels.Views.SegmentViews;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TapAndRun._GameAssets._Scripts.Editor.LevelConstructor.BuildCommands
{
    public class RoadSegmentBuilder : AbstractSegmentBuilder
    {
        private RoadSegmentView _instance;

        private readonly RoadSegmentView _segmentPrefab;

        public RoadSegmentBuilder(LevelView level, RoadSegmentView roadViewPrefab) : base(level)
        {
            _segmentPrefab = roadViewPrefab;
        }

        public override void Build()
        {
            var segmentsCount = _level.Segments.Count;

            if (segmentsCount > 0)
            {
                var snapPoint = _level.Segments[segmentsCount - 1].SnapPoint;

                _instance = Object.Instantiate(_segmentPrefab, snapPoint.position, snapPoint.rotation, _level.transform);
                _instance.transform.Rotate(Vector3.forward, _level.Segments[segmentsCount - 1].SnapAngleOffset);

                _level.Crystals.AddRange(_instance.Crystals);

                _level.Segments.Add(_instance);
            }
            else
            {
                throw new Exception(
                    $"В списке сегментов не найден прерыдущий. Количество созданных сегментов в списке: {segmentsCount}.");
            }
        }

        public override void Debuild()
        {
            var crystalsCount = _level.Crystals.Count;
            var amountToRemove = _instance.Crystals.Length;

            _level.Crystals.RemoveRange(crystalsCount - amountToRemove, amountToRemove);

            base.Debuild();
        }
    }
}