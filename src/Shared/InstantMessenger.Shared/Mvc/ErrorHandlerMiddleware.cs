using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using InstantMessenger.Shared.Modules;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace InstantMessenger.Shared.Mvc
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IEnumerable<IExceptionMapper> _mappers;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger, IEnumerable<IExceptionMapper> mappers)
        {
            _next = next;
            _logger = logger;
            _mappers = mappers;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var error = _mappers
                    .Select(x => x.Map(ex))
                    .FirstOrDefault(x => x is {});
                if (error is null)
                {
                    _logger.LogError(ex, $"Unexpected exception[{ex}] occurred");
                    await HandleUnexpectedExceptionAsync(httpContext);
                }
                else
                {
                    _logger.LogError(ex, $"Domain exception[{ex}] occurred");
                    await HandleDomainExceptionAsync(httpContext, error);
                }
            }
        }
        private Task HandleDomainExceptionAsync(HttpContext context, Error error)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            var content = JsonConvert.SerializeObject(new
            {
                code = error.Code,
                message = error.Message
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