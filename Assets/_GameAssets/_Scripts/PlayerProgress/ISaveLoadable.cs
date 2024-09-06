namespace TapAndRun.PlayerProgress
{
    public interface ISaveLoadable
    {
        string SaveKey { get; }

        SaveLoadData GetSaveLoadData();
        void RestoreValue(SaveLoadData loadData);
    }
}