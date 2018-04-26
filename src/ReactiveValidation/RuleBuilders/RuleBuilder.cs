using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Resources;

using ReactiveValidation.Adapters;
using ReactiveValidation.Exceptions;
using ReactiveValidation.Extensions;
using ReactiveValidation.Validators;

namespace ReactiveValidation
{
    internal abstract class BaseRuleBuilder<TObject, TProp, TInitial, TBuilder> :
        IRuleBuilderInitial<TObject, TProp, TBuilder>,
        IRuleBuilder<TObject, TProp, TBuilder>,
        IAdapterBuilder<TObject>
            where TObject : IValidatableObject
            where TInitial : IRuleBuilderInitial<TObject, TProp, TBuilder>
            where TBuilder : IRuleBuilder<TObject, TProp, TBuilder>
    {
        private readonly List<IPropertyValidator<TObject, TProp>> _propertyValidators;
        private readonly IList<LambdaExpression> _relatedProperties;

        private IPropertyValidator<TObject, TProp> _currentValidator;
        private Func<TObject, bool> _commonCondition;

        protected BaseRuleBuilder()
        {
            _propertyValidators = new List<IPropertyValidator<TObject, TProp>>();
            _relatedProperties = new List<LambdaExpression>();
        }


        public abstract IPropertiesAdapter Build(ObjectValidator<TObject> validator, params string[] properties);

        public TBuilder SetValidator(IPropertyValidator<TObject, TProp> validator)
        {
            _propertyValidators.Add(validator);
            _currentValidator = validator;

            return This;
        }


        #region When methods

        public TBuilder When(Func<bool> condition)
        {
            var wrappedCondition = WrapCondition(condition);
            return When(wrappedCondition);
        }

        public TBuilder When<TParam>(Expression<Func<TObject, bool>> conditionProperty)
        {
            var condition = conditionProperty.Compile();
            return When(condition, conditionProperty);
        }

        public TBuilder When<TParam>(
            Expression<Func<TObject, TParam>> property,
            Func<TParam, bool> condition)
        {
            var wrappedCondition = WrapCondition(property.Compile(), condition);
            return When(wrappedCondition, property);
        }

        public TBuilder When<TParam1, TParam2>(
            Expression<Func<TObject, TParam1>> property1,
            Expression<Func<TObject, TParam2>> property2,
            Func<TParam1, TParam2, bool> condition)
        {
            var wrappedCondition = WrapCondition(property1.Compile(), property2.Compile(), condition);
            return When(wrappedCondition, property1, property2);
        }

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
            if (_currentValidator is IPropertyValidatorSettings<TObject> validatorSettings) {
                validatorSettings.ValidateWhen(condition, relatedProperties);
            }
            else if (_currentValidator is WrappingValidator<TObject, TProp> wrappingValidator) {
                throw new MethodAlreadyCalledException($"Method 'When' already have been called for {wrappingValidator.InnerValidator.GetType()}");
            }
            else {
                var wrappedValidator =
                    new WrappingValidator<TObject, TProp>(condition, _currentValidator, relatedProperties);

                ReplaceValidator(_currentValidator, wrappedValidator);
                _currentValidator = wrappedValidator;
            }

            return This;
        }

        #endregion


        #region WithMessage/WithLocalizedMessage methods

        public TBuilder WithMessage(string message)
        {
            if (_currentValidator is IPropertyValidatorSettings<TObject> validatorSettings) {
                validatorSettings.SetStringSource(new StaticStringSource(message));
            }
            else {
                throw new NotImplementedException(
                    $"{_currentValidator.GetType()} must implement IPropertyValidatorSettings<TObject> for {nameof(WithMessage)} method");
            }

            return This;
        }

        public TBuilder WithLocalizedMessage(string messageKey)
        {
            if (_currentValidator is IPropertyValidatorSettings<TObject> validatorSettings) {
                validatorSettings.SetStringSource(new LanguageStringSource(messageKey));
            }
            else {
                throw new NotImplementedException(
                    $"{_currentValidator.GetType()} must implement IPropertyValidatorSettings<TObject> for {nameof(WithMessage)} method");
            }

            return This;
        }

        public TBuilder WithLocalizedMessage(ResourceManager resourceManager, string messageKey)
        {
            if (_currentValidator is IPropertyValidatorSettings<TObject> validatorSettings) {
                validatorSettings.SetStringSource(new LanguageStringSource(resourceManager, messageKey));
            }
            else {
                throw new NotImplementedException(
                    $"{_currentValidator.GetType()} must implement IPropertyValidatorSettings<TObject> for {nameof(WithMessage)} method");
            }

            return This;
        }

        #endregion


        #region AllWhen methods

        public IRuleBuilderOption<TObject, TProp> AllWhen(Func<bool> condition)
        {
            _commonCondition.GuardNotCallTwice("Method 'AllWhen' already have been called");

            _commonCondition = WrapCondition(condition);

            return this;
        }

        public IRuleBuilderOption<TObject, TProp> AllWhen(Expression<Func<TObject, bool>> conditionProperty)
        {
            _commonCondition.GuardNotCallTwice("Method 'AllWhen' already have been called");

            _relatedProperties.Add(conditionProperty);
            _commonCondition = conditionProperty.Compile();

            return This;
        }

        public IRuleBuilderOption<TObject, TProp> AllWhen<TParam>(
            Expression<Func<TObject, TParam>> property,
            Func<TParam, bool> condition)
        {
            _commonCondition.GuardNotCallTwice("Method 'AllWhen' already have been called");

            _relatedProperties.Add(property);
            _commonCondition = WrapCondition(property.Compile(), condition);

            return This;
        }

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


        protected abstract TBuilder This { get; }

        protected IReadOnlyCollection<IPropertyValidator<TObject, TProp>> GetPropertyValidators()
        {
            if (_commonCondition == null)
                return _propertyValidators;

            var complexValidator = new ComplexValidator<TObject, TProp>(_commonCondition, _propertyValidators, _relatedProperties.ToArray());
            return new[] { complexValidator };
        }


        private void ReplaceValidator(
            IPropertyValidator<TObject, TProp> oldValidator,
            IPropertyValidator<TObject, TProp> newValidator)
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
            return instance => {
                var param = paramFunc.Invoke(instance);
                return condition.Invoke(param);
            };
        }

        private static Func<TObject, bool> WrapCondition<TParam1, TParam2>(
            Func<TObject, TParam1> param1Func,
            Func<TObject, TParam2> param2Func,
            Func<TParam1, TParam2, bool> condition)
        {
            return instance => {
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
            return instance => {
                var param1 = param1Func.Invoke(instance);
                var param2 = param2Func.Invoke(instance);
                var param3 = param3Func.Invoke(instance);

                return condition.Invoke(param1, param2, param3);
            };
        }
    }


    internal class SinglePropertyRuleBuilder<TObject, TProp> :
        BaseRuleBuilder<TObject, TProp, ISinglePropertyRuleBuilderInitial<TObject, TProp>, ISinglePropertyRuleBuilder<TObject, TProp>>,
        ISinglePropertyRuleBuilderInitial<TObject, TProp>,
        ISinglePropertyRuleBuilder<TObject, TProp>
            where TObject : IValidatableObject
    {
        protected override ISinglePropertyRuleBuilder<TObject, TProp> This => this;

        public override IPropertiesAdapter Build(ObjectValidator<TObject> validator, params string[] properties)
        {
            var adapter = new SinglePropertyAdapter<TObject, TProp>(
                validator, GetPropertyValidators(), properties.First());

            return adapter;
        }
    }


    internal class PropertiesRuleBuilder<TObject> :
        BaseRuleBuilder<TObject, object, IPropertiesRuleBuilderInitial<TObject>, IPropertiesRuleBuilder<TObject>>,
        IPropertiesRuleBuilderInitial<TObject>,
        IPropertiesRuleBuilder<TObject>
            where TObject : IValidatableObject
    {
        protected override IPropertiesRuleBuilder<TObject> This => this;

        public override IPropertiesAdapter Build(ObjectValidator<TObject> validator, params string[] properties)
        {
            var adapter = new PropertiesAdapter<TObject>(
                validator, GetPropertyValidators(), properties);

            return adapter;
        }
    }


    internal class CollectionPropertyRuleBuilder<TObject, TCollection, TProp> :
        BaseRuleBuilder<TObject, TCollection, ICollectionRuleBuilderInitial<TObject, TCollection, TProp>, ICollectionRuleBuilder<TObject, TCollection, TProp>>,
        ICollectionRuleBuilderInitial<TObject, TCollection, TProp>,
        ICollectionRuleBuilder<TObject, TCollection, TProp>
            where TObject : IValidatableObject
            where TCollection : IEnumerable<TProp>
    {
        protected override ICollectionRuleBuilder<TObject, TCollection, TProp> This => this;

        public override IPropertiesAdapter Build(ObjectValidator<TObject> validator, params string[] properties)
        {
            var adapter = new CollectionPropertyAdapter<TObject, TCollection, TProp>(
                validator, GetPropertyValidators(), properties.First());

            return adapter;
        }
    }
}