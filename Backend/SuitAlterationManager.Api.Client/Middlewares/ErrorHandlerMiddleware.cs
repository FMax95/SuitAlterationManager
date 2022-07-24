using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SuitAlterationManager.Api.Client;
using SuitAlterationManager.Api.Client.Base;
using SuitAlterationManager.Domain.Base.Attributes;
using SuitAlterationManager.Domain.Base.Validation;
using SuitAlterationManager.Infrastructure.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SuitAlterationManager.Api.Middlewares
{
    public static class ExceptionMiddleware
	{
	}

	public class ErrorHandlerMiddleware
	{
		private readonly RequestDelegate next;
		private readonly ILoggerService _loggerService;
		public ErrorHandlerMiddleware(RequestDelegate next, ILoggerService loggerService)
		{
			this.next = next;
			this._loggerService = loggerService;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				if (ExceptionMustBeLogged(ex))
					_loggerService.Error<ErrorHandlerMiddleware>("ERROR!", ex);

				int statusCode;
				Envelope envelope;

				switch (ex)
				{
					case UnauthorizedAccessException e:
						statusCode = (int)HttpStatusCode.Unauthorized;
						envelope = Envelope.Failure(e.ToError());
						break;
					case ApplicationServiceException e:
						statusCode = (int)HttpStatusCode.BadRequest;
						envelope = Envelope.Failure(e.ToError());
						break;
					case DomainException e:
						statusCode = (int)HttpStatusCode.BadRequest;
						envelope = Envelope.Failure(e.ToError());
						break;
					case KeyNotFoundException e:
						statusCode = (int)HttpStatusCode.NotFound;
						envelope = Envelope.Failure(e.ToError());
						break;
					default:
						statusCode = (int)HttpStatusCode.InternalServerError;
						envelope = Envelope.Failure(ex.ToDefaultError());
						break;
				}
				
				context.Response.StatusCode = statusCode;
				context.Response.ContentType = "application/json";

				await context.Response.WriteAsync(envelope.ToJson());
			}
		}

        /// <summary>
        /// Returns true if the exception have to be logged
        /// </summary>
        /// <param name="ex"></param>
        /// <returns>true if the exception doesn't have the ExcludeFromLogging attribute and it's not a ValidationException</returns>
        private static bool ExceptionMustBeLogged(Exception ex)
        {
            return (ex.GetType().GetCustomAttributes(typeof(ExcludeFromLogging), true).Any() == false);
        }
    }

	public class Error
	{
		public string Code { get; }
		public string Message { get; }
		public object[] Parameters { get; }

		public Error(string code, string message = null)
		{
			Code = code;
			Message = message;
		}

		public Error(Exception exception)
		{
			Message = exception.Message;
		}

		public Error(Domain.Base.Validation.ApplicationServiceException exception)
		{
			Code = exception.Code;
			Message = exception.Message;
			Parameters = exception.Parameters;
		}
	}
	public static class ErrorExtensions
	{
		public static Error ToError(this Domain.Base.Validation.ApplicationServiceException domainException) => new Error(domainException);
		public static Error ToError(this Exception exception) => new Error(exception);
		public static Error ToDefaultError(this Exception exception) => new Error("UnhandledException", exception.Message);
	}

}
