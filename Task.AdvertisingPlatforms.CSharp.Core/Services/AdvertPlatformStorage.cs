using System.Collections;
using Task.AdvertisingPlatforms.CSharp.Core.Interfaces;
using Task.AdvertisingPlatforms.CSharp.Core.Models;

namespace Task.AdvertisingPlatforms.CSharp.Core.Services;

/// <summary>
/// Сервис хранения рекламодателей
/// </summary>
public class AdvertPlatformStorage : IAdvertPlatformStorage
{
    /// <summary>
    /// Словарь с локациями и рекламодателями
    /// </summary>
    /// Ключом является локация для быстрого поиска
    private Dictionary<string, HashSet<string>> _Locations_Platforms;

    public AdvertPlatformStorage()
    {
        _Locations_Platforms = new Dictionary<string, HashSet<string>>();
    }
    
    
    public void Add(IList<AdvertisingPlatform> listPlatforms)
    {
        foreach (var key in listPlatforms.SelectMany(x=>x.Locations))
        {
            var prefixes = GetPrefixesTag(key);
            foreach (var prefix in prefixes)
            {
                _Locations_Platforms.TryAdd(prefix, new());
            }
        }
        
        foreach (var locationsPlatform in _Locations_Platforms.Keys)
        {
            foreach (var prefix in GetPrefixesTag(locationsPlatform))
            {
                var platforms = listPlatforms.Where(x=>x.Locations.Contains(prefix));
                foreach (var platform in platforms)
                {
                    _Locations_Platforms[locationsPlatform].Add(platform.Name);
                }
            }
        }
    }

    private string[] GetPrefixesTag(string tag)
    {
        var parts = tag.Split('/', StringSplitOptions.RemoveEmptyEntries);
        string[] result = new string[parts.Length];
        string prefix = "";
        for (int i = 0; i < parts.Length; i++)
        {
            prefix += "/" + parts[i];
            result[i] = prefix;
        }
        return result;
    }


    public IEnumerable<string> GetPlatforms(string location)
    {
        if (_Locations_Platforms.TryGetValue(location, out var platforms))
        {
            return platforms;
        }
        return Enumerable.Empty<string>();
    }

}