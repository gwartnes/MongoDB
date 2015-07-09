using MausWorks.MongoDB.Configuration;
using MausWorks.MongoDB.Exceptions;
using Microsoft.Framework.OptionsModel;
using MongoDB.Driver;

namespace MausWorks.MongoDB
{
    internal class MongoDBContextProvider
    {
        internal TContext CreateContext<TContext>(
            IOptions<MongoDBContextConfiguration<TContext>> options, 
            IConfigureOptions<MongoDBContextConfiguration<TContext>> configure)
             
            where TContext : MongoDBContext, new()
        {
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

            if (config.ConnectionString != null && config.DatabaseName != null)
            {
                ctx.MongoClient = new MongoClient(config.ConnectionString.ToString());
                ctx.Database = ctx.MongoClient.GetDatabase(config.DatabaseName);

                ctx.SetupCollections();

                return ctx;
            }

            throw new ContextCreationException(typeof(TContext));
        }
    }
}
