using System;
using System.Collections.Generic;
using System.Linq;
using ReactiveValidation.Extensions;
using ReactiveValidation.ObjectObserver;
using ReactiveValidation.Resources.StringSources;
using ReactiveValidation.Validators;
using ReactiveValidation.Validators.Conditions;
using ReactiveValidation.Validators.PropertyValueTransformers;
using ReactiveValidation.Validators.Throttle;

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
        private readonly IValueTransformer<TObject, TProp>? _valueTransformer;
        private readonly List<IPropertyValidator<TObject>> _propertyValidators;

        private IPropertyValidator<TObject>? _currentValidator;
        private IValidationCondition<TObject>? _commonCondition;
        private IPropertiesThrottle? _commonThrottle;

        /// <summary>
        /// Create new base rule builder instance.
        /// </summary>
        /// <param name="validatableProperties">List of properties names which validating by this rules.</param>
        /// <param name="valueTransformer">Property value transformer.</param>
        protected BaseRuleBuilder(IReadOnlyList<string> validatableProperties, IValueTransformer<TObject, TProp>? valueTransformer = null)
        {
            _valueTransformer = valueTransformer;
            _propertyValidators = new List<IPropertyValidator<TObject>>();
            
            ValidatableProperties = validatableProperties;
            ObservingPropertiesSettings = new ObservingPropertySettings();
        }


        /// <inheritdoc />
        public CascadeMode? PropertyCascadeMode { get; set; }

        /// <inheritdoc />
        public IReadOnlyList<string> ValidatableProperties { get; }

        /// <inheritdoc />
        public ObservingPropertySettings ObservingPropertiesSettings { get; }

        /// <inheritdoc />
        public IReadOnlyList<IPropertyValidator<TObject>> GetValidators()
        {
            if (_commonCondition == null && _valueTransformer == null && _commonThrottle == null)
                return _propertyValidators;

            return _propertyValidators
                .Select(pv => new WrappingValidator<TObject, TProp>(pv, _commonCondition, _valueTransformer, _commonThrottle))
                .ToList();
        }

        /// <inheritdoc />
        public TBuilder SetValidator(IPropertyValidator<TObject> validator)
        {
            if (validator == null)
                throw new ArgumentNullException(nameof(validator));

            _propertyValidators.Add(validator);
            _currentValidator = validator;

            return This;
        }


        /// <inheritdoc />
        public TBuilder When(IValidationCondition<TObject> condition)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));

            _currentValidator.GuardNotNull("Current validator hasn't set");
            _currentValidator.ValidateWhen(condition);

            return This;
        }

        /// <inheritdoc />
        public TBuilder WithMessageSource(IStringSource stringSource)
        {
            if (stringSource == null)
                throw new ArgumentNullException(nameof(stringSource));

            _currentValidator.GuardNotNull("Current validator hasn't set");
            _currentValidator.SetStringSource(stringSource);

            return This;
        }

        /// <inheritdoc />
        public TBuilder Throttle(IPropertiesThrottle propertiesThrottle)
        {
            if (propertiesThrottle == null)
                throw new ArgumentNullException(nameof(propertiesThrottle));

            _currentValidator.GuardNotNull("Current validator hasn't set");
            _currentValidator.Throttle(propertiesThrottle);

            return This;
        }

        /// <inheritdoc />
        public IRuleBuilderOption<TObject, TProp> AllWhen(IValidationCondition<TObject> validationCondition)
        {
            if (validationCondition == null)
                throw new ArgumentNullException(nameof(validationCondition));

            _commonCondition.GuardNotCallTwice("Method 'AllWhen' has already been called");
            _commonCondition = validationCondition;

            return This;
        }

        /// <inheritdoc />
        public IRuleBuilderOption<TObject, TProp> CommonThrottle(IPropertiesThrottle propertiesThrottle)
        {
            if (propertiesThrottle == null)
                throw new ArgumentNullException(nameof(propertiesThrottle));

            _commonThrottle.GuardNotCallTwice("Method 'CommonThrottle' has already been called");
            _commonThrottle = propertiesThrottle;

            return This;
        }


        /// <summary>
        /// Reference to strong-typed current object.
        /// </summary>
        protected abstract TBuilder This { get; }
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
        public SinglePropertyRuleBuilder(string validatablePropertyName, IValueTransformer<TObject, TProp>? valueTransformer = null) : base(new []{ validatablePropertyName }, valueTransformer)
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
        BaseRuleBuilder<TObject, object?, IPropertiesRuleBuilder<TObject>>,
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