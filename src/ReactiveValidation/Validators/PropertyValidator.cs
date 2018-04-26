using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using ReactiveValidation.Extensions;
using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    public abstract class PropertyValidator<TObject, TProp> : IPropertyValidator<TObject, TProp>, IPropertyValidatorSettings<TObject>
        where TObject : IValidatableObject
    {
        private readonly ValidationMessageType _validationMessageType;
        private readonly HashSet<string> _relatedProperties;
        private readonly IStringSource _stringSource;

        private Func<TObject, bool> _condition;
        private IStringSource _overridedStringSource;

        protected PropertyValidator(IStringSource stringSource, ValidationMessageType validationMessageType, params LambdaExpression[] relatedProperties)
        {
            _stringSource = stringSource;
            _validationMessageType = validationMessageType;

            _relatedProperties = new HashSet<string>();

            AddRelatedProperties(relatedProperties);
        }


        public IEnumerable<string> RelatedProperties => _relatedProperties;


        public void SetStringSource(IStringSource stringSource)
        {
            _overridedStringSource.GuardNotCallTwice($"Methods 'WithMessage'/'WithLocalizedMessage' already have been called for {this.GetType()}");
            _overridedStringSource = stringSource;
        }

        public void ValidateWhen(Func<TObject, bool> condition, params LambdaExpression[] relatedProperties)
        {
            _condition.GuardNotCallTwice($"Method 'When' already have been called for {this.GetType()}");
            _condition = condition;

            AddRelatedProperties(relatedProperties);
        }


        public IEnumerable<ValidationMessage> ValidateProperty(ValidationContext<TObject, TProp> context)
        {
            if (_condition != null && _condition.Invoke(context.ValidatableObject) == false)
                return new ValidationMessage[0];

            if (IsValid(context) == true)
                return new ValidationMessage[0];


            var message = context.GetMessage(_overridedStringSource ?? _stringSource);
            var validationMessage = new ValidationMessage(message, _validationMessageType);
            return new []{ validationMessage };
        }

        protected abstract bool IsValid(ValidationContext<TObject, TProp> context);


        private void AddRelatedProperties(LambdaExpression[] relatedProperties)
        {
            if (relatedProperties?.Any() != true)
                return;

            foreach (var expression in relatedProperties)
            {
                var propertyName = ReactiveValidationHelper.GetPropertyName(typeof(TObject), expression);
                if (string.IsNullOrEmpty(propertyName) == false)
                    _relatedProperties.Add(propertyName);
            }
        }
    }
}
