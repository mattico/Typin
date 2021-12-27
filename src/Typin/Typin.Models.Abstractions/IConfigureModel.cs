﻿namespace Typin.Models
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Models.Builders;

    /// <summary>
    /// Represents an object that configures a model.
    /// </summary>
    public interface IConfigureModel
    {
        /// <summary>
        /// Checks whether type is a valid model configurator.
        /// </summary>
        public static bool IsValidType(Type type)
        {
            Type[] interfaces = type.GetInterfaces();

            return interfaces.Contains(typeof(IConfigureModel)) &&
                interfaces.Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IConfigureModel<>)) &&
                !type.IsAbstract &&
                !type.IsInterface;
        }

        /// <summary>
        /// Checks whether type is a valid model configurator.
        /// </summary>
        public static bool IsValidType(Type type, Type commandType)
        {
            return type.GetInterfaces().Contains(typeof(IConfigureModel<>).MakeGenericType(commandType)) &&
                !type.IsAbstract &&
                !type.IsInterface;
        }
    }

    /// <summary>
    /// Represents an object that configures a model <typeparamref name="TModel"/>.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IConfigureModel<TModel> : IConfigureModel
        where TModel : class, IModel
    {
        /// <summary>
        /// Configure model using a <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask ConfigureAsync(IModelBuilder<TModel> builder, CancellationToken cancellationToken);

        /// <summary>
        /// Checks whether type is a valid model configurator.
        /// </summary>
        public static new bool IsValidType(Type type)
        {
            return type.GetInterfaces().Contains(typeof(IConfigureModel<TModel>)) &&
                !type.IsAbstract &&
                !type.IsInterface;
        }
    }
}
