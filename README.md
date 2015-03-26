# MausWorks.MongoDB Service

A "Entity Framework-esque" service for MongoDB for usage in ASP.NET 5.

#Example usage:
An example can be found

##Startup.cs
    public void ConfigureServices(IServiceCollection services)
    {
        services
        	.AddMvc(Configuration)
        	.AddMongoDB(Configuration)
        	.AddMongoDBContext<TestContext>();
    }
    
##TestContext
	public class TestContext : MongoDBContext
	{
		public EntitySet<TestEntity> TestEntities { get; set; }

		public override void SetUpEntities()
		{
			TestEntities = new EntitySet<TestEntity>(Database, "TestEntities");
		}
	}
	
##config.json
    {
    	"MongoDB": {
    		"TestContext": {
    			"ConnectionString": "mongodb://usr:passw@mongoserver.tld:1337/database-name",
    			"DatabaseName": "database-name"
    		}
    	}
    }

