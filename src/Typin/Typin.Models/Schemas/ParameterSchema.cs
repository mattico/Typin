﻿namespace Typin.Models.Schemas
{
    using System;
    using System.Reflection;
    using Typin.Schemas.Collections;

    /// <summary>
    /// Stores command parameter schema.
    /// </summary>
    public class ParameterSchema : ArgumentSchema, IParameterSchema
    {
        /// <inheritdoc/>
        public int Order { get; }

        /// <inheritdoc/>
        public new string Name => base.Name!;

        /// <summary>
        /// Initializes an instance of <see cref="ParameterSchema"/> that represents a property-based parameter.
        /// </summary>
        public ParameterSchema(PropertyInfo property,
                               int order,
                               string name,
                               string? description,
                               Type? converter,
                               IExtensionsCollection extensions)
            : base(property, name, description, converter, extensions)
        {
            Order = order;
        }

        /// <summary>
        /// Initializes an instance of <see cref="ParameterSchema"/> that represents a dynamic parameter.
        /// </summary>
        public ParameterSchema(Type propertyType,
                               string propertyName,
                               int order,
                               string name,
                               string? description,
                               Type? converter,
                               IExtensionsCollection extensions)
            : base(propertyType, propertyName, name, description, converter, extensions)
        {
            Order = order;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{Bindable.Name} ([{Order}] <{Name}>)";
        }
    }
}