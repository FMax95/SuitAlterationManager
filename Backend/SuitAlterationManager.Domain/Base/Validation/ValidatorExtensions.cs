using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace SuitAlterationManager.Domain.Base.Validation
{
    public static class ValidatorExtensions
    {
        public static async Task<ValidationResult> ValidateAsync<T>(this AbstractValidator<T> validator, T entity, string action)
        {
            var context = new ValidationContext<T>(entity)
            {
                RootContextData = {[ValidationActions.Action] = action}
            };
            return await validator.ValidateAsync(context);
        }
        
        public static async Task<ValidationResult> ValidateAndThrowAsync<T>(this AbstractValidator<T> validator, T entity, string action)
        {
            var context = ValidationContext<T>.CreateWithOptions(entity, options => options.ThrowOnFailures());
            context.RootContextData[ValidationActions.Action] = action;
            return await validator.ValidateAsync(context);
        }
    }
}