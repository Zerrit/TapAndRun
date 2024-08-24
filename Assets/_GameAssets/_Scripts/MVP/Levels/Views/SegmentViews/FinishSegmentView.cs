using System;
using UnityEngine;

namespace TapAndRun.MVP.Levels.Views.SegmentViews
{
    public class FinishSegmentView : AbstractSegmentView
    {
        public event Action OnPlayerEntered;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                OnPlayerEntered?.Invoke();
            }
        }

        public override void ResetSegment()
        {
            
        }
    }
}