namespace ReactiveValidation.Validators.PropertyValueTransformers
{
    /// <summary>
    /// Interface for classes which allow transform value from one type to another. 
    /// </summary>
    /// <typeparam name="TObject">Type of object which store this value.</typeparam>
    /// <typeparam name="TTo">Target type of property.</typeparam>
    public interface IValueTransformer<in TObject, out TTo>
    {
        /// <summary>
        /// Transform value from one type to another.
        /// </summary>
        /// <param name="obj">Object which store value.</param>
        /// <param name="from">Source value.</param>
        /// <returns>Transformed value.</returns>
        TTo Transform(TObject obj, object? from);
    }
}