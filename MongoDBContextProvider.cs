﻿using MausWorks.MongoDB.Configuration;
using MausWorks.MongoDB.Exceptions;
using Microsoft.Framework.OptionsModel;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MausWorks.MongoDB
{
    /// <summary>
    /// A provider used for creating a <see cref="MongoDBContext"/>
    /// </summary>
    internal class MongoDBContextProvider<TContext> where TContext : MongoDBContext, new()
    {
        /// <summary>
        /// Gets or sets the name of the context.
        /// </summary>
        /// <value>
        /// The name of the context.
        /// </value>
        private string _contextName { get; set; }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        internal MongoDBContextConfiguration Configuration { get; private set; }
        
        internal MongoDBContextProvider(IOptions<MongoDBContextConfiguration<TContext>> options, IConfigureOptions<MongoDBContextConfiguration<TContext>> configure)
        {
            _contextName = typeof(TContext).Name;

            MongoDBContextConfiguration config;

            // We need to have a ConfigureOptions-object here, since the interface does not contain the "Action"-property
            // Intentional?
            var configOptions = (configure as ConfigureOptions<MongoDBContextConfiguration<TContext>>);

            if (configOptions != null)
            {
                config = options.GetNamedOptions(configOptions.Name);

                configOptions.Configure(options.Options, configOptions.Name);
            }
            else
            {
                config = options.Options;
            }

            Configuration = config;
        }

        /// <summary>
        /// Creates a valid <see cref="MongoDBContext"/> given that the provided <see cref="MongoDBContextConfiguration"/> is valid.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <param name="options">The options as provided by the <see cref="Microsoft.Framework.OptionsModel"/>.</param>
        /// <param name="configure">The <see cref="ConfigureOptions{TOptions}"/> as provided by the <see cref="Microsoft.Framework.OptionsModel"/>.</param>
        /// <returns></returns>
        internal TContext CreateContext()
        {
            // Make sure the configuration is valid.
            var errors = GetConfigurationErrors(Configuration);

            if (!errors.Any())
            {
                var ctx = new TContext();

                ctx.MongoClient = GetClient();
                ctx.Database = GetDatabase(ctx.MongoClient);

                // Call "SetupCollections" now when everything is initialized.
                ctx.SetupCollections();

                return ctx;
            }
            
            throw new ContextCreationException($"A valid configuration for the context '{_contextName}' was not provided.\r\nErrors:\r\n {String.Join("\r\n", errors.Select(e => e.Message))}");
        }

        /// <summary>
        /// Gets the a <see cref="IMongoClient"/> that is most suitable from the configuration.
        /// </summary>
        /// <returns></returns>
        internal IMongoClient GetClient()
        {
            // "Simple" configuration takes priority over "over-configuration"
            // To *not* use the ConnectionString in the configuration
            // simply omit it and use the MongoClientSettings instead.
            if (Configuration.ConnectionString != null && Configuration.DatabaseName != null)
            {
                return new MongoClient(Configuration.ConnectionString);
            }

            if (Configuration.ClientSettings != null)
            {
                return new MongoClient(Configuration.ClientSettings);
            }

            return null;
        }

        /// <summary>
        /// Gets a <see cref="IMongoDatabase"/> that is most suitable from the configuration.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <returns></returns>
        internal IMongoDatabase GetDatabase(IMongoClient client)
        {
            return client.GetDatabase(Configuration.DatabaseName, Configuration.DatabaseSettings);
        }

        #region -- Validation Constraints --

        private static readonly ImmutableArray<Func<MongoDBContextConfiguration, MongoDBConfigurationNotice>> _validationConstraints =
            ImmutableArray.Create<Func<MongoDBContextConfiguration, MongoDBConfigurationNotice>>(
            ((config) =>
            {
                // Validate whether a connection-string or client settings has been provided by the configuration.
                if ((config.ConnectionString != null) ||
                (
                    config.ClientSettings != null &&
                    (config.ClientSettings.Servers.Any() || config.ClientSettings.Server != null)
                ))
                {
                    return MongoDBConfigurationNotice.Valid;
                }
                return new MongoDBConfigurationNotice(true, $"A valid '{nameof(config.ConnectionString)}' (or '{nameof(config.ClientSettings)}') has not been provided.");
            }),

            ((config) =>
            {
                return // Make sure that a database name has been provided by the configuration 
                    config.DatabaseName != null
                        ? MongoDBConfigurationNotice.Valid
                        : MongoDBConfigurationNotice.MissingValue(nameof(config.DatabaseName));
            })
        );

        #endregion

        /// <summary>
        /// Determines whether the provided configuration can create a valid <see cref="MongoDBContext"/>.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        internal static bool IsConfigValid(MongoDBContextConfiguration config)
        {
            return !_validationConstraints.Any(c => c(config).IsError);
        }

        /// <summary>
        /// Gets configuration errors spawned by bad (or nonexistent) configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        internal static IEnumerable<MongoDBConfigurationNotice> GetConfigurationErrors(MongoDBContextConfiguration config)
        {
            return _validationConstraints.Select(c => c(config)).Where(c => c.IsError);
        }
    }
}
