using System;
using System.Linq.Expressions;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    public class LengthValidator <TObject> : PropertyValidator<TObject, string>
        where TObject : IValidatableObject
    {
        private readonly ValidatorParameter<TObject, int> _minLength;
        private readonly ValidatorParameter<TObject, int> _maxLength;

        public LengthValidator(
            Expression<Func<TObject, int>> minLengthExpression,
            Expression<Func<TObject, int>> maxLengthExpression,
            ValidationMessageType validationMessageType)
            : this(new LanguageStringSource(ValidatorsNames.LengthValidator), minLengthExpression, maxLengthExpression, validationMessageType)
        {}

        protected LengthValidator(
            IStringSource stringSource,
            Expression<Func<TObject, int>> minLengthExpression,
            Expression<Func<TObject, int>> maxLengthExpression,
            ValidationMessageType validationMessageType)
            : base(stringSource, validationMessageType, minLengthExpression, maxLengthExpression)
        {
            if (minLengthExpression != null)
                _minLength = new ValidatorParameter<TObject, int>(maxLengthExpression);

            if (maxLengthExpression != null)
                _maxLength = new ValidatorParameter<TObject, int>(maxLengthExpression);
        }


        protected override bool IsValid(ValidationContext<TObject, string> context)
        {
            int min = 0,
                max = 0;
            if (_minLength != null)
                min = context.GetParamValue(_minLength);
            if (_maxLength != null)
                max = context.GetParamValue(_maxLength);

            var totalLength = context.PropertyValue?.Length ?? 0;
            if (totalLength < min || max < totalLength) {
                context.RegisterMessageArgument("MinLength", _minLength, min);
                context.RegisterMessageArgument("MaxLength", _maxLength, max);
                context.RegisterMessageArgument("TotalLength", null, totalLength);

                return false;
            }

            return true;
        }
    }


    public class MinLengthValidator<TObject> : LengthValidator<TObject>
        where TObject : IValidatableObject
    {
        public MinLengthValidator(
            Expression<Func<TObject, int>> minLengthExpression,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.MinLengthValidator), minLengthExpression, _ => int.MaxValue, validationMessageType)
        { }
    }


    public class MaxLengthValidator<TObject> : LengthValidator<TObject>
        where TObject : IValidatableObject
    {
        public MaxLengthValidator(
            Expression<Func<TObject, int>> maxLengthExpression,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.MaxLengthValidator), _ => 0, maxLengthExpression, validationMessageType)
        { }
    }


    public class ExactLengthValidator<TObject> : LengthValidator<TObject>
        where TObject : IValidatableObject
    {
        public ExactLengthValidator(
            Expression<Func<TObject, int>> lengthExpression,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.ExactLengthValidator), lengthExpression, lengthExpression, validationMessageType)
        { }
    }
}
