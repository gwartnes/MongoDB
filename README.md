# MausWorks.MongoDB Service

A "Entity Framework-esque" service for MongoDB for usage in ASP.NET 5.

#Example usage:

##Startup.cs
    public void ConfigureServices(IServiceCollection services)
    {
        services
        	.AddMvc(Configuration)
        	.AddMongoDBContext<SomeContext>(Configuration.GetConfigurationSection("MongoDB:SomeContext"));
        
        // alternatively
        services.AddMongoDBContext<SomeContext>(o =>
		{
			o.ConnectionString = "mongodb://some-nice-connection-string";
			o.DatabaseName = "database, or empty if part of the connection-string";
		});
    }
    
##SomeContext
    public class SomeContext : MongoDBContext
    {
        public IMongoCollection<SomeEntity> Events { get; set; }

        public override void SetupCollections()
        {
            Events = CreateCollection<SomeEntity>("SomeEntities");
        }
    }
	
##Example configuration
    {
    	"MongoDB": {
    		"SomeContext": {
    			"ConnectionString": "mongodb://usr:passw@mongoserver.tld:1337/database-name",
    			"DatabaseName": "database-name"
    		}
    	}
    }

