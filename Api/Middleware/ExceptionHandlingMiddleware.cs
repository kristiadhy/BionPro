using Application.Exceptions.Abstractions;
using Application.Validators;
using Domain.DTO;
using System.Text.Json;

namespace Api.Middleware;

internal sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e, _logger);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception, ILogger<ExceptionHandlingMiddleware> logger)
    {
        httpContext.Response.ContentType = "application/json";

        var response = new ApiResponseDto<List<string>>
        {
            IsSuccess = false
        };

        switch (exception)
        {
            case ValidationException validationException:
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                response.ErrorMessage = "Invalid Validation";
                response.Data = validationException.Errors;
                logger.LogWarning(exception, response.ErrorMessage, response.Data);
                break;

            case NotFoundException _:
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                response.ErrorMessage = exception.Message;
                logger.LogError(exception, exception.Message);
                break;

            case BadRequestException _:
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                response.ErrorMessage = exception.Message;
                logger.LogError(exception, exception.Message);
                break;

            default:
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response.ErrorMessage = "An error occurred while processing your request.";
                logger.LogError(exception, exception.Message);
                break;
        }

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

}
