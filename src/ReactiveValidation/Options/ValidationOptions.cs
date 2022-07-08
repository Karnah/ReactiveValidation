using System.Collections.Generic;

using ReactiveValidation.Exceptions;
using ReactiveValidation.Internal;
using ReactiveValidation.ValidatorFactory;

namespace ReactiveValidation
{
    /// <summary>
    /// Options for validation.
    /// </summary>
    public static class ValidationOptions
    {
        private static bool _isConfigured;

        /// <summary>
        /// Manager, which allows use different localization for validation messages.
        /// </summary>
        public static LanguageManager LanguageManager { get; } = new LanguageManager();

        /// <summary>
        /// Class for resolving display name of properties by its metadata.
        /// </summary>
        public static IDisplayNameResolver DisplayNameResolver { get; internal set; } = new DefaultDisplayNameResolver();

        /// <summary>
        /// Factory for creating validator for object.
        /// </summary>
        public static IValidatorFactory ValidatorFactory { get; internal set; } = new DefaultValidatorFactory();

        /// <summary>
        /// Setup validation options.
        /// </summary>
        /// <exception cref="MethodAlreadyCalledException">Throws if options already was configured.</exception>
        public static ValidationOptionsBuilder Setup()
        {
            if (_isConfigured)
                throw new MethodAlreadyCalledException("Cannot setup options twice");

            _isConfigured = true;
            return new ValidationOptionsBuilder();
        }
    }
}