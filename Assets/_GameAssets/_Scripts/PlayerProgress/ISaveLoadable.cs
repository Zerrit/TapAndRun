namespace TapAndRun.PlayerProgress
{
    public interface ISaveLoadable
    {
        /// <summary>
        /// Индивидуальный ключ сохранения сущности.
        /// </summary>
        string SaveKey { get; }

        /// <summary>
        /// Возвращает объект сохранений заполненный массивом данных общего типа и индивидуальным ключом.
        /// </summary>
        SaveLoadData GetSaveLoadData();

        /// <summary>
        /// Инициализирует себя извлечёнными данными.
        /// </summary>
        void RestoreValue(SaveLoadData loadData);
    }
}