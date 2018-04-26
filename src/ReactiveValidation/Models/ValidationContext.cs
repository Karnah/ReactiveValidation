using System.Collections.Generic;
using System.Text;

namespace ReactiveValidation
{
    public class ValidationContext <TObject, TProp>
        where TObject : IValidatableObject
    {
        private readonly Dictionary<string, string> _messageArguments;

        public ValidationContext(TObject validatableObject, string propertyName, string displayPropertyName, TProp propertyValue)
        {
            ValidatableObject = validatableObject;
            PropertyName = propertyName;
            DisplayPropertyName = displayPropertyName;
            PropertyValue = propertyValue;

            _messageArguments = new Dictionary<string, string> {
                { nameof(PropertyName), string.IsNullOrEmpty(DisplayPropertyName) == true ? PropertyName : DisplayPropertyName }
            };
        }

        public ValidationContext(ValidationContext<TObject, TProp> parentContext)
            : this(parentContext.ValidatableObject, parentContext.PropertyName, parentContext.DisplayPropertyName, parentContext.PropertyValue)
        { }


        public TObject ValidatableObject { get; }

        public string PropertyName { get; }

        public string DisplayPropertyName { get; }

        public TProp PropertyValue { get; }



        public TParam GetParamValue<TParam>(ParameterInfo<TObject, TParam> parameterInfo)
        {
            var value = parameterInfo.FuncValue.Invoke(ValidatableObject);
            return value;
        }

        public void RegisterMessageArgument<TParam>(string placeholderName, ParameterInfo<TObject, TParam> parameterInfo, TParam paramValue)
        {
            var placeholderValue = parameterInfo?.DisplayName ?? parameterInfo?.Name;
            if (string.IsNullOrEmpty(placeholderValue) == false) {
                //todo need replace "{Placeholder}" on "filed {placeholderValue}"
            }
            else {
                placeholderValue = paramValue?.ToString();
            }

            //todo if placeholderValue is null or empty, what i must do?


            _messageArguments.Add(placeholderName, placeholderValue);
        }

        public string GetMessage(IStringSource stringSource)
        {
            var messagePattern = new StringBuilder(stringSource.GetString());

            foreach (var messageArgument in _messageArguments) {
                messagePattern = messagePattern.Replace($"{{{messageArgument.Key}}}", messageArgument.Value);
            }

            return messagePattern.ToString();
        }
    }
}
