using System;
using System.Net;
using System.Threading.Tasks;
using InstantMessenger.Identity.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace InstantMessenger.Identity.Infrastructure
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (DomainException ex)
            {
                _logger.LogError(ex, $"Domain exception[{ex}] occurred");
                await HandleDomainExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected exception[{ex}] occurred");
                await HandleUnexpectedExceptionAsync(httpContext);
            }
        }
        private Task HandleDomainExceptionAsync(HttpContext context, DomainException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            var content = JsonConvert.SerializeObject(new
            {
                code = exception.Code,
                message = exception.Message
            });
            return context.Response.WriteAsync(content);
        }
        private Task HandleUnexpectedExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var content = JsonConvert.SerializeObject(new
            {
                code = "internal_error",
                message = "Something has gone wrong."
            });
            return context.Response.WriteAsync(content);
        }
    }

}