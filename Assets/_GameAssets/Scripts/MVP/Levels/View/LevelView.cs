using TMPro;
using UnityEngine;

namespace TapAndRun.MVP.Levels.View
{
    public class LevelView : MonoBehaviour
    {
        [field: SerializeField] public TMP_Text LevelNumberText { get; private set; }
        [field: SerializeField] public Transform FinishSegment { get; private set; }

        [field: SerializeField] public AbstractSegmentView[] Segments { get; private set; }


        public void Initialize(int level)
        {
            LevelNumberText.text = level.ToString();
        }

        public void Reset()
        {
            /*foreach (var arrow in Arrows)
            {
                arrow.SetDefault();
            }

            foreach (var crystal in Crystals)
            {
                crystal.Reset();
            }*/
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
        
        //public int levelId;

        //public int difficulty;

        //public TMP_Text levelNumberText;

        //public Transform finishSegment;
        /*[SerializeField]public Quaternion levelRotation;


        public List<SegmentView> segmentList = new List<SegmentView>();     
        public List<Arrow> arrow = new List<Arrow>();
        public CrystalView[] crystal;

        private void Awake()
        {
            levelNumberText.text = (levelId + 1).ToString();
            levelRotation = transform.rotation;
        }

        public void GetCommands(List<ArrowType> commands)
        {
            foreach (Arrow arr in arrow)
            {
                commands.Add(arr._commandType);
            }
        }*/
    }
}