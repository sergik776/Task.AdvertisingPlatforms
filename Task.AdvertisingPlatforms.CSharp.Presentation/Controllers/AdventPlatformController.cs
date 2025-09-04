using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Task.AdvertisingPlatforms.CSharp.Core.Interfaces;

namespace Task.AdvertisingPlatforms.CSharp.Presentation.Controllers;

/// <summary>
/// Контроллер
/// </summary>
[ApiController]
[Route("[controller]")]
public class AdventPlatformController : ControllerBase
{
    /// <summary>
    /// логгер
    /// </summary>
    private readonly ILogger<AdventPlatformController> _logger;
    /// <summary>
    /// Хранилище рекламодателей
    /// </summary>
    private readonly IAdvertPlatformStorage  _advertPlatformStorage;
    /// <summary>
    /// Парсер
    /// </summary>
    private readonly IAdventPlatformParser _parser;

    public AdventPlatformController(ILogger<AdventPlatformController> logger, IAdvertPlatformStorage  advertPlatformStorage, IAdventPlatformParser parser)
    {
        _logger = logger;
        _advertPlatformStorage = advertPlatformStorage;
        _parser = parser;
    }
    
    /// <summary>
    /// Метод загрузки файла с рекламодателями и локациями
    /// </summary>
    /// <param name="file">Файл</param>
    /// <returns>200 - если нет ошибок</returns>
    [HttpPost("upload")]
    public async Task<IActionResult> UploadTxtFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Файл не выбран");
        string content;
        using (var reader = new StreamReader(file.OpenReadStream()))
        {
            content = await reader.ReadToEndAsync();
        }

        var res = _parser.Parse(content);
        _advertPlatformStorage.Add(res);
        return Ok(res);
    }
    
    /// <summary>
    /// Метод вывода рекламодателей по локации
    /// </summary>
    /// <param name="location">Локация</param>
    /// <returns>Список рекламодателей</returns>
    [HttpGet(Name = "SearchPlatformByLocation")]
    public IEnumerable<string>? SearchPlatformByLocation([RegularExpression(@"^\/.+$", ErrorMessage = "Параметр должен начинаться с '/' и содержать минимум один символ после")]
        string location)
    {
        return _advertPlatformStorage.GetPlatforms(location);
    }
}