﻿namespace Typin.Internal.Schemas
{
    using System;
    using System.Reflection;
    using Typin.Attributes;
    using Typin.Binding;
    using Typin.Internal.Exceptions;
    using Typin.Internal.Extensions;
    using Typin.Schemas;
    using Typin.Utilities;

    /// <summary>
    /// Resolves an instance of <see cref="CommandParameterSchema"/>.
    /// </summary>
    internal static class CommandParameterSchemaResolver
    {
        /// <summary>
        /// Resolves <see cref="CommandParameterSchema"/>.
        /// </summary>
        public static CommandParameterSchema? TryResolve(PropertyInfo property)
        {
            CommandParameterAttribute? attribute = property.GetCustomAttribute<CommandParameterAttribute>();
            if (attribute is null)
            {
                return null;
            }

            string name = attribute.HasAutoGeneratedName ? TextUtils.ToKebabCase(property.Name) : attribute.Name!;

            if (attribute.Converter is Type converterType && !converterType.Implements(typeof(IBindingConverter)))
            {
                throw AttributesExceptions.InvalidConverterType(converterType);
            }

            return new CommandParameterSchema(
                property,
                attribute.Order,
                name,
                attribute.Description,
                attribute.Converter
            );
        }
    }
}