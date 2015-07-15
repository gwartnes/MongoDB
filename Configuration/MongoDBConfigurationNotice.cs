using System;
using System.Diagnostics;

namespace MausWorks.MongoDB.Configuration
{
    /// <summary>
    /// Provides notices for configuration-errors (and other notices)
    /// </summary>
    public class MongoDBConfigurationNotice
    {
        private const string _missingValueFormat = "A value for the property '{0}' was not provided by the configuration";

        private const string _defaultValidMessage = "";

        /// <summary>
        /// Provides a basic implementation of the <see cref="MongoDBConfigurationNotice"/> to be used where no further information is required.
        /// </summary>
        public static readonly MongoDBConfigurationNotice Valid = new MongoDBConfigurationNotice(isError: false, message: _defaultValidMessage);

        /// <summary>
        /// Gets a value indicating whether this notice is an error.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this notice is an error; otherwise, <c>false</c>.
        /// </value>
        public bool IsError { get; private set; }

        /// <summary>
        /// Gets the message describing this notice.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDBConfigurationNotice"/> class.
        /// </summary>
        /// <param name="isError">if set to <c>true</c> this notice will be an error-notice.</param>
        /// <param name="message">The message.</param>
        public MongoDBConfigurationNotice(bool isError, string message)
        {
            IsError = isError;
            Message = message;
        }

        /// <summary>
        /// Returns an error-notice with a standardized error message for missing values in the configuration, also sends a <see cref="Trace.TraceError(string)"/> with the standardized warning.
        /// </summary>
        /// <param name="propertyName">The name of the missing property.</param>
        /// <returns></returns>
        public static MongoDBConfigurationNotice MissingValue(string propertyName)
        {
            var errorMessage = String.Format(_missingValueFormat, propertyName);

            Trace.TraceError(errorMessage);

            return new MongoDBConfigurationNotice(isError: true, message: errorMessage);
        }

        /// <summary>
        /// Returns a non-error notice and sends a warning to the <see cref="Trace"/> log
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static MongoDBConfigurationNotice Warning(string message)
        {
            Trace.TraceWarning(message);
            
            return new MongoDBConfigurationNotice(isError: false, message: message);
        }
    }
}
