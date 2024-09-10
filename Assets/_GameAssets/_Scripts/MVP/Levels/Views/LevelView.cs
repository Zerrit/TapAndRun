using System;
using System.Collections.Generic;
using TapAndRun.MVP.Levels.Views.SegmentViews;
using UnityEngine;

namespace TapAndRun.MVP.Levels.Views
{
    public class LevelView : MonoBehaviour
    {
        public event Action OnFinishReached;

        [field: SerializeField] public int SpeedDifficulty { get; set; }
        [field: SerializeField] public int CameraDifficulty { get; set; }

        [field: SerializeField] public StartSegmentView StartSegment { get; set; }
        [field: SerializeField] public FinishSegmentView FinishSegment { get; set; }

        [field: SerializeField] public List<AbstractSegmentView> Segments { get; private set; } = new();
        [field: SerializeField] public List<InteractionPoint> InteractionPoints { get; private set; } = new();
        [field: SerializeField] public List<CrystalView> Crystals { get; private set; } = new();

        public void Configure(int level)
        {
            StartSegment.LevelNumberText.text = (level + 1).ToString();

            FinishSegment.OnPlayerEntered += Complete;
        }

        public void ActivateArrow(int index = 0)
        {
            InteractionPoints[index]?.Activate();
        }

        public void SwitchToNextArrow(int currentArrowIndex)
        {
            InteractionPoints[currentArrowIndex]?.Deactivate();

            if (currentArrowIndex + 1 < InteractionPoints.Count)
            {
                InteractionPoints[currentArrowIndex + 1]?.Activate();
            }
        }

        public void Refresh()
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
            OnFinishReached?.Invoke();
        }
    }
}