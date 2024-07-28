using System;
using System.Collections.Generic;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Levels.View.SegmentViews;
using UnityEngine;

namespace TapAndRun.MVP.Levels.View
{
    public class LevelView : MonoBehaviour
    {
        public event Action OnLevelCompleted;
        
        [field: SerializeField] public int Difficulty { get; set; }

        [field: SerializeField] public StartSegmentView StartSegment { get; set; }
        [field: SerializeField] public FinishSegmentView FinishSegment { get; set; }

        [field: SerializeField] public List<AbstractSegmentView> Segments { get; } = new();
        [field: SerializeField] public List<InteractType> Interactions { get; } = new();

        public void Configure(int level)
        {
            StartSegment.LevelNumberText.text = level.ToString();
            FinishSegment.OnPlayerEntered += Complete;
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

        private void Complete()
        {
            OnLevelCompleted?.Invoke();
        }
    }
}