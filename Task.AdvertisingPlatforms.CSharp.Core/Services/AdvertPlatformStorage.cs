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
    
    
    public void Add(IList<AdvertisingPlatform> advertisingPlatform)
    {
        //Пересоздаем словарь, так как из условий ТЗ идет перезапись, а не добавление рекламодателей
        _Locations_Platforms = new Dictionary<string, HashSet<string>>();
        for (int i = 0; i < advertisingPlatform.Count(); i++)
        {
            for (int j = 0; j < advertisingPlatform[i].Locations.Count; j++)
            {
                if (_Locations_Platforms.ContainsKey(advertisingPlatform[i].Locations[j]))
                {
                    if (!_Locations_Platforms[advertisingPlatform[i].Locations[j]].Contains(advertisingPlatform[i].Name))
                    {
                        _Locations_Platforms[advertisingPlatform[i].Locations[j]].Add(advertisingPlatform[i].Name);
                    }
                }
                else
                {
                    _Locations_Platforms.Add(advertisingPlatform[i].Locations[j], new  HashSet<string>() { advertisingPlatform[i].Name });
                }
            }
        };
    }

    public IEnumerable<string>? GetPlatforms(string location)
    {
        if (_Locations_Platforms.TryGetValue(location, out var platforms))
        {
            return platforms;
        }
        return Enumerable.Empty<string>();
    }
}