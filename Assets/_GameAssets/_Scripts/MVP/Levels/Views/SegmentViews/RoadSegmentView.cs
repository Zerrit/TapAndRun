﻿using UnityEngine;

namespace TapAndRun.MVP.Levels.Views.SegmentViews
{
    public class RoadSegmentView : AbstractSegmentView
    {
        [field: SerializeField] public CrystalView[] Crystals { get; private set; }

        public override void ResetSegment()
        {
            foreach (var crystal in Crystals)
            {
                crystal.Reset();
            }
        }
    }
}