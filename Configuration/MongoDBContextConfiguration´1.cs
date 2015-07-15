namespace MausWorks.MongoDB.Configuration
{
    /// <summary>
    /// Configuration class for <see cref="MongoDBContext"/>s
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
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
