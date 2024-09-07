using System;

namespace TapAndRun.PlayerProgress
{
    [Serializable]
    public class ProgressData
    {
        public string Key { get; private set; }
        public object[] Data { get; private set; }
        
        public ProgressData(string key, object[] data)
        {
            Key = key;
            Data = data;
        }
    }
}