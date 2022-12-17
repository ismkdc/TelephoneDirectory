using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TelephoneDirectory.Infrastructure.Errors;

namespace TelephoneDirectory.Infrastructure.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ILogger<ExceptionMiddleware> logger)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            CustomError customError;
            var response = context.Response;
            switch (error)
            {
                case TelephoneDirectoryException e:
                    customError = e.CustomError;
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                default:
                    customError = CustomErrors.E_100;
                    logger.LogError(error.ToString());
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            await response.WriteAsJsonAsync(
                customError
            );
        }
    }
}