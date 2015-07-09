using System;

namespace MausWorks.MongoDB.Exceptions
{
    /// <summary>
    /// Represents errors that may occur when creating a <see cref="MongoDBContext"/>
    /// </summary>
    public class ContextCreationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContextCreationException"/> class.
        /// </summary>
        /// <param name="contextType">The <see cref="Type"/> of the context that failed to create</param>
		public ContextCreationException(Type contextType) : 
			this(contextType, null)
		{ }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextCreationException"/> class.
        /// </summary>
        /// <param name="contextType">The <see cref="Type"/> of the context that failed to create</param>
        /// <param name="innerException">
        /// <para>The exception that is the cause of the current exception, or a null reference</para>
        /// <para>if no inner exception is specified.</para>
        /// </param>
		public ContextCreationException(Type contextType, Exception innerException) : 
			this(String.Format("The MongoDBContext of the type '{0}' could not be created.", contextType.Name), innerException)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextCreationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">
        /// <para>The exception that is the cause of the current exception, or a null reference</para>
        /// <para>if no inner exception is specified.</para>
        /// </param>
        public ContextCreationException(string message, Exception innerException) : 
            base(message, innerException)
		{ }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextCreationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
		public ContextCreationException(string message) : base(message)
		{ }
    }
}