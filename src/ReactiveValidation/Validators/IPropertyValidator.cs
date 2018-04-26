using System.Collections.Generic;

namespace ReactiveValidation.Validators
{
    public interface IPropertyValidator<TObject, TProp>
        where TObject : IValidatableObject
    {
        IEnumerable<string> RelatedProperties { get; }


        IEnumerable<ValidationMessage> ValidateProperty(ValidationContext<TObject, TProp> context);
    }
}