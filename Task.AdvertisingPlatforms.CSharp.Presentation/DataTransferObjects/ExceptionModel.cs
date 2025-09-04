namespace Task.AdvertisingPlatforms.CSharp.Presentation.DataTransferObjects;

/// <summary>
/// ДТОшка для вывода исключений
/// </summary>
/// <param name="Code">Код ошибки</param>
/// <param name="Message">Сообщение</param>
public record ExceptionModel(short Code, string Message);