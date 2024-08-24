﻿using System;
using TapAndRun.MVP.Gameplay.Views;
using TapAndRun.MVP.Gameplay.Views.SegmentViews;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TapAndRun.Tools.LevelConstructor.BuildCommands
{
    public class InteractSegmentBuilder : AbstractSegmentBuilder
    {
        private InteractSegmentView _instance;

        private readonly InteractSegmentView _segmentPrefab;

        public InteractSegmentBuilder(LevelView level, InteractSegmentView interactViewPrefab) : base(level)
        {
            _segmentPrefab = interactViewPrefab;
        }
        
        public override void Build()
        {
            var segmentsCount = _level.Segments.Count;

            if (segmentsCount > 0)
            {
                var snapPoint = _level.Segments[segmentsCount-1].SnapPoint;

                _instance = Object.Instantiate(_segmentPrefab, snapPoint.position, snapPoint.rotation, _level.transform);
                _instance.transform.Rotate(Vector3.forward, _level.Segments[segmentsCount-1].SnapAngleOffset);
                
                _level.InteractionPoints.Add(_instance.Interaction);
                
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
            var segmentCount = _level.InteractionPoints.Count;
            _level.InteractionPoints.RemoveAt(segmentCount - 1);

            base.Debuild();
        }
    }
}