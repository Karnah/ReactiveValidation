using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Validator which check items count of collection.
    /// </summary>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <typeparam name="TCollection">The type of collection.</typeparam>
    /// <typeparam name="TItem">The type of element of collection.</typeparam>
    public class CountValidator<TObject, TCollection, TItem> : PropertyValidator<TObject, TCollection>
        where TObject : IValidatableObject
        where TCollection : IEnumerable<TItem>
    {
        private readonly ValidatorParameter<TObject, int> _minCount;
        private readonly ValidatorParameter<TObject, int> _maxCount;

        /// <summary>
        /// Initialize a new instance of <see cref="CountValidator{TObject,TProp,TParam}" /> class.
        /// </summary>
        /// <param name="minCountExpression">Minimum count of items in collection (inclusive).</param>
        /// <param name="maxCountExpression">Maximum count of items in collection (inclusive).</param>
        /// <param name="validationMessageType">The type of validatable message.</param>
        public CountValidator(
            Expression<Func<TObject, int>> minCountExpression,
            Expression<Func<TObject, int>> maxCountExpression,
            ValidationMessageType validationMessageType)
            : this(new LanguageStringSource(ValidatorsNames.CountValidator), minCountExpression, maxCountExpression, validationMessageType)
        { }

        /// <summary>
        /// Initialize a new instance of <see cref="CountValidator{TObject,TProp,TParam}" /> class.
        /// </summary>
        /// <param name="stringSource">The string source of validatable message.</param>
        /// <param name="minCountExpression">Minimum count of items in collection (inclusive).</param>
        /// <param name="maxCountExpression">Maximum count of items in collection (inclusive).</param>
        /// <param name="validationMessageType">The type of validatable message.</param>
        protected CountValidator(
            IStringSource stringSource,
            Expression<Func<TObject, int>> minCountExpression,
            Expression<Func<TObject, int>> maxCountExpression,
            ValidationMessageType validationMessageType)
            : base(stringSource, validationMessageType, minCountExpression, maxCountExpression)
        {
            if (minCountExpression != null)
                _minCount = new ValidatorParameter<TObject, int>(minCountExpression);

            if (maxCountExpression != null)
                _maxCount = new ValidatorParameter<TObject, int>(maxCountExpression);
        }


        /// <inheritdoc />
        protected override bool IsValid(ValidationContext<TObject, TCollection> context)
        {
            int min = 0,
                max = 0;
            if (_minCount != null)
                min = context.GetParamValue(_minCount);
            if (_maxCount != null)
                max = context.GetParamValue(_maxCount);

            var totalCount = context.PropertyValue?.Count() ?? 0;
            if (totalCount < min || max < totalCount)
            {
                context.RegisterMessageArgument("MinCount", _minCount, min);
                context.RegisterMessageArgument("MaxCount", _maxCount, max);
                context.RegisterMessageArgument("TotalCount", null, totalCount);

                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// Validator which check that count of items in collection not less than minimum.
    /// </summary>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <typeparam name="TCollection">The type of collection.</typeparam>
    /// <typeparam name="TItem">The type of element of collection.</typeparam>
    public class MinCountValidator<TObject, TCollection, TItem> : CountValidator<TObject, TCollection, TItem>
        where TObject : IValidatableObject
        where TCollection : IEnumerable<TItem>
    {
        /// <summary>
        /// Initialize a new instance of <see cref="MinCountValidator{TObject,TProp,TParam}" /> class.
        /// </summary>
        /// <param name="minCountExpression">Minimum count of items in collection (inclusive).</param>
        /// <param name="validationMessageType">The type of validatable message.</param>
        public MinCountValidator(
            Expression<Func<TObject, int>> minCountExpression,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.MinCountValidator), minCountExpression, _ => int.MaxValue, validationMessageType)
        { }
    }


    /// <summary>
    /// Validator which check that count of items in collection not greater than maximum.
    /// </summary>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <typeparam name="TCollection">The type of collection.</typeparam>
    /// <typeparam name="TItem">The type of element of collection.</typeparam>
    public class MaxCountValidator<TObject, TCollection, TItem> : CountValidator<TObject, TCollection, TItem>
        where TObject : IValidatableObject
        where TCollection : IEnumerable<TItem>
    {
        /// <summary>
        /// Initialize a new instance of <see cref="MaxCountValidator{TObject,TProp,TParam}" /> class.
        /// </summary>
        /// <param name="maxCountExpression">Maximum count of items in collection (inclusive).</param>
        /// <param name="validationMessageType">The type of validatable message.</param>
        public MaxCountValidator(
            Expression<Func<TObject, int>> maxCountExpression,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.MaxCountValidator), _ => 0, maxCountExpression, validationMessageType)
        { }
    }


    /// <summary>
    /// Validator which check that count of items in collection equal to specified value.
    /// </summary>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <typeparam name="TCollection">The type of collection.</typeparam>
    /// <typeparam name="TItem">The type of element of collection.</typeparam>
    public class ExactCountValidator<TObject, TCollection, TItem> : CountValidator<TObject, TCollection, TItem>
        where TObject : IValidatableObject
        where TCollection : IEnumerable<TItem>
    {
        /// <summary>
        /// Initialize a new instance of <see cref="ExactCountValidator{TObject,TProp,TParam}" /> class.
        /// </summary>
        /// <param name="countExpression">Count of items in collection.</param>
        /// <param name="validationMessageType">The type of validatable message.</param>
        public ExactCountValidator(
            Expression<Func<TObject, int>> countExpression,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.ExactCountValidator), countExpression, countExpression, validationMessageType)
        { }
    }
}
