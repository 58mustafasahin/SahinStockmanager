using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SM.Core.Common.Constants;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace SM.Core.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, ILogger<ExceptionMiddleware> logger)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext, e, logger);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception e, ILogger<ExceptionMiddleware> logger)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            _ = e.Message;
            string message = "";
            var httpStatus = "";


            var exceptionType = e.GetBaseException().GetType();


            if (e.Message.Contains(Messages.AuthorizationDenied, StringComparison.CurrentCultureIgnoreCase))
            {
                message = Messages.AuthorizationDenied;
                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                logger.LogTrace(message);
            }
            else if (exceptionType == typeof(ValidationException))
            {
                message = e.Message;
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                logger.LogTrace(message);
            }
            else if (exceptionType == typeof(ApplicationException))
            {
                message = e.Message;
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                logger.LogWarning(message);
            }
            else if (exceptionType == typeof(MethodAccessException))
            {
                message = e.Message;
                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                logger.LogTrace(message);
            }
            else
            {
                string errorRecordNumber = Guid.NewGuid().ToString();
                message = "An unexpected error occurred. Contact your system administrator error log number : " + errorRecordNumber;  // $"{e.Message}<br><br>{e.InnerException}";
                logger.LogCritical(e, message);
            }
            await httpContext.Response.WriteAsJsonAsync(new { success = false, message = message, httpStatus = httpStatus });
        }
    }
}