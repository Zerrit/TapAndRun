using System;
using System.Collections.Generic;

namespace TapAndRun.PlayerProgress
{
    [Serializable]
    public struct SaveFile
    {
        public DateTime SaveTime; 
        public List<ProgressData> Data; 

        public SaveFile(List<ProgressData> data)
        {
            SaveTime = DateTime.Now;
            Data = data;
        }
    }
}