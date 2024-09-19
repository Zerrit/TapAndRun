using UnityEngine;

namespace TapAndRun.Configs
{
    [CreateAssetMenu(fileName = "AdsServiceConfig", menuName = "Ads Service Config")]
    public class AdsServiceConfig : ScriptableObject
    {
        [field:SerializeField] public string AndroidAppKey { get; private set; }
        [field:SerializeField] public string IOSAppKey { get; private set; }

        [field:SerializeField, Header("Счётчик до показа рекламы (сек.)")] 
        public int AdsIntervalCount { get; private set; }

        [field:SerializeField, Header("Время перезарядки до показа рекламы")] 
        public float AdsCooldownTime { get; private set; }
    }
}