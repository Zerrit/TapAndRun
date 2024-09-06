using System;
using System.Collections.Generic;

namespace TapAndRun.PlayerProgress
{
    [Serializable]
    public struct SaveFile
    {
        public DateTime SaveTime { get; }
        public List<SaveLoadData> Data { get; }

        public SaveFile(List<SaveLoadData> data)
        {
            SaveTime = DateTime.Now;
            Data = data;
        }
    }
}