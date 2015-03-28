using System;

namespace MausWorks.MongoDB.Exceptions
{
    public class ContextCreationException : Exception
    {
		public ContextCreationException(Type contextType) : 
			this(contextType, null)
		{

		}

		public ContextCreationException(Type contextType, Exception innerException) : 
			this(String.Format("The MongoDBContext of the type '{0}' could not be created.", contextType.Name), innerException)
        {

		}

		public ContextCreationException(string message, Exception innerException) : base(message, innerException)
		{
		}

		public ContextCreationException(string message) : base(message)
		{
		}
    }
}