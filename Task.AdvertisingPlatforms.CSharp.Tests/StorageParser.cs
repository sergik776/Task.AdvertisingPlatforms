using Microsoft.Extensions.Logging;
using Moq;
using Task.AdvertisingPlatforms.CSharp.Core.Interfaces;
using Task.AdvertisingPlatforms.CSharp.Core.Services;

namespace Task.AdvertisingPlatforms.CSharp.Tests;

public class StorageTest
{
    private IAdvertPlatformStorage _storage;
    private IAdventPlatformParser _parser;
    
    [SetUp]
    public void Setup()
    {
        _storage = new AdvertPlatformStorage();
        _parser = new AdventPlatformParser();
    }
    
    // Тест добавления рекламодателя в time-im memory хранилище
    // Если первый раз сработает нормально, то последующее тесты не имеют значения
    // так как список локаций пересоздается внутри хранилища
    [Test]
    public void Add()
    {
        var content_ok = File.ReadAllText("../../../../TestFiles/File1_ok");
        var result = _parser.Parse(content_ok);
        Assert.DoesNotThrow(() => _storage.Add(result));
    }
    
    // Тест получения списка рекламодателей по локации
    // При загрузке рабочего файла, должен вернуть все 4 рекламодателя, так как 
    // каждый входит в зону /ru
    [Test]
    public void Get()
    {
        var content_ok = File.ReadAllText("../../../../TestFiles/File1_ok");
        var data = _parser.Parse(content_ok);
        _storage.Add(data);
        var result = _storage.GetPlatforms("/ru");
        Assert.That(result.Count, Is.EqualTo(4));
    }
}