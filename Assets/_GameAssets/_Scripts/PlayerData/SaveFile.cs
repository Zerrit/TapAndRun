using System;
using System.Collections.Generic;

namespace TapAndRun.PlayerData
{
    [Serializable]
    public struct SaveFile
    {
        public DateTime SaveTime; 
        public List<SaveableData> Data; 

        public SaveFile(List<SaveableData> data)
        {
            SaveTime = DateTime.Now;
            Data = data;
        }
    }
}