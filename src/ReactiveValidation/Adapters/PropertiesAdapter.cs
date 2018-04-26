using System.Collections.Generic;
using ReactiveValidation.Validators;

namespace ReactiveValidation.Adapters
{
    internal class PropertiesAdapter<TObject> : BasePropertiesAdapter<TObject, object>
        where TObject : IValidatableObject
    {
        private readonly SortedSet<string> _propertiesNames;

        public PropertiesAdapter(
            ObjectValidator<TObject> objectValidator,
            IReadOnlyCollection<IPropertyValidator<TObject, object>> propertyValidators,
            IEnumerable<string> propertiesNames)
            : base(objectValidator, propertyValidators)
        {
            _propertiesNames = new SortedSet<string>(propertiesNames);
        }


        public override void Revalidate()
        {
            foreach (var propertiesName in _propertiesNames) {
                RevalidateProperty(propertiesName);
            }
        }


        protected override bool IsTargetProperty(string propertyName)
        {
            return _propertiesNames.Contains(propertyName);
        }

        protected override void RevalidateProperty(string propertyName)
        {
            InnerRevalidateProperty(propertyName);
        }
    }
}
