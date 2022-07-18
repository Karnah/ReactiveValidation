using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ReactiveValidation.Resources.StringSources;
using ReactiveValidation.Validators;

namespace ReactiveValidation
{
    /// <summary>
    /// Info for validatable property.
    /// </summary>
    internal class ValidatableProperty<TObject>
        where TObject : IValidatableObject
    {
        /// <summary>
        /// Create new class of info for validatable property.
        /// </summary>
        /// <param name="propertyName">Name of property.</param>
        /// <param name="displayNameSource">Source of display name.</param>
        /// <param name="propertyCascadeMode">Property cascade mode.</param>
        /// <param name="validators">List of all property validators.</param>
        public ValidatableProperty(string propertyName, IStringSource? displayNameSource, CascadeMode propertyCascadeMode, IReadOnlyList<IPropertyValidator<TObject>> validators)
        {
            PropertyName = propertyName;
            DisplayNameSource = displayNameSource;
            RelatedProperties = new HashSet<string>(validators.SelectMany(v => v.RelatedProperties));
            Validators = validators;
            PropertyCascadeMode = propertyCascadeMode;
            AsyncValidatorCancellationTokenSources = validators
                .SkipWhile(v => !v.IsAsync)
                .ToDictionary(v => v, _ => (CancellationTokenSource?) null);
            ValidatorsValidationMessages = Validators.ToDictionary(v => v, _ => (IReadOnlyList<ValidationMessage>) Array.Empty<ValidationMessage>());
        }


        /// <summary>
        /// Name of property.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Source of display name.
        /// </summary>
        public IStringSource? DisplayNameSource { get; }

        /// <summary>
        /// Specifies how rules of property should cascade when one fails.
        /// </summary>
        public CascadeMode PropertyCascadeMode { get; }
        
        /// <summary>
        /// List of properties which affect on validation result of this property. 
        /// </summary>
        public HashSet<string> RelatedProperties { get; }

        /// <summary>
        /// List of all property validators.
        /// </summary>
        public IReadOnlyList<IPropertyValidator<TObject>> Validators { get; }

        /// <summary>
        /// List of <see cref="CancellationTokenSource" /> for all validators which should validate async.
        /// </summary>
        public Dictionary<IPropertyValidator<TObject>, CancellationTokenSource?> AsyncValidatorCancellationTokenSources { get; }

        /// <summary>
        /// List of property validators and its current validation messages.
        /// </summary>
        public IDictionary<IPropertyValidator<TObject>, IReadOnlyList<ValidationMessage>> ValidatorsValidationMessages { get; }

        /// <summary>
        /// Validation messages of property.
        /// </summary>
        public IReadOnlyList<ValidationMessage> ValidationMessages => ValidatorsValidationMessages.SelectMany(vm => vm.Value).ToList();
    }
}
