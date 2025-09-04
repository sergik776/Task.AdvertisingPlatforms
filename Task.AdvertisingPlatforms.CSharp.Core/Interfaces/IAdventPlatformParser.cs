using Task.AdvertisingPlatforms.CSharp.Core.Models;

namespace Task.AdvertisingPlatforms.CSharp.Core.Interfaces;

/// <summary>
/// интерфейс для парсинга файла с рекламодателями
/// </summary>
public interface IAdventPlatformParser
{
    /// <summary>
    /// Метод парсинга
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public IList<AdvertisingPlatform> Parse(string content);
}