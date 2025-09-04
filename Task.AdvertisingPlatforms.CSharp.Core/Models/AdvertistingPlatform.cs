using System.Text;

namespace Task.AdvertisingPlatforms.CSharp.Core.Models;


/// <summary>
/// Модель для парсинга рекламной платформы
/// </summary>
public class AdvertisingPlatform
{
    /// <summary>
    /// Название рекламной платформы
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// Локации в которых работает реклама
    /// </summary>
    public IList<string> Locations { get; private set; }

    /// <summary>
    /// Новая рекламная платформа
    /// </summary>
    /// <param name="name">Название платформы</param>
    /// <param name="locations">Локации с рекламами</param>
    public AdvertisingPlatform(string name, IEnumerable<string> locations)
    {
        Name = name;
        Locations = new List<string>(locations);
    }

    public override string ToString()
    {
        StringBuilder SB = new StringBuilder();
        SB.Append("Platform: ");
        SB.AppendLine(Name);
        SB.Append("Locations: ");
        for (int i = 0; i < Locations.Count-1; i++)
        {
            SB.Append(Locations[i]);
        }
        SB.AppendLine(Locations[Locations.Count-1]);
        return SB.ToString();
    }
}