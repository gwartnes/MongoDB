using MausWorks.MongoDB.Configuration;
using MausWorks.MongoDB.Exceptions;
using Microsoft.Framework.OptionsModel;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MausWorks.MongoDB
{
    internal class MongoDBContextProvider
    {
        private string ContextName { get; set; }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        internal MongoDBContextConfiguration Configuration { get; private set; }

        /// <summary>
        /// Creates a valid <see cref="MongoDBContext"/> given that the provided <see cref="MongoDBContextConfiguration"/> is valid.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <param name="options">The options as provided by the <see cref="Microsoft.Framework.OptionsModel"/>.</param>
        /// <param name="configure">The <see cref="ConfigureOptions{TOptions}"/> as provided by the <see cref="Microsoft.Framework.OptionsModel"/>.</param>
        /// <returns></returns>
        internal TContext CreateContext<TContext>(
                    IOptions<MongoDBContextConfiguration<TContext>> options,
                    IConfigureOptions<MongoDBContextConfiguration<TContext>> configure)

                    where TContext : MongoDBContext, new()
        {
            ContextName = typeof(TContext).Name;

            MongoDBContextConfiguration config;

            var ctx = new TContext();

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

            // Make sure the configuration is valid.
            var errors = GetConfigurationErrors(config);

            if (!errors.Any())
            {
                Configuration = config;

                ctx.MongoClient = GetClient();
                ctx.Database = GetDatabase(ctx.MongoClient);

                // Call "SetupCollections" now when everything is initialized.
                ctx.SetupCollections();

                return ctx;
            }

            string message = String.Format("A valid configuration for the context '{0}' was not provided.\r\nErrors:\r\n {1}", 
                ContextName, 
                String.Join("\r\n", errors.Select(e => e.Message)));

            throw new ContextCreationException(message);
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

        private static readonly ImmutableArray<Func<MongoDBContextConfiguration, MongoDBConfigurationNotice>> _validationConstraints = 
            ImmutableArray.Create<Func<MongoDBContextConfiguration, MongoDBConfigurationNotice>>(
            ((config) => {
                // Validate whether a connection-string or client settings has been provided by the configuration.
                if ((config.ConnectionString != null) ||
                (
                    config.ClientSettings != null &&
                    (config.ClientSettings.Servers.Any() || config.ClientSettings.Server != null)
                ))
                {
                    return MongoDBConfigurationNotice.Valid;
                }
                return new MongoDBConfigurationNotice(true, String.Format("A valid '{0}' (or '{1}') has not been provided.", nameof(config.ConnectionString), nameof(config.ClientSettings)));
            }),
            
            ((config) => {
                return // Make sure that a database name has been provided by the configuration 
                    config.DatabaseName != null
                        ? MongoDBConfigurationNotice.Valid
                        : MongoDBConfigurationNotice.MissingValue(nameof(config.DatabaseName));
            })
        );

        internal static bool IsConfigValid(MongoDBContextConfiguration config)
        {
            return !_validationConstraints.Any(c => c(config).IsError);
        }

        internal static IEnumerable<MongoDBConfigurationNotice> GetConfigurationErrors(MongoDBContextConfiguration config)
        {
            return _validationConstraints.Select(c => c(config)).Where(c => c.IsError);
        }
    }
}
