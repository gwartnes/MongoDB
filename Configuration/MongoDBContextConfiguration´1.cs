namespace MausWorks.MongoDB.Configuration
{
    public class MongoDBContextConfiguration<TContext> : MongoDBContextConfiguration
        where TContext : MongoDBContext, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDBContextConfiguration{TContext}"/> class.
        /// </summary>
        public MongoDBContextConfiguration()
            : base(typeof(TContext).Name)
        {

        }
    }
}
