using Task.AdvertisingPlatforms.CSharp.Presentation.DataTransferObjects;
using Task.AdvertisingPlatforms.CSharp.Core.Util;

namespace Task.AdvertisingPlatforms.CSharp.Presentation.Middlewares;

/// <summary>
/// Мидлвар исключений
/// </summary>
public class ErrorMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async System.Threading.Tasks.Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AdvertParseException ex) // Ошибка парсинга
        {
            //Тут можно выдавать результат в понятном для конечного пользователя формате
            await SendErrorResponse(new ExceptionModel(400, ex.Message), context);
        }
        catch (Exception ex) //Любая другая ошибка
        {
            //Ошибка не обработанная, выдается в изначальном виде
            await SendErrorResponse(new ExceptionModel(500, ex.Message), context);
        }
    }

    private async System.Threading.Tasks.Task SendErrorResponse(ExceptionModel ex, HttpContext context)
    {
        context.Response.StatusCode = ex.Code;
        context.Response.ContentType = "application/json";

        var response = new { code = ex.Code, message = ex.Message };
        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(jsonResponse);
    }
}