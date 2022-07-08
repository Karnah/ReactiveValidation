using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ReactiveValidation.ObjectObserver
{
    /// <summary>
    /// Settings of observing property.
    /// </summary>
    internal class ObservingPropertySettings
    {
        /// <summary>
        /// <see langword="true" />, if property inherit <see cref="INotifyPropertyChanged" /> and should be revalidated on <see cref="INotifyPropertyChanged.PropertyChanged" /> event.
        /// </summary>
        public bool TrackValueChanged { get; set; }

        /// <summary>
        /// <see langword="true" />, if property inherit <see cref="IValidatableObject" /> and should be revalidated on <see cref="INotifyDataErrorInfo.ErrorsChanged" /> event.
        /// </summary>
        public bool TrackValueErrorsChanged { get; set; }

        /// <summary>
        /// <see langword="true" />, if property inherit <see cref="INotifyCollectionChanged" /> and should be revalidated on <see cref="INotifyCollectionChanged.CollectionChanged" /> event.
        /// </summary>
        public bool TrackCollectionChanged { get; set; }

        /// <summary>
        /// <see langword="true" />, if property inherit <see cref="INotifyCollectionChanged" />, its items inherit <see cref="INotifyPropertyChanged" />
        /// and property should be revalidated on item's <see cref="INotifyPropertyChanged.PropertyChanged" /> event.
        /// </summary>
        public bool TrackCollectionItemChanged { get; set; }

        /// <summary>
        /// <see langword="true" />, if property inherit <see cref="INotifyCollectionChanged" />, its items inherit <see cref="IValidatableObject" />
        /// and property should be revalidated on item's <see cref="INotifyDataErrorInfo.ErrorsChanged" /> event.
        /// </summary>
        public bool TrackCollectionItemErrorsChanged { get; set; }

        /// <summary>
        /// If property inherit <see cref="IValidatableObject" /> - this method allows create <see cref="IObjectValidator" /> for property value.
        /// </summary>
        public Func<IValidatableObject, IObjectValidator>? PropertyValueFactoryMethod { get; set; }

        /// <summary>
        /// If property is collection and its items inherit <see cref="IValidatableObject" /> - this method allows create <see cref="IObjectValidator" /> for collection items.
        /// </summary>
        public Func<IValidatableObject, IObjectValidator>? CollectionItemFactoryMethod { get; set; }

        /// <summary>
        /// <see langword="false" /> if no settings are used.
        /// </summary>
        public bool IsDefaultSettings =>
            TrackValueChanged == false &&
            TrackValueErrorsChanged == false &&
            TrackCollectionChanged == false &&
            TrackCollectionItemChanged == false &&
            TrackCollectionItemErrorsChanged == false &&
            PropertyValueFactoryMethod == null &&
            CollectionItemFactoryMethod == null;
    }
}
