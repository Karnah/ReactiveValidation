using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    public class CountValidator<TObject, TCollection, TProp> : PropertyValidator<TObject, TCollection>
        where TObject : IValidatableObject
        where TCollection : IEnumerable<TProp>
    {
        private readonly ParameterInfo<TObject, int> _minCount;
        private readonly ParameterInfo<TObject, int> _maxCount;

        public CountValidator(
            Expression<Func<TObject, int>> minCountExpression,
            Expression<Func<TObject, int>> maxCountExpression,
            ValidationMessageType validationMessageType)
            : this(new LanguageStringSource(ValidatorsNames.CountValidator), minCountExpression, maxCountExpression, validationMessageType)
        { }

        protected CountValidator(
            IStringSource stringSource,
            Expression<Func<TObject, int>> minCountExpression,
            Expression<Func<TObject, int>> maxCountExpression,
            ValidationMessageType validationMessageType)
            : base(stringSource, validationMessageType, minCountExpression, maxCountExpression)
        {
            if (minCountExpression != null)
                _minCount = minCountExpression.GetParameterInfo();

            if (maxCountExpression != null)
                _maxCount = maxCountExpression.GetParameterInfo();
        }


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


    public class MinCountValidator<TObject, TCollection, TProp> : CountValidator<TObject, TCollection, TProp>
        where TObject : IValidatableObject
        where TCollection : IEnumerable<TProp>
    {
        public MinCountValidator(
            Expression<Func<TObject, int>> minCountExpression,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.MinCountValidator), minCountExpression, _ => int.MaxValue, validationMessageType)
        { }
    }


    public class MaxCountValidator<TObject, TCollection, TProp> : CountValidator<TObject, TCollection, TProp>
        where TObject : IValidatableObject
        where TCollection : IEnumerable<TProp>
    {
        public MaxCountValidator(
            Expression<Func<TObject, int>> maxCountExpression,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.MaxCountValidator), _ => 0, maxCountExpression, validationMessageType)
        { }
    }


    public class ExactCountValidator<TObject, TCollection, TProp> : CountValidator<TObject, TCollection, TProp>
        where TObject : IValidatableObject
        where TCollection : IEnumerable<TProp>
    {
        public ExactCountValidator(
            Expression<Func<TObject, int>> countExpression,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.ExactCountValidator), countExpression, countExpression, validationMessageType)
        { }
    }
}
