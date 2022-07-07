using System;

namespace ReactiveValidation.Exceptions;

/// <summary>
/// Exception thrown when using settings which is not configured.
/// </summary>
public class ValidationSettingsException : Exception
{
    /// <summary>
    /// Create instance of <see cref="ValidationSettingsException" /> class.
    /// </summary>
    public ValidationSettingsException(string message) : base(message)
    {
    }
}