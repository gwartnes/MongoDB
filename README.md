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
			o.ConnectionString = "mongodb://usr:passw@mongoserver.tld:1337/database-name";
			o.DatabaseName = "database-name"; //or null if part of the connection-string
		});
    }
    
##SomeContext
    public class SomeContext : MongoDBContext
    {
        public IMongoCollection<SomeEntity> SomeEntities { get; set; }

        public override void SetupCollections()
        {
            SomeEntities = CreateCollection<SomeEntity>("SomeEntities");
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

