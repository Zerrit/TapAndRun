namespace TapAndRun.PlayerProgress
{
    public interface ISaveLoadable //TODO возможно сменить название на IProgressable
    {
        /// <summary>
        /// Индивидуальный ключ сохранения сущности.
        /// </summary>
        string SaveKey { get; }

        /// <summary>
        /// Возвращает объект сохранений заполненный массивом данных общего типа и индивидуальным ключом.
        /// </summary>
        ProgressData GetProgressData();

        /// <summary>
        /// Инициализирует себя извлечёнными данными.
        /// </summary>
        void RestoreProgress(ProgressData loadData);
    }
}