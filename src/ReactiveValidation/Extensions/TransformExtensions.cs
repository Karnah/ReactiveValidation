using System;
using System.Linq.Expressions;
using ReactiveValidation.Validators.PropertyValueTransformers;

namespace ReactiveValidation.Extensions
{
    /// <summary>
    /// Extensions for creation validators which check transformed value. 
    /// </summary>
    public static class TransformExtensions
    {
        /// <summary>
        /// Create validator with transforming func.
        /// </summary>
        /// <param name="builder">The validation builder.</param>
        /// <param name="property">The validatable property.</param>
        /// <param name="transformer">Transforming func.</param>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The original type of validatable property.</typeparam>
        /// <typeparam name="TPropTransformed">The target type of validatable property.</typeparam>
        /// <returns>Single property validator for <typeparamref name="TPropTransformed" /> type.</returns>
        /// <exception cref="ArgumentNullException">If <see cref="builder" /> or <see cref="property" /> or <see cref="transformer" /> is null.</exception>
        public static ISinglePropertyRuleBuilderInitial<TObject, TPropTransformed> Transform<TObject, TProp, TPropTransformed>(
            this IValidationBuilder<TObject> builder,
            Expression<Func<TObject, TProp>> property,
            Func<TObject, TProp, TPropTransformed> transformer)
                where TObject : IValidatableObject
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            if (transformer == null)
                throw new ArgumentNullException(nameof(transformer));
            
            return builder.Transform(property, new FuncValueTransformer<TObject, TProp, TPropTransformed>(transformer));
        }
        
        /// <summary>
        /// Create validator with transforming func.
        /// </summary>
        /// <param name="builder">The validation builder.</param>
        /// <param name="property">The validatable property.</param>
        /// <param name="transformer">Transforming func.</param>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The original type of validatable property.</typeparam>
        /// <typeparam name="TPropTransformed">The target type of validatable property.</typeparam>
        /// <returns>Single property validator for <typeparamref name="TPropTransformed" /> type.</returns>
        /// <exception cref="ArgumentNullException">If <see cref="builder" /> or <see cref="property" /> or <see cref="transformer" /> is null.</exception>
        public static ISinglePropertyRuleBuilderInitial<TObject, TPropTransformed> Transform<TObject, TProp, TPropTransformed>(
            this IValidationBuilder<TObject> builder,
            Expression<Func<TObject, TProp>> property,
            Func<TProp, TPropTransformed> transformer)
            where TObject : IValidatableObject
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            if (transformer == null)
                throw new ArgumentNullException(nameof(transformer));
            
            return builder.Transform(property, new FuncValueTransformer<TObject, TProp, TPropTransformed>((_, prop) => transformer.Invoke(prop)));
        }

        #region Tranform from string to numeric types

        /// <summary>
        /// Create validator for <see cref="short" /> type which transforming from <see cref="string" />. 
        /// </summary>
        /// <param name="builder">The validation builder.</param>
        /// <param name="property">The validatable property.</param>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <returns>Single property validator for <see cref="Nullable{Short}"/> type.</returns>
        public static ISinglePropertyRuleBuilderInitial<TObject, short?> TransformToShort<TObject>(
            this IValidationBuilder<TObject> builder,
            Expression<Func<TObject, string>> property)
            where TObject : IValidatableObject
        {
            return builder.Transform(property, s => short.TryParse(s, out var num) ? num : (short?)null);
        }
        
        /// <summary>
        /// Create validator for <see cref="ushort" /> type which transforming from <see cref="string" />. 
        /// </summary>
        /// <param name="builder">The validation builder.</param>
        /// <param name="property">The validatable property.</param>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <returns>Single property validator for <see cref="Nullable{UShort}"/> type.</returns>
        public static ISinglePropertyRuleBuilderInitial<TObject, ushort?> TransformToUShort<TObject>(
            this IValidationBuilder<TObject> builder,
            Expression<Func<TObject, string>> property)
            where TObject : IValidatableObject
        {
            return builder.Transform(property, s => ushort.TryParse(s, out var num) ? num : (ushort?)null);
        }
        
        /// <summary>
        /// Create validator for <see cref="int" /> type which transforming from <see cref="string" />. 
        /// </summary>
        /// <param name="builder">The validation builder.</param>
        /// <param name="property">The validatable property.</param>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <returns>Single property validator for <see cref="Nullable{Int}"/> type.</returns>
        public static ISinglePropertyRuleBuilderInitial<TObject, int?> TransformToInt<TObject>(
            this IValidationBuilder<TObject> builder,
            Expression<Func<TObject, string>> property)
                where TObject : IValidatableObject
        {
            return builder.Transform(property, s => int.TryParse(s, out var num) ? num : (int?)null);
        }

        /// <summary>
        /// Create validator for <see cref="uint" /> type which transforming from <see cref="string" />. 
        /// </summary>
        /// <param name="builder">The validation builder.</param>
        /// <param name="property">The validatable property.</param>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <returns>Single property validator for <see cref="Nullable{UInt}"/> type.</returns>
        public static ISinglePropertyRuleBuilderInitial<TObject, uint?> TransformToUInt<TObject>(
            this IValidationBuilder<TObject> builder,
            Expression<Func<TObject, string>> property)
            where TObject : IValidatableObject
        {
            return builder.Transform(property, s => uint.TryParse(s, out var num) ? num : (uint?)null);
        }
        
        /// <summary>
        /// Create validator for <see cref="long" /> type which transforming from <see cref="string" />. 
        /// </summary>
        /// <param name="builder">The validation builder.</param>
        /// <param name="property">The validatable property.</param>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <returns>Single property validator for <see cref="Nullable{Long}"/> type.</returns>
        public static ISinglePropertyRuleBuilderInitial<TObject, long?> TransformToLong<TObject>(
            this IValidationBuilder<TObject> builder,
            Expression<Func<TObject, string>> property)
            where TObject : IValidatableObject
        {
            return builder.Transform(property, s => long.TryParse(s, out var num) ? num : (long?)null);
        }
        
        /// <summary>
        /// Create validator for <see cref="ulong" /> type which transforming from <see cref="string" />. 
        /// </summary>
        /// <param name="builder">The validation builder.</param>
        /// <param name="property">The validatable property.</param>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <returns>Single property validator for <see cref="Nullable{Ulong}"/> type.</returns>
        public static ISinglePropertyRuleBuilderInitial<TObject, ulong?> TransformToULong<TObject>(
            this IValidationBuilder<TObject> builder,
            Expression<Func<TObject, string>> property)
            where TObject : IValidatableObject
        {
            return builder.Transform(property, s => ulong.TryParse(s, out var num) ? num : (ulong?)null);
        }
        
        /// <summary>
        /// Create validator for <see cref="float" /> type which transforming from <see cref="string" />. 
        /// </summary>
        /// <param name="builder">The validation builder.</param>
        /// <param name="property">The validatable property.</param>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <returns>Single property validator for <see cref="Nullable{Float}"/> type.</returns>
        public static ISinglePropertyRuleBuilderInitial<TObject, float?> TransformToFloat<TObject>(
            this IValidationBuilder<TObject> builder,
            Expression<Func<TObject, string>> property)
            where TObject : IValidatableObject
        {
            return builder.Transform(property, s => float.TryParse(s, out var num) ? num : (float?)null);
        }
        
        /// <summary>
        /// Create validator for <see cref="double" /> type which transforming from <see cref="string" />. 
        /// </summary>
        /// <param name="builder">The validation builder.</param>
        /// <param name="property">The validatable property.</param>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <returns>Single property validator for <see cref="Nullable{Double}"/> type.</returns>
        public static ISinglePropertyRuleBuilderInitial<TObject, double?> TransformToDouble<TObject>(
            this IValidationBuilder<TObject> builder,
            Expression<Func<TObject, string>> property)
            where TObject : IValidatableObject
        {
            return builder.Transform(property, s => double.TryParse(s, out var num) ? num : (double?)null);
        }
        
        /// <summary>
        /// Create validator for <see cref="decimal" /> type which transforming from <see cref="string" />. 
        /// </summary>
        /// <param name="builder">The validation builder.</param>
        /// <param name="property">The validatable property.</param>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <returns>Single property validator for <see cref="Nullable{Decimal}"/> type.</returns>
        public static ISinglePropertyRuleBuilderInitial<TObject, decimal?> TransformToDecimal<TObject>(
            this IValidationBuilder<TObject> builder,
            Expression<Func<TObject, string>> property)
            where TObject : IValidatableObject
        {
            return builder.Transform(property, s => decimal.TryParse(s, out var num) ? num : (decimal?)null);
        }
        
        #endregion
    }
}