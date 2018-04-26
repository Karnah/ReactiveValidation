using System;

namespace ReactiveValidation
{
    public class ParameterInfo<TObject, TParam>
        where TObject : IValidatableObject
    {
        private readonly IStringSource _displayNameSource;

        public ParameterInfo(string name, IStringSource displayNameSource, Func<TObject, TParam> funcValue)
        {
            Name = name;
            _displayNameSource = displayNameSource;
            FuncValue = funcValue;
        }


        public string Name { get; }

        public string DisplayName => _displayNameSource?.GetString();

        public Func<TObject, TParam> FuncValue { get; }
    }
}
