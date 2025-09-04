using Microsoft.Extensions.Logging;
using Moq;
using Task.AdvertisingPlatforms.CSharp.Core.Interfaces;
using Task.AdvertisingPlatforms.CSharp.Core.Services;
using Task.AdvertisingPlatforms.CSharp.Core.Util;

namespace Task.AdvertisingPlatforms.CSharp.Tests;

public class ParserTests
{
    private IAdventPlatformParser _parser;

    [SetUp]
    public void Setup()
    {
        _parser = new AdventPlatformParser();
    }

    // Тест считается успешным, если было загружено 4 рекламодателя, так как их в файле 4
    [Test]
    public void Parse_File1_ok()
    {
        var content_ok = File.ReadAllText("../../../../TestFiles/File1_ok");
        var result = _parser.Parse(content_ok);
        Assert.That(result.Count, Is.EqualTo(4));
    }
    
    // Тест считается успешным, если вернулась ошибка неправильного формата, так как в файле отсутствует ':'
    [Test]
    public void Parse_File2_error()
    {
        var content_error = File.ReadAllText("../../../../TestFiles/File2_error");
        var ex = Assert.Throws<AdvertParseException>(() => _parser.Parse(content_error));
        Assert.That(ex.Message, Does.Contain("неправильный формат"));
    }

}