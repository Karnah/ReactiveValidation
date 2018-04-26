using System.Collections.Generic;

namespace ReactiveValidation
{
    public interface IObjectValidator
    {
        bool IsValid { get; }

        bool HasWarnings { get; }

        IEnumerable<ValidationMessage> ValidationMessages { get; }


        IEnumerable<ValidationMessage> GetMessages(string propertyName);

        void Revalidate();
    }
}
