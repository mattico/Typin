﻿namespace Typin.Exceptions.Resolvers.ParameterResolver
{
    using System;
    using System.Collections.Generic;
    using Typin.Models.Binding;
    using Typin.Models.Schemas;
    using Typin.Utilities.Extensions;

    /// <summary>
    /// Parameter duplicate by order exception.
    /// </summary>
    public sealed class ParameterDuplicateByOrderException : ParameterResolverException
    {
        /// <summary>
        /// Duplicated parameter order.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="ParameterDuplicateByOrderException"/>.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="order"></param>
        /// <param name="invalidParameters"></param>
        public ParameterDuplicateByOrderException(IModelSchema command, int order, IReadOnlyList<IParameterSchema> invalidParameters) :
            base(command,
                 invalidParameters,
                 BuildMessage(command, order, invalidParameters))
        {
            Order = order;
        }

        private static string BuildMessage(IModelSchema command, int order, IReadOnlyList<IParameterSchema> invalidParameters)
        {
            return $"Command '{command.Type.FullName}' is invalid because it contains {invalidParameters.Count} parameters with the same order ('{order}'):{Environment.NewLine}" +
                   $"{invalidParameters.JoinToString(Environment.NewLine)}{Environment.NewLine}" +
                   Environment.NewLine +
                   "Parameters must have unique order.";
        }
    }
}