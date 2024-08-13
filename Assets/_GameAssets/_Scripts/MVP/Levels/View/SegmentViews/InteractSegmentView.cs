using UnityEngine;

namespace TapAndRun.MVP.Levels.View.SegmentViews
{
    public class InteractSegmentView : AbstractSegmentView
    {
        [field: SerializeField] public InteractionPoint Interaction { get; private set; }

        public void Activate()
        {
            Interaction.Activate();
        }

        public void Deactivate()
        {
            Interaction.Deactivate();
        }
        
        public override void ResetSegment()
        {
            Interaction.SetDefault();
        }
    }
}