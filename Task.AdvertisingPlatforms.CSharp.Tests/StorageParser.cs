using System.Diagnostics;
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
        
        var content_ok = File.ReadAllText("../../../../TestFiles/File1_ok");
        var data = _parser.Parse(content_ok);
        _storage.Add(data);
    }
    
    // Тест добавления рекламодателя в time-im memory хранилище
    // Если первый раз сработает нормально, то последующее тесты не имеют значения
    // так как список локаций пересоздается внутри хранилища
    [Test]
    public void AddAdvertisingPlatforms()
    {
        var content_ok = File.ReadAllText("../../../../TestFiles/File1_ok");
        var result = _parser.Parse(content_ok);
        Assert.DoesNotThrow(() => _storage.Add(result));
    }
    
    // Тест получения списка рекламодателей по локации
    [Test]
    public void GetAdvertisingPlatformsByLocation()
    {
        var result1 = _storage.GetPlatforms("/ru/msk");
        Assert.That(result1.Count, Is.EqualTo(2));
        var result2 = _storage.GetPlatforms("/ru/svrd");
        Assert.That(result2.Count, Is.EqualTo(2));
        var result3 = _storage.GetPlatforms("/ru/svrd/revda");
        Assert.That(result3.Count, Is.EqualTo(3));
        var result4 = _storage.GetPlatforms("/ru");
        Assert.That(result4.Count, Is.EqualTo(1));
    }
}