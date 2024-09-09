using Application.Exceptions;
using Application.Validators;
using Domain.DTO;
using Domain.Enum;
using System.Text.Json;

namespace Api.Middleware
{
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
                _logger.LogError(e, e.Message);
                await HandleExceptionAsync(context, e);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";

            var response = new ResponseDto
            {
                IsSuccess = false
            };

            if (exception is ValidationException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                response.Type = ErrorMessageEnum.InvalidValidation;
                response.Message = "Invalid Validation";
                response.Errors = (exception as ValidationException)?.Errors;
            }
            else if (exception is NotFoundException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                response.Type = ErrorMessageEnum.NotFound;
                response.Message = exception.Message;
            }
            else if (exception is BadRequestException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                response.Type = ErrorMessageEnum.BadRequest;
                response.Message = exception.Message;
            }
            else
            {
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response.Type = ErrorMessageEnum.InternalServerError;
                response.Message = "An error occurred while processing your request.";
            }

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
