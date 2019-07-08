using System.ComponentModel.DataAnnotations;
using Geev.Runtime.Validation;

namespace Geev.Zero.SampleApp.Users.Dto
{
    public class CustomValidateMethodInput : ICustomValidate
    {
        public void AddValidationErrors(CustomValidationContext context)
        {
            var message = context.Localize(GeevZeroConsts.LocalizationSourceName, "Identity.UserNotInRole");
            context.Results.Add(new ValidationResult(message));
        }
    }
}