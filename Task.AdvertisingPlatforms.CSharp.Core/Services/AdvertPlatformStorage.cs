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
            var parts = key.Split('/', StringSplitOptions.RemoveEmptyEntries);
            string prefix = "";
            foreach (var part in parts)
            {
                prefix += "/" + part;
                _Locations_Platforms.TryAdd(prefix, new());
            }
        }
        
        foreach (var locationsPlatform in _Locations_Platforms.Keys)
        {
            var parts = locationsPlatform.Split('/', StringSplitOptions.RemoveEmptyEntries);
            string prefix = "";
            foreach (var part in parts)
            {
                prefix += "/" + part;
                var platforms = listPlatforms.Where(x=>x.Locations.Contains(prefix));
                foreach (var platform in platforms)
                {
                    _Locations_Platforms[locationsPlatform].Add(platform.Name);
                }
            }
        }
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