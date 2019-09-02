using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ReactiveValidation
{
    /// <summary>
    /// Information about observing property.
    /// </summary>
    internal class ObservingPropertyInfo
    {
        /// <summary>
        /// Name of property.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Previous value of property.
        /// </summary>
        public object PreviousValue { get; set; }

        /// <summary>
        /// Method for creating object validator for instance.
        /// </summary>
        public Func<IValidatableObject, IObjectValidator> FactoryMethod { get; set; }

        /// <summary>
        /// Action for tracking property or collection items.
        /// </summary>
        public EventHandler<DataErrorsChangedEventArgs> ErrorsChangedAction { get; set; }

        /// <summary>
        /// Action for tracking collection.
        /// </summary>
        public NotifyCollectionChangedEventHandler CollectionChangedAction { get; set; }
    }
}
