using System;
using System.Threading;
using System.Threading.Tasks;
using ReactiveValidation.Helpers;
using ReactiveValidation.Resources.StringSources;

namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Validator which check property value by async predicate.
    /// </summary>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <typeparam name="TProp">The type of validatable property.</typeparam>
    public class AsyncPredicateValidator<TObject, TProp> : BaseAsyncPropertyValidator<TObject, TProp>
        where TObject : IValidatableObject
    {
        private readonly Func<ValidationContext<TObject, TProp>, CancellationToken, Task<bool>> _predicate;

        /// <summary>
        /// Initialize a new instance of <see cref="AsyncPredicateValidator{TObject,TProp}" /> class.
        /// </summary>
        /// <param name="predicate">Async predicate which check property value.</param>
        /// <param name="validationMessageType">The type validatable message.</param>
        public AsyncPredicateValidator(
            Func<ValidationContext<TObject, TProp>, CancellationToken, Task<bool>> predicate,
            ValidationMessageType validationMessageType
            ) : base(new LanguageStringSource(ValidatorsNames.PredicateValidator), validationMessageType)
        {
            _predicate = predicate;
        }

        /// <inheritdoc />
        protected override async Task<bool> IsValidAsync(ValidationContext<TObject, TProp> context,
            CancellationToken cancellationToken)
        {
            return await _predicate
                .Invoke(context, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}