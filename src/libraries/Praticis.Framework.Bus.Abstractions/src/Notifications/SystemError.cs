
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using FluentValidation.Results;

namespace Praticis.Framework.Bus.Abstractions
{
    /// <summary>
    /// A system error notification message.
    /// </summary>
    public class SystemError : Notification
    {
        #region Properties

        /// <summary>
        /// The system error exception.
        /// </summary>
        public string Exception { get; private set; }

        /// <summary>
        /// The system error inner exception.
        /// </summary>
        public string InnerException { get; private set; }

        /// <summary>
        /// The system error stack trace.
        /// </summary>
        public string StackTrace { get; private set; }

        /// <summary>
        /// The source method where the system error was registered.
        /// <strong>It is captured automatically</strong>.
        /// </summary>
        public string SourceMethod { get; private set; }

        /// <summary>
        /// The source line number where the system error was registered.
        /// <strong>It is captured automatically</strong>.
        /// </summary>
        public int SourceLineNumber { get; private set; }

        /// <summary>
        /// The source file name where the system error was registered.
        /// <strong>It is captured automatically</strong>.
        /// </summary>
        public string SourceFileName { get; private set; }

        /// <summary>
        /// The object data related to the system error.
        /// </summary>
        public object ObjectManipulated { get; private set; }

        #endregion

        #region Constructors
        
        /// <summary>
        /// Create a system error notification message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="sourceMethod">
        /// The source method where the error was registered.
        /// <strong>It is captured automatically</strong>.
        /// </param>
        /// <param name="sourceLineNumber">
        /// The source line number where the error was registered.
        /// <strong>It is captured automatically</strong>.
        /// </param>
        /// <param name="sourceFileName">
        /// The source file name where the error was registered.
        /// <strong>It is captured automatically</strong>.
        /// </param>
        public SystemError(string errorMessage, [CallerMemberName] string sourceMethod = "",
            [CallerLineNumber] int sourceLineNumber = 0, [CallerFilePath] string sourceFileName = "")
            : base(errorMessage, Enums.NotificationType.System_Error)
        {
            this.Message = errorMessage;
            this.SourceMethod = sourceMethod;
            this.SourceLineNumber = sourceLineNumber;
            this.SourceFileName = sourceFileName;
        }
        
        /// <summary>
        /// Create a system error notification message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="objectManipulated">The object data related to the error.</param>
        /// <param name="sourceMethod">
        /// The source method where the log was registered.
        /// <strong>It is captured automatically</strong>.
        /// </param>
        /// <param name="sourceLineNumber">
        /// The source line number where the log was registered.
        /// <strong>It is captured automatically</strong>.
        /// </param>
        /// <param name="sourceFileName">
        /// The source file name where the log was registered.
        /// <strong>It is captured automatically</strong>.
        /// </param>
        public SystemError(string errorMessage, object objectManipulated, [CallerMemberName] string sourceMethod = "", 
            [CallerLineNumber] int sourceLineNumber = 0, [CallerFilePath] string sourceFileName = "")
            : base(errorMessage, Enums.NotificationType.System_Error)
        {
            this.Message = errorMessage;
            this.ObjectManipulated = objectManipulated;
            this.SourceMethod = sourceMethod;
            this.SourceLineNumber = sourceLineNumber;
            this.SourceFileName = sourceFileName;
        }

        /// <summary>
        /// Create a system error notification message.
        /// </summary><param name="errorCode">The message code.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="objectManipulated">The object data related to the error.</param>
        /// <param name="sourceMethod">
        /// The source method where the log was registered.
        /// <strong>It is captured automatically</strong>.
        /// </param>
        /// <param name="sourceLineNumber">
        /// The source line number where the log was registered.
        /// <strong>It is captured automatically</strong>.
        /// </param>
        /// <param name="sourceFileName">
        /// The source file name where the log was registered.
        /// <strong>It is captured automatically</strong>.
        /// </param>
        public SystemError(string errorCode, string errorMessage, object objectManipulated, 
            [CallerMemberName] string sourceMethod = "", [CallerLineNumber] int sourceLineNumber = 0, 
            [CallerFilePath] string sourceFileName = "")
            : base(errorMessage, Enums.NotificationType.System_Error)
        {
            this.Code = errorCode;
            this.Message = errorMessage;
            this.ObjectManipulated = objectManipulated;
            this.SourceMethod = sourceMethod;
            this.SourceLineNumber = sourceLineNumber;
            this.SourceFileName = sourceFileName;
        }

        /// <summary>
        /// Create a system error notification message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="exception">The exception error.</param>
        /// <param name="sourceMethod">
        /// The source method where the log was registered.
        /// <strong>It is captured automatically</strong>.
        /// </param>
        /// <param name="sourceLineNumber">
        /// The source line number where the log was registered.
        /// <strong>It is captured automatically</strong>.
        /// </param>
        /// <param name="sourceFileName">
        /// The source file name where the log was registered.
        /// <strong>It is captured automatically</strong>.
        /// </param>
        public SystemError(string errorMessage, Exception exception, [CallerMemberName] string sourceMethod = "", 
            [CallerLineNumber] int sourceLineNumber = 0, [CallerFilePath] string sourceFileName = "")
            : base(errorMessage, Enums.NotificationType.System_Error)
        {
            this.Message = errorMessage;
            this.Exception = exception?.Message;
            this.StackTrace = exception?.StackTrace;
            this.InnerException = exception?.InnerException?.Message;
            this.SourceMethod = sourceMethod;
            this.SourceLineNumber = sourceLineNumber;
            this.SourceFileName = sourceFileName;
        }

        /// <summary>
        /// Create a system error notification message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="objectManipulated">The object data related to the error.</param>
        /// <param name="exception">The exception error.</param>
        /// <param name="sourceMethod">
        /// The source method where the log was registered.
        /// <strong>It is captured automatically</strong>.
        /// </param>
        /// <param name="sourceLineNumber">
        /// The source line number where the log was registered.
        /// <strong>It is captured automatically</strong>.
        /// </param>
        /// <param name="sourceFileName">
        /// The source file name where the log was registered.
        /// <strong>It is captured automatically</strong>.
        /// </param>
        public SystemError(string errorMessage, object objectManipulated, Exception exception, 
            [CallerMemberName] string sourceMethod = "", [CallerLineNumber] int sourceLineNumber = 0, 
            [CallerFilePath] string sourceFileName = "")
            : base(errorMessage, Enums.NotificationType.System_Error)
        {
            this.Message = errorMessage;
            this.ObjectManipulated = objectManipulated;
            this.Exception = exception?.Message;
            this.StackTrace = exception?.StackTrace;
            this.InnerException = exception?.InnerException?.Message;
            this.SourceMethod = sourceMethod;
            this.SourceLineNumber = sourceLineNumber;
            this.SourceFileName = sourceFileName;
        }

        /// <summary>
        /// Create a system error notification message.
        /// </summary>
        /// <param name="errorCode">The message code.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="exception">The exception error.</param>
        /// <param name="sourceMethod">
        /// The source method where the log was registered.
        /// <strong>It is captured automatically</strong>.
        /// </param>
        /// <param name="sourceLineNumber">
        /// The source line number where the log was registered.
        /// <strong>It is captured automatically</strong>.
        /// </param>
        /// <param name="sourceFileName">
        /// The source file name where the log was registered.
        /// <strong>It is captured automatically</strong>.
        /// </param>
        public SystemError(string errorCode, string errorMessage, Exception exception, [CallerMemberName] string sourceMethod = "",
            [CallerLineNumber] int sourceLineNumber = 0, [CallerFilePath] string sourceFileName = "")
            : base(errorMessage, Enums.NotificationType.System_Error)
        {
            this.Code = errorCode;
            this.Message = errorMessage;
            this.Exception = exception?.Message;
            this.StackTrace = exception?.StackTrace;
            this.InnerException = exception?.InnerException?.Message;
            this.SourceMethod = sourceMethod;
            this.SourceLineNumber = sourceLineNumber;
            this.SourceFileName = sourceFileName;
        }

        /// <summary>
        /// Create a system error notification message.
        /// </summary>
        /// <param name="errorCode">The message code.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="objectManipulated">The object data related to the error.</param>
        /// <param name="exception">The exception error.</param>
        /// <param name="sourceMethod">
        /// The source method where the log was registered.
        /// <strong>It is captured automatically</strong>.
        /// </param>
        /// <param name="sourceLineNumber">
        /// The source line number where the log was registered.
        /// <strong>It is captured automatically</strong>.
        /// </param>
        /// <param name="sourceFileName">
        /// The source file name where the log was registered.
        /// <strong>It is captured automatically</strong>.
        /// </param>
        public SystemError(string errorCode, string errorMessage, object objectManipulated, Exception exception, 
            [CallerMemberName] string sourceMethod = "", [CallerLineNumber] int sourceLineNumber = 0, 
            [CallerFilePath] string sourceFileName = "")
            : base(errorMessage, Enums.NotificationType.System_Error)
        {
            this.Code = errorCode;
            this.Message = errorMessage;
            this.ObjectManipulated = objectManipulated;
            this.Exception = exception?.Message;
            this.StackTrace = exception?.StackTrace;
            this.InnerException = exception?.InnerException?.Message;
            this.SourceMethod = sourceMethod;
            this.SourceLineNumber = sourceLineNumber;
            this.SourceFileName = sourceFileName;
        }

        #endregion

        public override IEnumerable<ValidationFailure> Validate()
        {
            return base.Validate();
        }

        public override string ToString()
            => $"{this.Message} [Method: {this.SourceMethod}; Line: {this.SourceLineNumber}; File: {this.SourceFileName}]";
    }
}