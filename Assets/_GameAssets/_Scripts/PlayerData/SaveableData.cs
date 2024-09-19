using System;

namespace TapAndRun.PlayerData
{
    [Serializable]
    public class SaveableData
    {
        public string Key { get; private set; }
        public object[] Data { get; private set; }

        public SaveableData(string key, object[] data)
        {
            Key = key;
            Data = data;
        }
    }
}