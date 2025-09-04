namespace Task.AdvertisingPlatforms.CSharp.Core.Util;

/// <summary>
/// Исключение при ошибки парсинга файла рекламных платформ
/// </summary>
public class AdvertParseException : Exception
{
    public AdvertParseException(string message) : base(message) { }
    public AdvertParseException(string message, Exception innerException) : base(message, innerException) { }
}