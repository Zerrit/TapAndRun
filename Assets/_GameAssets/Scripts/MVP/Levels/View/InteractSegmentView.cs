using UnityEngine;

namespace TapAndRun.MVP.Levels.View
{
    public class InteractSegmentView : AbstractSegmentView
    {
        [field: SerializeField] public InteractionPoint Interaction { get; private set; }
        public override void Reset()
        {
            Interaction.SetDefault();
        }
    }
}