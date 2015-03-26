using MongoDB.Driver;
using System;

namespace MausWorks.MongoDB
{
    public abstract class MongoDBContext
    {
		protected MongoServer MongoServer { get; set; }
		private MongoClient MongoClient { get; set; }
		protected MongoDatabase Database { get; private set; }

		private MongoDBContextParameters _contextParams;

		public static TContext Create<TContext>(IServiceProvider provider) where TContext : MongoDBContext, new()
		{
			var svc = GetMongoDBService(provider);

			var ctx = new TContext();

			ctx._contextParams = svc.Contexts[typeof(TContext).Name];

			ctx.MongoClient = new MongoClient(ctx._contextParams.ConnectionString);
			ctx.MongoServer = ctx.MongoClient.GetServer();

			ctx.Database = ctx.MongoServer.GetDatabase(ctx._contextParams.DatabaseName);

			ctx.SetUpEntities();

			return ctx;
		}

		public abstract void SetUpEntities();

		private static MongoDBService GetMongoDBService(IServiceProvider provider)
		{
			return (MongoDBService)provider.GetService(typeof (MongoDBService));
		}
    }
}