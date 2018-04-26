using System;
using System.Linq.Expressions;

namespace ReactiveValidation.Validators
{
    public interface IPropertyValidatorSettings<out TObject>
    {
        void SetStringSource(IStringSource stringSource);

        void ValidateWhen(Func<TObject, bool> condition, params LambdaExpression[] relatedProperties);
    }
}
