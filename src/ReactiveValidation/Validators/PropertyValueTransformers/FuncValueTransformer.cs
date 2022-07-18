using System;

namespace ReactiveValidation.Validators.PropertyValueTransformers
{
    /// <summary>
    /// Value transformer based on <see cref="Func{TResult}" />. 
    /// </summary>
    /// <typeparam name="TObject">Type of object which store this value.</typeparam>
    /// <typeparam name="TFrom">Source type of property.</typeparam>
    /// <typeparam name="TTo">Target type of property.</typeparam>
    public class FuncValueTransformer<TObject, TFrom, TTo> : IValueTransformer<TObject, TTo>
    {
        private readonly Func<TObject, TFrom, TTo> _func;

        /// <summary>
        /// Create new instance of <see cref="FuncValueTransformer{TObject,TFrom,TTo}" /> class.
        /// </summary>
        /// <param name="func">Transforming func.</param>
        public FuncValueTransformer(Func<TObject, TFrom, TTo> func)
        {
            _func = func;
        }

        /// <inheritdoc />
        public TTo Transform(TObject obj, object? from)
        {
            return _func.Invoke(obj, (TFrom)from!);
        }
    }
}