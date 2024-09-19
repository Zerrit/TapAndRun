using UnityEngine;

namespace TapAndRun.MVP.Levels.Views.SegmentViews
{
    public class InteractSegmentView : AbstractSegmentView
    {
        [field: SerializeField] public InteractionPoint Interaction { get; private set; }

        public override void ResetSegment()
        {
            Interaction.SetDefault();
        }
    }
}