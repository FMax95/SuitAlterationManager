using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SuitAlterationManager.Domain.Base.Attributes;
using SuitAlterationManager.Domain.Base.Validation;
using SuitAlterationManager.Infrastructure.Logger;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace SuitAlterationManager.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class BaseExceptionAttribute : ExceptionFilterAttribute
    {
        protected readonly ILoggerService loggerService;

        public BaseExceptionAttribute(ILoggerService loggerService)
        {
            this.loggerService = loggerService;
        }

        protected void LogException(Exception exception)
        {
            if (exception.GetType().GetCustomAttributes(typeof(ExcludeFromLogging), true).Any() == false)
            {
                loggerService.Error<AllowExceptionAttribute>("ERROR!", exception);
            }
        }

        public JsonResult CreateExceptionResponse(ExceptionContext context, int statusCode, string responseMessage)
        {
            // Define response
            var response = context.HttpContext.Response;
            response.ContentType = "application/json";
            response.StatusCode = statusCode;

            // Define default exception and response value
            var responseException = new Exception(responseMessage)
            {
                Source = context.Exception.Source,
            };
            Envelope envelope = Envelope.Failure(responseException.ToError());

            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            return new JsonResult(envelope, options);
        }

        protected virtual int GetStatusCode(Exception ex)
        {
            int statusCode;

            switch (ex)
            {
                case ForbiddenException:
                    statusCode = (int)HttpStatusCode.Forbidden;
                    break;
                case ValidationException:
                    statusCode = (int)HttpStatusCode.UnprocessableEntity;
                    break;
                case DomainException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case KeyNotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    break;
                case NotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            return statusCode;
        }
    }
}
