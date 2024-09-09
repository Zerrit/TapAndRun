﻿using System.Threading;
using Cysharp.Threading.Tasks;

namespace TapAndRun.PlayerData.SaveLoad
{
    public interface IDataStorage
    {
        void Write(string data);
        UniTask WriteAsync(string data, CancellationToken token);
        UniTask<string> Readsync(CancellationToken token);
        bool IsFileExist();
    }
}