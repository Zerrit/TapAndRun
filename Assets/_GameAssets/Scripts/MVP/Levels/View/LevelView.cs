using System.Collections.Generic;
using TapAndRun.MVP.Levels.Model;
using TMPro;
using UnityEngine;

namespace TapAndRun.MVP.Levels.View
{
    public class LevelView : MonoBehaviour
    {
        [field: SerializeField] public int Difficulty { get; set; }
        [field: SerializeField] public AbstractSegmentView FinishSegment { get; set; }
        [field: SerializeField] public TMP_Text LevelNumberText { get; set; }
        [field: SerializeField] public List<AbstractSegmentView> Segments { get; private set; } = new();
        [field: SerializeField] public List<InteractType> Interactions { get; private set; } = new();


        public void Initialize(int level)
        {
            LevelNumberText.text = level.ToString();
        }

        public void ResetLevel()
        {
            foreach (var segment in Segments)
            {
                segment.ResetSegment();
            }
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}