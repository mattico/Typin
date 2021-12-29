﻿namespace Typin.Internal.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Commands.Schemas;
    using Typin.Directives.Schemas;
    using Typin.Exceptions.Resolvers.CommandResolver;
    using Typin.Schemas;

    /// <summary>
    /// Resolves an instance of <see cref="RootSchema"/>.
    /// </summary>
    internal class RootSchemaResolver
    {
        private readonly IReadOnlyCollection<Type> _commandTypes;
        private readonly IReadOnlyCollection<Type> _dynamicCommandTypes;
        private readonly IReadOnlyCollection<Type> _directiveTypes;

        public ICommandSchema? DefaultCommand { get; private set; }
        public Dictionary<string, ICommandSchema>? Commands { get; private set; }
        public Dictionary<string, IDirectiveSchema>? Directives { get; private set; }

        /// <summary>
        /// Initializes an instance of <see cref="RootSchemaResolver"/>.
        /// </summary>
        public RootSchemaResolver(IReadOnlyCollection<Type> commandTypes,
                                  IReadOnlyCollection<Type> dynamicCommandTypes,
                                  IReadOnlyCollection<Type> directiveTypes)
        {
            _commandTypes = commandTypes;
            _dynamicCommandTypes = dynamicCommandTypes;
            _directiveTypes = directiveTypes;
        }

        /// <summary>
        /// Resolves the root schema.
        /// </summary>
        public RootSchema Resolve()
        {
            ResolveCommands(_commandTypes);
            ResolveDirectives(_directiveTypes);

            return new RootSchema(Directives!, Commands!, DefaultCommand);
        }

        private void ResolveCommands(IReadOnlyCollection<Type> commandTypes)
        {
            ICommandSchema? defaultCommand = null;
            Dictionary<string, ICommandSchema> commands = new();
            List<ICommandSchema> invalidCommands = new();

            //foreach (Type commandType in commandTypes)
            //{
            //    ICommandSchema command = CommandSchemaResolver.Resolve(commandType);

            //    if (command.IsDefault)
            //    {
            //        if (defaultCommand is null)
            //        {
            //            defaultCommand = command;
            //        }
            //        else
            //        {
            //            if (!invalidCommands.Contains(defaultCommand))
            //            {
            //                invalidCommands.Add(defaultCommand);
            //            }

            //            invalidCommands.Add(command);
            //        }
            //    }
            //    else if (!commands.TryAdd(command.Name!, command))
            //    {
            //        invalidCommands.Add(command);
            //    }
            //}

            //if (commands.Count == 0 && defaultCommand is null)
            //{
            //    defaultCommand = StubDefaultCommand.Schema;
            //}

            if (invalidCommands.Count > 0)
            {
                IGrouping<string, ICommandSchema> duplicateNameGroup = invalidCommands.Union(commands.Values)
                                                                                      .GroupBy(c => c.Name!, StringComparer.Ordinal)
                                                                                      .First();

                throw new CommandDuplicateByNameException(duplicateNameGroup.Key, duplicateNameGroup.ToArray());
            }

            Commands = commands;
            DefaultCommand = defaultCommand;
        }

        private void ResolveDirectives(IReadOnlyCollection<Type> directiveTypes)
        {
            Dictionary<string, IDirectiveSchema> directives = new();
            //List<IDirectiveSchema> invalidDirectives = new();

            //foreach (Type? directiveType in directiveTypes)
            //{
            //    IDirectiveSchema directive = DirectiveSchemaResolver.Resolve(directiveType);

            //    if (!directives.TryAdd(directive.Name, directive))
            //    {
            //        invalidDirectives.Add(directive);
            //    }
            //}

            //if (invalidDirectives.Count > 0)
            //{
            //    IGrouping<string, IDirectiveSchema> duplicateNameGroup = invalidDirectives.Union(directives.Values)
            //                                                                              .GroupBy(c => c.Name, StringComparer.Ordinal)
            //                                                                              .First();

            //    throw new DirectiveDuplicateByNameException(duplicateNameGroup.Key, duplicateNameGroup.ToArray());
            //}

            Directives = directives;
        }
    }
}