using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;

namespace SuitAlterationManager.Domain.Base.Validation
{
    public class Error
    {
        public bool FromValidation { get; }
        public string Code { get; }
        public string Message { get; }
        public IList<ValidationField> Fields { get; }
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
        
        public Error(DomainException exception)
        {
            Code = exception.Code;
            Message = exception.Message;
            Parameters = exception.Parameters;
        }

		public Error(NotFoundException exception)
		{
			Code = exception.Code;
			Message = exception.Message;
			Parameters = exception.Parameters;
		}

		public Error(ValidationResult validationResult)
        {
            FromValidation = true;
            Code = ErrorCodes.InvalidData;
            Fields = validationResult.Errors.ToValidationFields();
        }
        
        public Error(ValidationException validationException)
        {
            FromValidation = true;
            Code = ErrorCodes.InvalidData;
            Fields = validationException.Errors.ToValidationFields();
        }
        
    }

    public class ValidationField
    {
        public string Name { get; set; }
        public IList<ErrorField> Errors { get; set; }
    }

    public class ErrorField
    {
        public string Message { get; set; }
        public string Code { get; set; }
    }

    public static class ErrorExtensions
    {
        internal static List<ValidationField> ToValidationFields(this IEnumerable<ValidationFailure> errors)
        {
            return errors
                .GroupBy(e => e.PropertyName)
                .Select(g => new ValidationField
                {
                    Name = g.Key,
                    Errors = g.Select(e => new ErrorField
                    {
                        Message = e.ErrorMessage,
                        Code = e.ErrorCode
                    }).ToList()
                }).ToList();
        }
        
        public static Error ToError(this ValidationResult validationResult) => new Error(validationResult);
        
        public static Error ToError(this ValidationException validationException) => new Error(validationException);
        
        public static Error ToError(this DomainException domainException) => new Error(domainException);
        
        public static Error ToError(this Exception exception) => new Error(exception);
        public static Error ToError(this NotFoundException exception) => new Error(exception);
    }
}