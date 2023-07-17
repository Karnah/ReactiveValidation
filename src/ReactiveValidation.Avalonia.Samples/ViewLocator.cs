using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace ReactiveValidation.Avalonia.Samples
{
    public class ViewLocator : IDataTemplate
    {
        /// <inheritdoc />
        public Control Build(object? data)
        {
            var name = data?.GetType().FullName!.Replace("ViewModel", "View");
            if (name != null)
            {
                var type = Type.GetType(name);
                if (type != null)
                {
                    return (Control)Activator.CreateInstance(type)!;
                }
            }

            return new TextBlock { Text = "Not Found: " + name };
        }

        /// <inheritdoc />
        public bool Match(object? data)
        {
            return data is ReactiveValidatableObject;
        }
    }
}