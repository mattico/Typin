﻿namespace Typin.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Typin.Internal.Extensions;

    /// <summary>
    /// Represents a bindable <see cref="PropertyInfo"/>.
    /// </summary>
    public sealed class BindablePropertyInfo
    {
        /// <summary>
        /// Property info may be null for built-in arguments (help and version options)
        /// </summary>
        public PropertyInfo? Property { get; }

        /// <summary>
        /// Whether command argument is scalar.
        /// </summary>
        public bool IsScalar
        {
            get
            {
                _isScalar ??= Property.TryGetEnumerableArgumentUnderlyingType() is null;

                return _isScalar.Value;
            }
        }

        /// <summary>
        /// Initializes an instance of <see cref="BindablePropertyInfo"/>.
        /// </summary>
        internal BindablePropertyInfo(PropertyInfo? property)
        {
            Property = property;
        }

        private bool? _isScalar;

        private IReadOnlyList<string>? _validValues;

        /// <summary>
        /// Returns a list of valid values.
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<string> GetValidValues()
        {
            _validValues ??= InternalGetValidValues();

            return _validValues;

            IReadOnlyList<string> InternalGetValidValues()
            {
                if (Property is null)
                    return Array.Empty<string>();

                Type underlyingType = Property.PropertyType.TryGetNullableUnderlyingType() ??
                                      Property.PropertyType.TryGetEnumerableUnderlyingType() ??
                                      Property.PropertyType;

                // Enum
                if (underlyingType.IsEnum)
                    return Enum.GetNames(underlyingType);

                return Array.Empty<string>();
            }
        }

        /// <summary>
        /// Gets property value from command instance.
        /// When <see cref="Property"/> is null, returns null.
        /// </summary>
        /// <param name="commandInstance">Command instance.</param>
        /// <returns>Property value.</returns>
        public object? GetValue(ICommand commandInstance)
        {
            return Property?.GetValue(commandInstance);
        }

        /// <summary>
        /// Sets a property value in command instance.
        /// When <see cref="Property"/> is null, call is ignored (no operation - nop).
        /// </summary>
        /// <param name="commandInstance">Command instance.</param>
        /// <param name="value">Value to set.</param>
        public void SetValue(ICommand commandInstance, object? value)
        {
            Property?.SetValue(commandInstance, value);
        }

        /// <summary>
        /// Converts <see cref="BindablePropertyInfo"/> to <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="bindablePropertyInfo">Bindable property info to convert.</param>
        public static implicit operator PropertyInfo?(BindablePropertyInfo bindablePropertyInfo)
        {
            return bindablePropertyInfo.Property;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return Property?.Equals(obj) ?? false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return Property?.GetHashCode() ?? 0;
        }

        /// <inheritdoc/>
        public override string? ToString()
        {
            return Property?.ToString() ?? "<built-in argument>";
        }
    }
}