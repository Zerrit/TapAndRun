using System;
using System.Collections.Generic;

namespace TapAndRun.PlayerData
{
    [Serializable]
    public struct SaveFile
    {
        public DateTime SaveTime { get; private set; }
        public List<SaveableData> Data { get; private set; }

        public SaveFile(List<SaveableData> data)
        {
            SaveTime = DateTime.Now;
            Data = data;
        }
    }
}