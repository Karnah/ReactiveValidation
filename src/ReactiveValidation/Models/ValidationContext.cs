using System.Collections.Generic;

namespace ReactiveValidation
{
    public class ValidationContext <TObject, TProp>
        where TObject : IValidatableObject
    {
        private readonly Dictionary<string, IStringSource> _messageArguments;

        public ValidationContext(TObject validatableObject, string propertyName, IStringSource displayPropertySource, TProp propertyValue)
        {
            ValidatableObject = validatableObject;
            PropertyName = propertyName;
            DisplayPropertySource = displayPropertySource;
            PropertyValue = propertyValue;

            _messageArguments = new Dictionary<string, IStringSource> {
                { nameof(PropertyName), DisplayPropertySource ?? new StaticStringSource(propertyName) }
            };
        }

        public ValidationContext(ValidationContext<TObject, TProp> parentContext)
            : this(parentContext.ValidatableObject, parentContext.PropertyName, parentContext.DisplayPropertySource, parentContext.PropertyValue)
        { }


        public TObject ValidatableObject { get; }

        public string PropertyName { get; }

        public IStringSource DisplayPropertySource { get; }

        public TProp PropertyValue { get; }



        public TParam GetParamValue<TParam>(ParameterInfo<TObject, TParam> parameterInfo)
        {
            var value = parameterInfo.FuncValue.Invoke(ValidatableObject);
            return value;
        }

        public void RegisterMessageArgument<TParam>(string placeholderName, ParameterInfo<TObject, TParam> parameterInfo, TParam paramValue)
        {
            if (parameterInfo?.DisplayNameSource != null) {
                _messageArguments.Add(placeholderName, parameterInfo.DisplayNameSource);
            }
            else {
                var stringSource = new StaticStringSource(parameterInfo?.Name ?? paramValue?.ToString());
                _messageArguments.Add(placeholderName, stringSource);
            }
        }

        public IStringSource GetMessageSource(IStringSource stringSource)
        {
            return new ValidationMessageStringSource(stringSource, _messageArguments);
        }
    }
}
