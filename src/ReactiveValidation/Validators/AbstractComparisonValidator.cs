﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    public abstract class AbstractComparisonValidator<TObject, TProp, TParam> : PropertyValidator<TObject, TProp>
        where TObject : IValidatableObject
    {
        private readonly IComparer _comparer;
        private readonly ParameterInfo<TObject, TParam> _valueToCompare;

        protected AbstractComparisonValidator(
            IStringSource stringSource,
            Expression<Func<TObject, TParam>> valueToCompareExpression,
            IComparer comparer,
            ValidationMessageType validationMessageType)
            : base(stringSource, validationMessageType, valueToCompareExpression)
        {
            _comparer = comparer ?? Comparer<TProp>.Default;
            _valueToCompare = valueToCompareExpression.GetParameterInfo();
        }

        protected sealed override bool IsValid(ValidationContext<TObject, TProp> context)
        {
            var propertyValue = context.PropertyValue;
            if (propertyValue == null)
                return true;

            var paramValue = context.GetParamValue(_valueToCompare);
            var comparationResult = _comparer.Compare(propertyValue, paramValue);

            if (IsValid(comparationResult) == false) {
                context.RegisterMessageArgument("ValueToCompare", _valueToCompare, paramValue);
                return false;
            }

            return true;
        }

        protected abstract bool IsValid(int comparationResult);
    }
}