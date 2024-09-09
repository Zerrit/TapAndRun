namespace TapAndRun.PlayerData
{
    public interface IProgressable 
    {
        /// <summary>
        /// Индивидуальный ключ сохранения сущности.
        /// </summary>
        string SaveKey { get; }

        /// <summary>
        /// Возвращает объект сохранений заполненный массивом данных общего типа и индивидуальным ключом.
        /// </summary>
        SaveableData GetProgressData();

        /// <summary>
        /// Инициализирует себя извлечёнными данными.
        /// </summary>
        void RestoreProgress(SaveableData loadData);
    }
}