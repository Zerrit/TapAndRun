namespace TapAndRun.PrallaxBackground
{
    public interface IParallaxView
    {
        /// <summary>
        /// Устанавливает базовый пресет для фона игры.
        /// </summary>
        void SetDefault();

        /// <summary>
        /// Переключает пресет фона игры на следующий по списку.
        /// </summary>
        void ChangeStyle();
    }
}