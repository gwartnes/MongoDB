using MausWorks.MongoDB.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.OptionsModel;

namespace MausWorks.MongoDB.Builder
{
    public partial class MongoDBContextBuilder<TContext>
        where TContext : MongoDBContext, new()
    {
        private TContext _context;

        public IServiceCollection Services { get; set; }

        public MongoDBContextBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public MongoDBContextBuilder<TContext> Build()
        {
            Services.AddSingleton(serviceProvider =>
            {
                var contextProvider = new MongoDBContextProvider<TContext>(
                    serviceProvider.GetRequiredService<IOptions<MongoDBContextConfiguration<TContext>>>(),
                    serviceProvider.GetService<IConfigureOptions<MongoDBContextConfiguration<TContext>>>());

                // This is pretty hacky...
                _context = contextProvider.CreateContext();

                return _context;
            });

            return this;
        }
    }
}
