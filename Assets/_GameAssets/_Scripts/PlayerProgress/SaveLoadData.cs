using System;

namespace TapAndRun.PlayerProgress
{
    [Serializable]
    public class SaveLoadData
    {
        public string Key { get; private set; }
        public object[] Data { get; private set; }
        
        public SaveLoadData(string key, object[] data)
        {
            Key = key;
            Data = data;
        }
    }
}