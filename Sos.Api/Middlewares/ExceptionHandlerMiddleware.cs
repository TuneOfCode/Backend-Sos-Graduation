using Sos.Api.Configurations;
using Sos.Application.Core.Exceptions;
using Sos.Domain.Core.Commons.Bases;
using Sos.Domain.Core.Errors;
using Sos.Domain.Core.Exceptions;
using System.Net;
using System.Text.Json;

namespace Sos.Api.Middlewares
{
    /// <summary>
    /// Middleware for handling exceptions
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ExceptionHandlerMiddleware"/> class.
    /// </remarks>
    /// <param name="next">The next middleware</param>
    /// <param name="logger">The logger</param>
    public class ExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlerMiddleware> logger
    )
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger = logger;

        /// <summary>
        /// Invokes the exception handler middleware with the specified <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="httpContext">The HTTP httpContext.</param>
        /// <returns>The task that can be awaited by the next middleware.</returns>
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred: {Message}", ex.Message);

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        /// <summary>
        /// Handles the specified <see cref="Exception"/> for the specified <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="httpContext">The HTTP httpContext.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>The HTTP response that is modified based on the exception.</returns>
        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            (HttpStatusCode httpStatusCode, IReadOnlyCollection<Error> errors) = GetHttpStatusCodeAndErrors(exception);

            httpContext.Response.ContentType = "application/json";

            httpContext.Response.StatusCode = (int)httpStatusCode;

            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            string response = JsonSerializer.Serialize(new ApiErrorResponse(errors), serializerOptions);

            await httpContext.Response.WriteAsync(response);
        }

        private static (HttpStatusCode httpStatusCode, IReadOnlyCollection<Error>) GetHttpStatusCodeAndErrors(Exception exception) =>
            exception switch
            {
                ValidationException validationException
                => (HttpStatusCode.BadRequest, validationException.Errors),
                DomainException domainException
                => (HttpStatusCode.BadRequest, new[] { domainException.Error }),
                _ => (HttpStatusCode.InternalServerError, new[] { GeneralError.ServerError })
            };
    }
}
