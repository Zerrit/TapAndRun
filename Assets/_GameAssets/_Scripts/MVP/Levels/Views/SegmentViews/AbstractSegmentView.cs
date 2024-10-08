﻿using UnityEngine;

namespace TapAndRun.MVP.Levels.Views.SegmentViews
{
    public abstract class AbstractSegmentView : MonoBehaviour
    {
        [field:SerializeField] public int SnapAngleOffset { get; private set; }   
        [field:SerializeField] public Transform SnapPoint { get; private set; }

        public abstract void ResetSegment();
    }
}