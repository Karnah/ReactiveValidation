﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReactiveValidation.Resources.StringSources
{
    /// <summary>
    /// String source for validation message.
    /// </summary>
    public class ValidationMessageStringSource : IStringSource
    {
        private readonly IStringSource _patternStringSource;
        private readonly Dictionary<string, IStringSource> _arguments;

        /// <summary>
        /// Create new instance of validation message source.
        /// </summary>
        /// <param name="patternStringSource">Source of message patter.</param>
        /// <param name="arguments">Sources of message arguments.</param>
        public ValidationMessageStringSource(IStringSource patternStringSource, Dictionary<string, IStringSource> arguments)
        {
            _patternStringSource = patternStringSource ?? throw new ArgumentNullException(nameof(patternStringSource));
            _arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
        }

        /// <inheritdoc />
        public string GetString()
        {
            var messagePattern = new StringBuilder(_patternStringSource.GetString());

            foreach (var messageArgument in _arguments)
            {
                messagePattern = messagePattern.Replace($"{{{messageArgument.Key}}}", messageArgument.Value.GetString());
            }

            return messagePattern.ToString();
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ValidationMessageStringSource) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (_patternStringSource.GetHashCode() * 397) ^ (_arguments.GetHashCode());
            }
        }

        /// <summary>
        /// Check if two sources are equal.
        /// </summary>
        protected bool Equals(ValidationMessageStringSource other)
        {
            return Equals(_patternStringSource, other._patternStringSource) &&
                    _arguments.SequenceEqual(other._arguments);
        }
    }
}
