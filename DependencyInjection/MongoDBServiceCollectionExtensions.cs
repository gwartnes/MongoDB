using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;

namespace MausWorks.MongoDB.DependencyInjection
{
    public static class MongoDBServiceCollectionExtensions
    {
		public static IServiceCollection AddMongoDB(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped(factory => new MongoDBService(configuration));
			
			return services;
		}

		public static IServiceCollection AddMongoDBContext<TContext>(this IServiceCollection services)
			where TContext : MongoDBContext, new()
		{
			services.AddScoped(serviceProvider => MongoDBContext.Create<TContext>(serviceProvider));

			return services;
		}
	}
}