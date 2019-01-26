using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrostructure;

namespace TeamEdge.WebLayer
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger _logger;

        /// <summary>
        ///
        /// </summary>
        public ErrorHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ErrorHandlingMiddleware>()
                ?? throw new ArgumentNullException(nameof(loggerFactory)); ;
            this.next = next;
        }

        /// <summary>
        ///
        /// </summary>
        public async Task Invoke(HttpContext context /* other scoped dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {    
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            string reasonPhrase = null;
            object mes = null;

            switch (exception)
            {
                case NotFoundException ex:
                    {
                        code = HttpStatusCode.BadRequest;
                        reasonPhrase = "NotFoundException";
                        mes = new ErrorMessage
                        {
                            Alias = ex.Alias,
                            Message = ex.Message
                        };
                        break;
                    }
                case UnauthorizedException ex:
                    {
                        code = HttpStatusCode.Unauthorized;
                        break;
                    }
                default:
                    {
                        reasonPhrase = "Internal server error";
                        mes = new
                        {
                            exception.Message,
                            innerException = exception.InnerException?.Message,
                            exception.StackTrace
                        };
                        break;
                    }
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            var res = JsonConvert.SerializeObject(mes);
            context.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = reasonPhrase;
            return context.Response.WriteAsync(res);
        }
    }
}
