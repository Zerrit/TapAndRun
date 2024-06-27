using System.Collections.Generic;
using TapAndRun.Level;
using TMPro;
using UnityEngine;

namespace TapAndRun.Levels.View
{
    public class LevelView : MonoBehaviour
    {
        public int levelId;

        public int difficulty;

        public TMP_Text levelNumberText;

        public Transform finishSegment;
        [SerializeField]public Quaternion levelRotation;


        public List<SegmentView> segmentList = new List<SegmentView>();     
        public List<Arrow> arrow = new List<Arrow>();
        public CrystalView[] crystal;

        private void Awake()
        {
            levelNumberText.text = (levelId + 1).ToString();
            levelRotation = transform.rotation;
        }

        public void ResetArrows()
        {
            foreach (Arrow arr in arrow)
            {
                arr.SetDefault();
            }

            foreach (CrystalView crys in crystal)
            {
                crys.Reset();
            }
        }

        public void DeleteLevel()
        {
            Destroy(gameObject);
        }

        public void GetCommands(List<ArrowType> commands)
        {
            foreach (Arrow arr in arrow)
            {
                commands.Add(arr._commandType);
            }
        }
    }
}