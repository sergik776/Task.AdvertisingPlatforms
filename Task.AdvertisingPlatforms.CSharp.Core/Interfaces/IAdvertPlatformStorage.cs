using Task.AdvertisingPlatforms.CSharp.Core.Models;

namespace Task.AdvertisingPlatforms.CSharp.Core.Interfaces;

/// <summary>
/// Интерфейс для работы с RAM хранилищем рекламных платформ - синглтон
/// </summary>
public interface IAdvertPlatformStorage
{
    /// <summary>
    /// Добавить площадки
    /// </summary>
    /// <param name="advertisingPlatform">Новые площадки</param>
    /// <returns>Task</returns>
    public void Add(IList<AdvertisingPlatform> advertisingPlatform);

    /// <summary>
    /// Поиск площадки по локации
    /// </summary>
    /// <param name="location">Название локации</param>
    /// <returns>Название площадки</returns>
    public IEnumerable<string>? GetPlatforms(string location);
}