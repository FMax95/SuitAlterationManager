using Microsoft.AspNetCore.Http;
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
    public class DisallowExceptionAttribute : BaseExceptionAttribute
    {
        public DisallowExceptionAttribute(ILoggerService loggerService)
            : base(loggerService)
        { }

        public override void OnException(ExceptionContext context)
        {
            LogException(context.Exception);

            var exceptionResponse = CreateExceptionResponse(context,
                                                            GetStatusCode(context.Exception),
                                                            ErrorCodes.ErrorOccured);
            context.Result = exceptionResponse;
        }

        protected override int GetStatusCode(Exception ex)
        {
            int statusCode;

            switch (ex)
            {
                case ForbiddenException:
                case ValidationException:
                case DomainException:
                case KeyNotFoundException:
                case NotFoundException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    break;
                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            return statusCode;
        }
    }
}
