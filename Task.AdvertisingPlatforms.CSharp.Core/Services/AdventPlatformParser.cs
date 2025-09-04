using Task.AdvertisingPlatforms.CSharp.Core.Interfaces;
using Task.AdvertisingPlatforms.CSharp.Core.Models;
using Task.AdvertisingPlatforms.CSharp.Core.Util;

namespace Task.AdvertisingPlatforms.CSharp.Core.Services;

/// <summary>
/// Сервис парсинга рекламодателей
/// </summary>
public class AdventPlatformParser : IAdventPlatformParser
{
    public AdventPlatformParser()
    {
    }
    
    /// <summary>
    /// Главный метод парсинга
    /// </summary>
    /// <param name="content">Строка с данными</param>
    /// <returns>Коллекция рекламодателей</returns>
    /// <exception cref="AdvertParseException">Ошибка парсинга</exception>
    public IList<AdvertisingPlatform> Parse(string content)
    {
        var platforms = new List<AdvertisingPlatform>();
        var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in lines)
        {
            var parts = line.Split(':');
            if (parts.Length != 2) throw new AdvertParseException($"Данные имеют неправильный формат");
            var name = parts[0].Trim();
            if(string.IsNullOrEmpty(name)) throw new AdvertParseException($"Название рекламодателя не может быть пустым");
            var locations = parts[1]
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());
            if (locations.Count() == 0) throw new AdvertParseException($"Список локаций рекламодателя не может быть пустым");
            
            // Создаем HashSet для всех локаций и их префиксов
            var extendedLocations = new HashSet<string>();
            foreach (var loc in locations)
            {
                var segments = loc.Split('/', StringSplitOptions.RemoveEmptyEntries);
                var prefix = "";
                foreach (var segment in segments)
                {
                    prefix += "/" + segment;
                    extendedLocations.Add(prefix);
                }
            }
            platforms.Add(new AdvertisingPlatform(name, extendedLocations));
        }
        return platforms;
    }
}