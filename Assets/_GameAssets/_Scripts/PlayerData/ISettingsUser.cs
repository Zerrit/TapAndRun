namespace TapAndRun.PlayerData
{
    public interface ISettingsUser
    {
        /// <summary>
        /// Индивидуальный ключ сохранения сущности.
        /// </summary>
        string SaveKey { get; }

        /// <summary>
        /// Возвращает объект сохранений заполненный массивом данных общего типа и индивидуальным ключом.
        /// </summary>
        SaveableData GetSettingsData();

        /// <summary>
        /// Инициализирует себя извлечёнными данными.
        /// </summary>
        void RestoreSettings(SaveableData data);
    }
}