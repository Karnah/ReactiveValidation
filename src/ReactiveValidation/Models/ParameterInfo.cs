using System;

namespace ReactiveValidation
{
    public class ParameterInfo<TObject, TParam>
        where TObject : IValidatableObject
    {
        public ParameterInfo(string name, IStringSource displayNameSource, Func<TObject, TParam> funcValue)
        {
            Name = name;
            DisplayNameSource = displayNameSource;
            FuncValue = funcValue;
        }


        public string Name { get; }

        public IStringSource DisplayNameSource { get; }

        public Func<TObject, TParam> FuncValue { get; }
    }
}
