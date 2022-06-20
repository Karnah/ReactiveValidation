using System;
using System.Collections.Generic;
using System.Linq;
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
    /// <typeparam name="TBuilder">Type of main rule builder.</typeparam>
    internal abstract class BaseRuleBuilder<TObject, TProp, TBuilder> :
        IRuleBuilder<TObject>,
        IRuleBuilder<TObject, TProp, TBuilder>
            where TObject : IValidatableObject
            where TBuilder : IRuleBuilder<TObject, TProp, TBuilder>
    {
        private readonly List<IPropertyValidator<TObject>> _propertyValidators;

        private IPropertyValidator<TObject> _currentValidator;
        private IValidationCondition<TObject> _commonCondition;

        /// <summary>
        /// Create new base rule builder instance.
        /// </summary>
        /// <param name="validatableProperties">List of properties names which validating by this rules.</param>
        protected BaseRuleBuilder(IReadOnlyList<string> validatableProperties)
        {
            ValidatableProperties = validatableProperties;
            ObservingPropertiesSettings = new ObservingPropertySettings();

            _propertyValidators = new List<IPropertyValidator<TObject>>();
        }


        /// <inheritdoc />
        public IReadOnlyList<string> ValidatableProperties { get; }

        /// <inheritdoc />
        public ObservingPropertySettings ObservingPropertiesSettings { get; }

        /// <inheritdoc />
        public IReadOnlyList<IPropertyValidator<TObject>> GetValidators()
        {
            if (_commonCondition == null)
                return _propertyValidators;

            return _propertyValidators
                .Select(pv => new WrappingValidator<TObject>(_commonCondition, pv))
                .ToList();
        }

        /// <inheritdoc />
        public TBuilder SetValidator(IPropertyValidator<TObject> validator)
        {
            _propertyValidators.Add(validator);
            _currentValidator = validator;

            return This;
        }


        /// <inheritdoc />
        public TBuilder When(IValidationCondition<TObject> condition)
        {
            if (_currentValidator is IPropertyValidatorSettings<TObject> validatorSettings)
            {
                validatorSettings.ValidateWhen(condition);
            }
            else if (_currentValidator is WrappingValidator<TObject> wrappingValidator)
            {
                throw new MethodAlreadyCalledException($"Method 'When' already have been called for {wrappingValidator.InnerValidator.GetType()}");
            }
            else
            {
                var wrappedValidator = new WrappingValidator<TObject>(condition, _currentValidator);

                ReplaceValidator(_currentValidator, wrappedValidator);
                _currentValidator = wrappedValidator;
            }

            return This;
        }


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

        
        /// <inheritdoc />
        public IRuleBuilderOption<TObject, TProp> AllWhen(IValidationCondition<TObject> validationCondition)
        {
            _commonCondition.GuardNotCallTwice("Method 'AllWhen' already have been called");
            _commonCondition = validationCondition;

            return This;
        }

        
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
    }


    /// <summary>
    /// Rule builder for single property.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    /// <typeparam name="TProp">Type of validatable property.</typeparam>
    internal class SinglePropertyRuleBuilder<TObject, TProp> :
        BaseRuleBuilder<TObject, TProp, ISinglePropertyRuleBuilder<TObject, TProp>>,
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
        BaseRuleBuilder<TObject, object, IPropertiesRuleBuilder<TObject>>,
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
    /// <typeparam name="TItem">Type of collection item.</typeparam>
    internal class CollectionPropertyRuleBuilder<TObject, TCollection, TItem> :
        BaseRuleBuilder<TObject, TCollection, ICollectionRuleBuilder<TObject, TCollection, TItem>>,
        ICollectionRuleBuilder<TObject, TCollection, TItem>
            where TObject : IValidatableObject
            where TCollection : IEnumerable<TItem>
    {
        /// <inheritdoc />
        public CollectionPropertyRuleBuilder(string validatablePropertyName) : base(new[] { validatablePropertyName })
        {
        }

        /// <inheritdoc />
        protected override ICollectionRuleBuilder<TObject, TCollection, TItem> This => this;
    }
}