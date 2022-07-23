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
    public class AllowExceptionAttribute : BaseExceptionAttribute
    {
        private readonly Type[] allowedTypes;

        public AllowExceptionAttribute(ILoggerService loggerService, Type[] allowedTypes = null)
            : base(loggerService)
        {
            this.allowedTypes = allowedTypes;
        }

        public override void OnException(ExceptionContext context)
        {
            if (IsExceptionAllowed(context.Exception))
            {
                LogException(context.Exception);

                var exceptionResponse = CreateExceptionResponse(context,
                                                                GetStatusCode(context.Exception),
                                                                context.Exception.Message);
                context.Result = exceptionResponse;
            }
        }

        /// <summary>
        /// Let exception pass if allowedTypes id not valorized (no restrictions) or if the exception type is contained in allowedTypes 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool IsExceptionAllowed(Exception exception)
        {
            return allowedTypes == null
                   || !allowedTypes.Any()
                   || (allowedTypes.Any() && allowedTypes.Contains(exception.GetType()));
        }
    }
}
