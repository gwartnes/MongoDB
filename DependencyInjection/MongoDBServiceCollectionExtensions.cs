using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using System;
using MausWorks.MongoDB.Configuration;
using Microsoft.Framework.OptionsModel;
using MausWorks.MongoDB.Builder;

namespace MausWorks.MongoDB.DependencyInjection
{
    /// <summary>
    /// Contains extensions for adding MongoDB-related services to a provided <see cref="IServiceCollection"/>
    /// </summary>
    public static class MongoDBServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a <see cref="MongoDBContext"/> to the provided <see cref="IServiceCollection"/>
        /// <para>
        /// This overload requires you to call <see cref="ConfigureMongoDBContext{TContext}(IServiceCollection, Action{MongoDBContextConfiguration{TContext}}, string)"/> or 
        /// <see cref="ConfigureMongoDBContext{TContext}(IServiceCollection, IConfiguration, string)"/> to configure the context.
        /// </para>
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the <see cref="MongoDBContext"/> to</param>
        /// <returns></returns>
        public static MongoDBContextBuilder<TContext> AddMongoDBContext<TContext>(this IServiceCollection services)
            where TContext : MongoDBContext, new()
        {
            return AddContext<TContext>(services);
        }

        /// <summary>
        /// Adds a <see cref="MongoDBContext"/> to the provided <see cref="IServiceCollection"/>
        /// </summary>
        /// <typeparam name="TContext">The type of the context to add</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the <see cref="MongoDBContext"/> to</param>
        /// <returns>The provided <see cref="IServiceCollection"/></returns>
		public static MongoDBContextBuilder<TContext> AddMongoDBContext<TContext>(this IServiceCollection services, Action<MongoDBContextConfiguration<TContext>> contextConfiguration = null, string optionsName = "")
            where TContext : MongoDBContext, new()
		{
            if (contextConfiguration != null)
            {
                ConfigureMongoDBContext(services, contextConfiguration, optionsName);
            }
            
            return AddContext<TContext>(services);
        }

        /// <summary>
        /// Adds a <see cref="MongoDBContext"/> to the provided <see cref="IServiceCollection"/>
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the <see cref="MongoDBContext"/> to</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="optionsName">Name of the options.</param>
        /// <returns></returns>
        public static MongoDBContextBuilder<TContext> AddMongoDBContext<TContext>(this IServiceCollection services, IConfiguration configuration = null, string optionsName = "")
            where TContext : MongoDBContext, new()
        {
            if (configuration != null)
            {
                ConfigureMongoDBContext<TContext>(services, configuration, optionsName);
            }
            
            return AddContext<TContext>(services);
        }

        /// <summary>
        /// Provides configuration (or further configuration) for <see cref="MongoDBContext"/>s
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the <see cref="MongoDBContext"/> to</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="optionsName">Name of the options.</param>
        /// <returns></returns>
        public static IServiceCollection ConfigureMongoDBContext<TContext>(this IServiceCollection services,  Action<MongoDBContextConfiguration<TContext>> configuration, string optionsName = "")
            where TContext : MongoDBContext, new()
        {
            services.Configure(configuration, optionsName);

            return services;
        }

        /// <summary>
        /// Provides configuration (or further configuration) for <see cref="MongoDBContext"/>s
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the <see cref="MongoDBContext"/> to</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="optionsName">Name of the options.</param>
        /// <returns></returns>
        public static IServiceCollection ConfigureMongoDBContext<TContext>(this IServiceCollection services, IConfiguration configuration, string optionsName = "")
            where TContext : MongoDBContext, new()
        {
            services.Configure<MongoDBContextConfiguration<TContext>>(configuration, optionsName);

            return services;
        }

        private static MongoDBContextBuilder<TContext> AddContext<TContext>(IServiceCollection services)
            where TContext : MongoDBContext, new()
        {
            var builder = new MongoDBContextBuilder<TContext>(services);
            
            return builder.Build();
        }

    }
}