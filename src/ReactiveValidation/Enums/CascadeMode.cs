namespace ReactiveValidation;

/// <summary>
/// Specifies how rules should cascade when one fails.
/// </summary>
public enum CascadeMode
{
    /// <summary>
    /// When a rule/validator fails, execution continues to the next rule/validator.
    /// </summary>
    Continue = 0,
    
    /// <summary>
    /// When a rule/validator fails, validation is stopped for the current rule/validator.
    /// </summary>
    Stop = 1
}