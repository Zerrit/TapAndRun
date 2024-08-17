using TMPro;
using UnityEngine;

namespace TapAndRun.MVP.Gameplay.Views.SegmentViews
{
    public class StartSegmentView : AbstractSegmentView
    {
        [field: SerializeField] public Transform SegmentCenter { get; private set; }
        [field: SerializeField] public TMP_Text LevelNumberText { get; private set; }
        
        public override void ResetSegment()
        {
            //TODO
        }
    }
}