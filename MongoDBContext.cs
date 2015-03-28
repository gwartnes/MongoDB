using MausWorks.MongoDB.Exceptions;
using MongoDB.Driver;
using System;

namespace MausWorks.MongoDB
{
	public abstract class MongoDBContext
	{
		protected MongoServer MongoServer { get; set; }
		protected MongoClient MongoClient { get; set; }
		protected MongoDatabase Database { get; private set; }

		private MongoDBContextParameters _contextParams;

		public static TContext Create<TContext>(IServiceProvider provider) where TContext : MongoDBContext, new()
		{
			var svc = GetMongoDBService(provider);

			MongoDBContextParameters ctxParams;

			if (svc.Contexts.TryGetValue(typeof(TContext).Name, out ctxParams))
			{
				var ctx = new TContext();

				ctx._contextParams = svc.Contexts[typeof(TContext).Name];

				ctx.MongoClient = new MongoClient(ctx._contextParams.ConnectionString);
				ctx.MongoServer = ctx.MongoClient.GetServer();

				ctx.Database = ctx.MongoServer.GetDatabase(ctx._contextParams.DatabaseName);

				ctx.SetUpEntities();

				return ctx;
			}

			throw new ContextCreationException
			(
				typeof(TContext),
				innerException: new NullReferenceException("The configuration did not contain configuration-settings for the provided context.")
			);
		}

		public abstract void SetUpEntities();

		private static MongoDBService GetMongoDBService(IServiceProvider provider)
		{
			return (MongoDBService)provider.GetService(typeof(MongoDBService));
		}
	}
}