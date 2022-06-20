using System;
using System.Linq.Expressions;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Validator which check length of string.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    public class LengthValidator <TObject> : BaseSyncPropertyValidator<TObject, string>
        where TObject : IValidatableObject
    {
        private readonly ValidatorParameter<TObject, int> _minLength;
        private readonly ValidatorParameter<TObject, int> _maxLength;

        /// <summary>
        /// Initialize a new instance of <see cref="LengthValidator{TObject}" /> class.
        /// </summary>
        /// <param name="minLengthExpression">Minimum length of string (inclusive).</param>
        /// <param name="maxLengthExpression">Maximum length of string (inclusive).</param>
        /// <param name="validationMessageType">The type of validatable message.</param>
        public LengthValidator(
            Expression<Func<TObject, int>> minLengthExpression,
            Expression<Func<TObject, int>> maxLengthExpression,
            ValidationMessageType validationMessageType)
            : this(new LanguageStringSource(ValidatorsNames.LengthValidator), minLengthExpression, maxLengthExpression, validationMessageType)
        {}

        /// <summary>
        /// Initialize a new instance of <see cref="LengthValidator{TObject}" /> class.
        /// </summary>
        /// <param name="stringSource">The source of validatable message.</param>
        /// <param name="minLengthExpression">Minimum length of string (inclusive).</param>
        /// <param name="maxLengthExpression">Maximum length of string (inclusive).</param>
        /// <param name="validationMessageType">The type of validatable message.</param>
        protected LengthValidator(
            IStringSource stringSource,
            Expression<Func<TObject, int>> minLengthExpression,
            Expression<Func<TObject, int>> maxLengthExpression,
            ValidationMessageType validationMessageType)
            : base(stringSource, validationMessageType, minLengthExpression, maxLengthExpression)
        {
            if (minLengthExpression != null)
                _minLength = new ValidatorParameter<TObject, int>(minLengthExpression);

            if (maxLengthExpression != null)
                _maxLength = new ValidatorParameter<TObject, int>(maxLengthExpression);
        }


        /// <inheritdoc />
        protected override bool IsValid(ValidationContext<TObject, string> context)
        {
            int min = 0,
                max = 0;
            if (_minLength != null)
                min = context.GetParamValue(_minLength);
            if (_maxLength != null)
                max = context.GetParamValue(_maxLength);

            var totalLength = context.PropertyValue?.Length ?? 0;
            if (totalLength < min || max < totalLength)
            {
                context.RegisterMessageArgument("MinLength", _minLength, min);
                context.RegisterMessageArgument("MaxLength", _maxLength, max);
                context.RegisterMessageArgument("TotalLength", null, totalLength);

                return false;
            }

            return true;
        }
    }


    /// <summary>
    /// Validator which check that length of string is not less than minimum.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    public class MinLengthValidator<TObject> : LengthValidator<TObject>
        where TObject : IValidatableObject
    {
        /// <summary>
        /// Initialize a new instance of <see cref="MinLengthValidator{TObject}" /> class.
        /// </summary>
        /// <param name="minLengthExpression">Minimum length of string (inclusive).</param>
        /// <param name="validationMessageType">The type of validatable message.</param>
        public MinLengthValidator(
            Expression<Func<TObject, int>> minLengthExpression,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.MinLengthValidator), minLengthExpression, _ => int.MaxValue, validationMessageType)
        { }
    }


    /// <summary>
    /// Validator which check that length of string is not greater than maximum.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    public class MaxLengthValidator<TObject> : LengthValidator<TObject>
        where TObject : IValidatableObject
    {
        /// <summary>
        /// Initialize a new instance of <see cref="MaxLengthValidator{TObject}" /> class.
        /// </summary>
        /// <param name="maxLengthExpression">Maximum length of string (inclusive).</param>
        /// <param name="validationMessageType">The type of validatable message.</param>
        public MaxLengthValidator(
            Expression<Func<TObject, int>> maxLengthExpression,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.MaxLengthValidator), _ => 0, maxLengthExpression, validationMessageType)
        { }
    }


    /// <summary>
    /// Validator which check that length of string is equals to specified value.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    public class ExactLengthValidator<TObject> : LengthValidator<TObject>
        where TObject : IValidatableObject
    {
        /// <summary>
        /// Initialize a new instance of <see cref="ExactLengthValidator{TObject}" /> class.
        /// </summary>
        /// <param name="lengthExpression">Length of string.</param>
        /// <param name="validationMessageType">The type of validatable message.</param>
        public ExactLengthValidator(
            Expression<Func<TObject, int>> lengthExpression,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.ExactLengthValidator), lengthExpression, lengthExpression, validationMessageType)
        { }
    }
}
