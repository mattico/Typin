﻿namespace Typin.Directives.Attributes
{
    using System;

    /// <summary>
    /// Annotates a type that defines a directive.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class DirectiveAttribute : Attribute
    {
        /// <summary>
        /// Directive name.
        /// All directives in an application must have different names and aliases.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Directive alias.
        /// All directives in an application must have different names and aliases.
        /// </summary>
        public string? Alias { get; init; }

        /// <summary>
        /// Directive description, which is used in help text.
        /// </summary>
        public string? Description { get; init; }

        ///// <summary>
        ///// List of CLI mode types, in which the directive can be executed.
        ///// If null (default) or empty, directive can be executed in every registered mode in the app.
        ///// </summary>
        //public Type[]? SupportedModes { get; init; }

        ///// <summary>
        ///// List of CLI mode types, in which the directive cannot be executed.
        ///// If null (default) or empty, directive can be executed in every registered mode in the app.
        ///// </summary>
        //public Type[]? ExcludedModes { get; init; }

        /// <summary>
        /// Initializes an instance of <see cref="DirectiveAttribute"/>.
        /// </summary>
        public DirectiveAttribute()
        {
            Name = string.Empty;
        }

        /// <summary>
        /// Initializes an instance of <see cref="DirectiveAttribute"/>.
        /// </summary>
        public DirectiveAttribute(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}