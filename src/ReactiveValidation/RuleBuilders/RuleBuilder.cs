using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using ReactiveValidation.Exceptions;
using ReactiveValidation.Extensions;
using ReactiveValidation.Validators;

namespace ReactiveValidation
{
    /// <summary>
    /// Base class for creating validation rules for properties.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    /// <typeparam name="TProp">Type of validatable properties.</typeparam>
    /// <typeparam name="TInitial">Type of initial rule builder.</typeparam>
    /// <typeparam name="TBuilder">Type of main rule builder.</typeparam>
    internal abstract class BaseRuleBuilder<TObject, TProp, TInitial, TBuilder> :
        IRuleBuilder<TObject>,
        IRuleBuilderInitial<TObject, TProp, TBuilder>,
        IRuleBuilder<TObject, TProp, TBuilder>
            where TObject : IValidatableObject
            where TInitial : IRuleBuilderInitial<TObject, TProp, TBuilder>
            where TBuilder : IRuleBuilder<TObject, TProp, TBuilder>
    {
        private readonly List<IPropertyValidator<TObject>> _propertyValidators;
        private readonly IList<LambdaExpression> _relatedProperties;

        private IPropertyValidator<TObject> _currentValidator;
        private Func<TObject, bool> _commonCondition;

        /// <summary>
        /// Create new base rule builder instance.
        /// </summary>
        /// <param name="validatableProperties">List of properties names which validating by this rules.</param>
        protected BaseRuleBuilder(IReadOnlyList<string> validatableProperties)
        {
            ValidatableProperties = validatableProperties;

            _propertyValidators = new List<IPropertyValidator<TObject>>();
            _relatedProperties = new List<LambdaExpression>();
        }


        /// <inheritdoc />
        public IReadOnlyList<string> ValidatableProperties { get; }

        /// <inheritdoc />
        public IReadOnlyList<IPropertyValidator<TObject>> GetValidators()
        {
            if (_commonCondition == null)
                return _propertyValidators;

            var complexValidator = new ComplexValidator<TObject>(_commonCondition, _propertyValidators, _relatedProperties.ToArray());
            return new[] { complexValidator };
        }

        /// <inheritdoc />
        public TBuilder SetValidator(IPropertyValidator<TObject> validator)
        {
            _propertyValidators.Add(validator);
            _currentValidator = validator;

            return This;
        }


        #region When methods

        /// <inheritdoc />
        public TBuilder When(Func<bool> condition)
        {
            var wrappedCondition = WrapCondition(condition);
            return When(wrappedCondition);
        }

        /// <inheritdoc />
        public TBuilder When(Expression<Func<TObject, bool>> conditionProperty)
        {
            var condition = conditionProperty.Compile();
            return When(condition, conditionProperty);
        }

        /// <inheritdoc />
        public TBuilder When<TParam>(
            Expression<Func<TObject, TParam>> property,
            Func<TParam, bool> condition)
        {
            var wrappedCondition = WrapCondition(property.Compile(), condition);
            return When(wrappedCondition, property);
        }

        /// <inheritdoc />
        public TBuilder When<TParam1, TParam2>(
            Expression<Func<TObject, TParam1>> property1,
            Expression<Func<TObject, TParam2>> property2,
            Func<TParam1, TParam2, bool> condition)
        {
            var wrappedCondition = WrapCondition(property1.Compile(), property2.Compile(), condition);
            return When(wrappedCondition, property1, property2);
        }

        /// <inheritdoc />
        public TBuilder When<TParam1, TParam2, TParam3>(
            Expression<Func<TObject, TParam1>> property1,
            Expression<Func<TObject, TParam2>> property2,
            Expression<Func<TObject, TParam3>> property3,
            Func<TParam1, TParam2, TParam3, bool> condition)
        {
            var wrappedCondition = WrapCondition(property1.Compile(), property2.Compile(), property3.Compile(), condition);
            return When(wrappedCondition, property1, property2, property3);
        }


        private TBuilder When(Func<TObject, bool> condition, params LambdaExpression[] relatedProperties)
        {
            if (_currentValidator is IPropertyValidatorSettings<TObject> validatorSettings)
            {
                validatorSettings.ValidateWhen(condition, relatedProperties);
            }
            else if (_currentValidator is WrappingValidator<TObject> wrappingValidator)
            {
                throw new MethodAlreadyCalledException($"Method 'When' already have been called for {wrappingValidator.InnerValidator.GetType()}");
            }
            else
            {
                var wrappedValidator = new WrappingValidator<TObject>(condition, _currentValidator, relatedProperties);

                ReplaceValidator(_currentValidator, wrappedValidator);
                _currentValidator = wrappedValidator;
            }

            return This;
        }

        #endregion


        #region WithMessage/WithLocalizedMessage methods

        /// <inheritdoc />
        public TBuilder WithMessage(string message)
        {
            if (_currentValidator is IPropertyValidatorSettings<TObject> validatorSettings)
            {
                validatorSettings.SetStringSource(new StaticStringSource(message));
            }
            else
            {
                throw new NotImplementedException($"{_currentValidator.GetType()} must implement IPropertyValidatorSettings<TObject> for {nameof(WithMessage)} method");
            }

            return This;
        }

        /// <inheritdoc />
        public TBuilder WithLocalizedMessage(string messageKey)
        {
            if (_currentValidator is IPropertyValidatorSettings<TObject> validatorSettings)
            {
                validatorSettings.SetStringSource(new LanguageStringSource(messageKey));
            }
            else
            {
                throw new NotImplementedException($"{_currentValidator.GetType()} must implement IPropertyValidatorSettings<TObject> for {nameof(WithMessage)} method");
            }

            return This;
        }

        /// <inheritdoc />
        public TBuilder WithLocalizedMessage(string resource, string messageKey)
        {
            if (_currentValidator is IPropertyValidatorSettings<TObject> validatorSettings)
            {
                validatorSettings.SetStringSource(new LanguageStringSource(resource, messageKey));
            }
            else
            {
                throw new NotImplementedException($"{_currentValidator.GetType()} must implement IPropertyValidatorSettings<TObject> for {nameof(WithMessage)} method");
            }

            return This;
        }

        #endregion


        #region AllWhen methods

        /// <inheritdoc />
        public IRuleBuilderOption<TObject, TProp> AllWhen(Func<bool> condition)
        {
            _commonCondition.GuardNotCallTwice("Method 'AllWhen' already have been called");

            _commonCondition = WrapCondition(condition);

            return this;
        }

        /// <inheritdoc />
        public IRuleBuilderOption<TObject, TProp> AllWhen(Expression<Func<TObject, bool>> conditionProperty)
        {
            _commonCondition.GuardNotCallTwice("Method 'AllWhen' already have been called");

            _relatedProperties.Add(conditionProperty);
            _commonCondition = conditionProperty.Compile();

            return This;
        }

        /// <inheritdoc />
        public IRuleBuilderOption<TObject, TProp> AllWhen<TParam>(
            Expression<Func<TObject, TParam>> property,
            Func<TParam, bool> condition)
        {
            _commonCondition.GuardNotCallTwice("Method 'AllWhen' already have been called");

            _relatedProperties.Add(property);
            _commonCondition = WrapCondition(property.Compile(), condition);

            return This;
        }

        /// <inheritdoc />
        public IRuleBuilderOption<TObject, TProp> AllWhen<TParam1, TParam2>(
            Expression<Func<TObject, TParam1>> property1,
            Expression<Func<TObject, TParam2>> property2,
            Func<TParam1, TParam2, bool> condition)
        {
            _commonCondition.GuardNotCallTwice("Method 'AllWhen' already have been called");

            _relatedProperties.Add(property1);
            _relatedProperties.Add(property2);
            _commonCondition = WrapCondition(property1.Compile(), property2.Compile(), condition);

            return This;
        }

        /// <inheritdoc />
        public IRuleBuilderOption<TObject, TProp> AllWhen<TParam1, TParam2, TParam3>(
            Expression<Func<TObject, TParam1>> property1,
            Expression<Func<TObject, TParam2>> property2,
            Expression<Func<TObject, TParam3>> property3,
            Func<TParam1, TParam2, TParam3, bool> condition)
        {
            _commonCondition.GuardNotCallTwice("Method 'AllWhen' already have been called");

            _relatedProperties.Add(property1);
            _relatedProperties.Add(property2);
            _relatedProperties.Add(property3);
            _commonCondition = WrapCondition(property1.Compile(), property2.Compile(), property3.Compile(), condition);

            return This;
        }

        #endregion


        /// <summary>
        /// Reference to strong-typed current object.
        /// </summary>
        protected abstract TBuilder This { get; }


        /// <summary>
        /// Replace one validator by another.
        /// </summary>
        /// <param name="oldValidator">Old validator.</param>
        /// <param name="newValidator">New validator.</param>
        private void ReplaceValidator(
            IPropertyValidator<TObject> oldValidator,
            IPropertyValidator<TObject> newValidator)
        {
            var index = _propertyValidators.IndexOf(oldValidator);
            _propertyValidators[index] = newValidator;
        }


        private static Func<TObject, bool> WrapCondition(Func<bool> condition)
        {
            return _ => condition();
        }

        private static Func<TObject, bool> WrapCondition<TParam>(
            Func<TObject, TParam> paramFunc,
            Func<TParam, bool> condition)
        {
            return instance =>
            {
                var param = paramFunc.Invoke(instance);
                return condition.Invoke(param);
            };
        }

        private static Func<TObject, bool> WrapCondition<TParam1, TParam2>(
            Func<TObject, TParam1> param1Func,
            Func<TObject, TParam2> param2Func,
            Func<TParam1, TParam2, bool> condition)
        {
            return instance =>
            {
                var param1 = param1Func.Invoke(instance);
                var param2 = param2Func.Invoke(instance);

                return condition.Invoke(param1, param2);
            };
        }

        private static Func<TObject, bool> WrapCondition<TParam1, TParam2, TParam3>(
            Func<TObject, TParam1> param1Func,
            Func<TObject, TParam2> param2Func,
            Func<TObject, TParam3> param3Func,
            Func<TParam1, TParam2, TParam3, bool> condition)
        {
            return instance =>
            {
                var param1 = param1Func.Invoke(instance);
                var param2 = param2Func.Invoke(instance);
                var param3 = param3Func.Invoke(instance);

                return condition.Invoke(param1, param2, param3);
            };
        }
    }


    /// <summary>
    /// Rule builder for single property.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    /// <typeparam name="TProp">Type of validatable property.</typeparam>
    internal class SinglePropertyRuleBuilder<TObject, TProp> :
        BaseRuleBuilder<TObject, TProp, ISinglePropertyRuleBuilderInitial<TObject, TProp>, ISinglePropertyRuleBuilder<TObject, TProp>>,
        ISinglePropertyRuleBuilderInitial<TObject, TProp>,
        ISinglePropertyRuleBuilder<TObject, TProp>
            where TObject : IValidatableObject
    {
        /// <inheritdoc />
        public SinglePropertyRuleBuilder(string validatablePropertyName) : base(new []{ validatablePropertyName })
        {
        }

        /// <inheritdoc />
        protected override ISinglePropertyRuleBuilder<TObject, TProp> This => this;
    }


    /// <summary>
    /// Rule builder for several properties.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    internal class PropertiesRuleBuilder<TObject> :
        BaseRuleBuilder<TObject, object, IPropertiesRuleBuilderInitial<TObject>, IPropertiesRuleBuilder<TObject>>,
        IPropertiesRuleBuilderInitial<TObject>,
        IPropertiesRuleBuilder<TObject>
            where TObject : IValidatableObject
    {
        /// <inheritdoc />
        public PropertiesRuleBuilder(IReadOnlyList<string> validatableProperties) : base(validatableProperties)
        {
        }

        /// <inheritdoc />
        protected override IPropertiesRuleBuilder<TObject> This => this;
    }


    /// <summary>
    /// Rule builder for property of collection type.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    /// <typeparam name="TCollection">Type of collection.</typeparam>
    /// <typeparam name="TProp">Type of collection item.</typeparam>
    internal class CollectionPropertyRuleBuilder<TObject, TCollection, TProp> :
        BaseRuleBuilder<TObject, TCollection, ICollectionRuleBuilderInitial<TObject, TCollection, TProp>, ICollectionRuleBuilder<TObject, TCollection, TProp>>,
        ICollectionRuleBuilderInitial<TObject, TCollection, TProp>,
        ICollectionRuleBuilder<TObject, TCollection, TProp>
            where TObject : IValidatableObject
            where TCollection : IEnumerable<TProp>
    {
        /// <inheritdoc />
        public CollectionPropertyRuleBuilder(string validatablePropertyName) : base(new[] { validatablePropertyName })
        {
        }

        /// <inheritdoc />
        protected override ICollectionRuleBuilder<TObject, TCollection, TProp> This => this;
    }
}