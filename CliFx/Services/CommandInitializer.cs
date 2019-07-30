﻿using System;
using System.Collections.Generic;
using System.Linq;
using CliFx.Exceptions;
using CliFx.Internal;
using CliFx.Models;

namespace CliFx.Services
{
    public class CommandInitializer : ICommandInitializer
    {
        private readonly ICommandOptionInputConverter _commandOptionInputConverter;

        public CommandInitializer(ICommandOptionInputConverter commandOptionInputConverter)
        {
            _commandOptionInputConverter = commandOptionInputConverter;
        }

        public CommandInitializer()
            : this(new CommandOptionInputConverter())
        {
        }

        public void InitializeCommand(ICommand command, CommandSchema schema, CommandInput input)
        {
            // Set command options
            var isGroupNameDetected = false;
            var groupName = default(string);
            var properties = new HashSet<CommandOptionSchema>();
            foreach (var option in input.Options)
            {
                var optionSchema = schema.Options.FindByAlias(option.Alias);

                if (optionSchema == null)
                    continue;

                if (isGroupNameDetected && !string.Equals(groupName, optionSchema.GroupName, StringComparison.OrdinalIgnoreCase))
                    continue;

                if (!isGroupNameDetected)
                {
                    groupName = optionSchema.GroupName;
                    isGroupNameDetected = true;
                }

                var convertedValue = _commandOptionInputConverter.ConvertOption(option, optionSchema.Property.PropertyType);
                optionSchema.Property.SetValue(command, convertedValue);

                properties.Add(optionSchema);
            }

            var unsetRequiredOptions = schema.Options
                .Except(properties)
                .Where(p => p.IsRequired)
                .Where(p => string.Equals(p.GroupName, groupName, StringComparison.OrdinalIgnoreCase))
                .ToArray();

            if (unsetRequiredOptions.Any())
                throw new MissingCommandOptionException(
                    $"Can't resolve command because one or more required properties were not set: {unsetRequiredOptions.Select(p => p.Name).JoinToString(", ")}");
        }
    }
}