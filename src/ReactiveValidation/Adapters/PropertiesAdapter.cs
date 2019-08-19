using System.Collections.Generic;
using ReactiveValidation.Validators;

namespace ReactiveValidation.Adapters
{
    /// <summary>
    /// Adapter for which checks several properties at once.
    /// </summary>
    internal class PropertiesAdapter<TObject> : BasePropertiesAdapter<TObject, object>
        where TObject : IValidatableObject
    {
        /// <summary>
        /// List of names of target properties.
        /// </summary>
        private readonly SortedSet<string> _propertiesNames;

        public PropertiesAdapter(
            ObjectValidator<TObject> objectValidator,
            IReadOnlyCollection<IPropertyValidator<TObject, object>> propertyValidators,
            IEnumerable<string> propertiesNames)
            : base(objectValidator, propertyValidators)
        {
            _propertiesNames = new SortedSet<string>(propertiesNames);
        }


        /// <inheritdoc />
        public override void Revalidate()
        {
            foreach (var propertiesName in _propertiesNames) {
                RevalidateProperty(propertiesName);
            }
        }


        /// <inheritdoc />
        protected override bool IsTargetProperty(string propertyName)
        {
            return _propertiesNames.Contains(propertyName);
        }

        /// <inheritdoc />
        protected override void RevalidateProperty(string propertyName)
        {
            InnerRevalidateProperty(propertyName);
        }
    }
}
