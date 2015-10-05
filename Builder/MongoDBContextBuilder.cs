using MausWorks.MongoDB.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.OptionsModel;

namespace MausWorks.MongoDB.Builder
{
    public partial class MongoDBContextBuilder<TContext>
        where TContext : MongoDBContext, new()
    {
        public TContext MongoDBContext { get; set; }

        public IServiceCollection Services { get; set; }

        public MongoDBContextBuilder(IServiceCollection services)
        {
            Services = services;
        }

        /// <summary>
        /// Builds the <see cref="MongoDBContext"/>
        /// </summary>
        /// <returns></returns>
        public MongoDBContextBuilder<TContext> Build()
        {
            Services.AddScoped(serviceProvider =>
            {
                var contextProvider = new MongoDBContextProvider<TContext>(
                    serviceProvider.GetRequiredService<IOptions<MongoDBContextConfiguration<TContext>>>(),
                    serviceProvider.GetService<IConfigureOptions<MongoDBContextConfiguration<TContext>>>());

                // This is pretty hacky...
                // Is setting something from outside the scope of a Func<> considered bad practice?
                MongoDBContext = contextProvider.CreateContext();

                return MongoDBContext;
            });

            return this;
        }
    }
}
