using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ReactiveValidation
{
    /// <summary>
    /// Information about observing property.
    /// </summary>
    internal class ObservingProperty
    {
        /// <summary>
        /// Create instance of <see cref="ObservingProperty" /> class.
        /// </summary>
        public ObservingProperty(string propertyName, ObservingPropertySettings settings)
        {
            PropertyName = propertyName;
            Settings = settings;
        }

        /// <summary>
        /// Name of property.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Observing property settings.
        /// </summary>
        public ObservingPropertySettings Settings { get; }
        
        /// <summary>
        /// Previous value of property.
        /// </summary>
        public object? PreviousValue { get; set; }

        /// <summary>
        /// Action for tracking property or collection items.
        /// </summary>
        public PropertyChangedEventHandler? ValueChangedAction { get; set; }

        /// <summary>
        /// Action for tracking property or collection items errors.
        /// </summary>
        public EventHandler<DataErrorsChangedEventArgs>? ErrorsChangedAction { get; set; }

        /// <summary>
        /// Action for tracking collection.
        /// </summary>
        public NotifyCollectionChangedEventHandler? CollectionChangedAction { get; set; }
    }
}
