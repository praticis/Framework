
using System;

using Xunit;

using Praticis.Framework.Bus.Abstractions;
using Praticis.Framework.Bus.Abstractions.Enums;

namespace Praticis.Framework.Tests.Bus.Abstractions.UnitTests
{
    public class SystemErrorTests
    {
        [Fact]
        public void SystemError_With_Message_Created_Successfully()
        {
            var msg = "Error Message";
            var error = new SystemError(msg);

            Assert.NotEqual(default, error.EventId);
            Assert.Null(error.Code);
            Assert.Equal(msg, error.Message);
            Assert.Null(error.Exception);
            Assert.Null(error.InnerException);
            Assert.Null(error.StackTrace);
            Assert.NotNull(error.SourceMethod);
            Assert.True(error.SourceLineNumber >= 0);
            Assert.NotNull(error.SourceFileName);
            Assert.Null(error.ObjectManipulated);
            Assert.Equal(WorkType.Event, error.WorkType);
            Assert.True(error.IsValid);
            Assert.Empty(error.Validate());
        }

        [Fact]
        public void SystemError_With_Message_And_ObjectManipulated_Created_Successfully()
        {
            string msg = "Error Message";
            object obj = new Fakes.DefaultCommand();
            
            var error = new SystemError(msg, obj);

            Assert.NotEqual(default, error.EventId);
            Assert.Null(error.Code);
            Assert.Equal(msg, error.Message);
            Assert.Null(error.Exception);
            Assert.Null(error.InnerException);
            Assert.Null(error.StackTrace);
            Assert.NotNull(error.SourceMethod);
            Assert.True(error.SourceLineNumber >= 0);
            Assert.NotNull(error.SourceFileName);
            Assert.Equal(obj, error.ObjectManipulated);
            Assert.Equal(WorkType.Event, error.WorkType);
            Assert.True(error.IsValid);
            Assert.Empty(error.Validate());
        }

        [Fact]
        public void SystemError_With_Code_And_Message_And_ObjectManipulated_Created_Successfully()
        {
            string code = "1", msg = "Error Message";
            object obj = new Fakes.DefaultCommand();

            var error = new SystemError(code, msg, obj);

            Assert.NotEqual(default, error.EventId);
            Assert.Equal(code, error.Code);
            Assert.Equal(msg, error.Message);
            Assert.Null(error.Exception);
            Assert.Null(error.InnerException);
            Assert.Null(error.StackTrace);
            Assert.NotNull(error.SourceMethod);
            Assert.True(error.SourceLineNumber >= 0);
            Assert.NotNull(error.SourceFileName);
            Assert.Equal(obj, error.ObjectManipulated);
            Assert.Equal(WorkType.Event, error.WorkType);
            Assert.True(error.IsValid);
            Assert.Empty(error.Validate());
        }

        [Fact]
        public void SystemError_With_Message_And_Exception_Created_Successfully()
        {
            string msg = "Error Message";
            Exception exception = null;

            try
            {
                string ex = null;
                ex.ToString();
            }
            catch (Exception e)
            {
                exception = e;
            }

            var error = new SystemError(msg, exception);
            
            Assert.NotEqual(default, error.EventId);
            Assert.Null(error.Code);
            Assert.Equal(msg, error.Message);
            Assert.NotNull(error.Exception);
            Assert.Null(error.InnerException);
            Assert.NotNull(error.StackTrace);
            Assert.NotNull(error.SourceMethod);
            Assert.True(error.SourceLineNumber >= 0);
            Assert.NotNull(error.SourceFileName);
            Assert.Null(error.ObjectManipulated);
            Assert.Equal(WorkType.Event, error.WorkType);
            Assert.True(error.IsValid);
            Assert.Empty(error.Validate());
        }

        [Fact]
        public void SystemError_With_Message_And_ObjectManipulated_And_Exception_Created_Successfully()
        {
            string msg = "Error Message";
            object obj = new Fakes.DefaultCommand();
            Exception exception = null;

            try
            {
                string ex = null;
                ex.ToString();
            }
            catch (Exception e)
            {
                exception = e;
            }

            var error = new SystemError(msg, obj, exception);

            Assert.NotEqual(default, error.EventId);
            Assert.Null(error.Code);
            Assert.Equal(msg, error.Message);
            Assert.NotNull(error.Exception);
            Assert.Null(error.InnerException);
            Assert.NotNull(error.StackTrace);
            Assert.NotNull(error.SourceMethod);
            Assert.True(error.SourceLineNumber >= 0);
            Assert.NotNull(error.SourceFileName);
            Assert.Equal(obj, error.ObjectManipulated);
            Assert.Equal(WorkType.Event, error.WorkType);
            Assert.True(error.IsValid);
            Assert.Empty(error.Validate());
        }

        [Fact]
        public void SystemError_With_Code_And_Message_And_Exception_Created_Successfully()
        {
            string code = "1", msg = "Error Message";
            Exception exception = null;

            try
            {
                string ex = null;
                ex.ToString();
            }
            catch (Exception e)
            {
                exception = e;
            }

            var error = new SystemError(code, msg, exception);

            Assert.NotEqual(default, error.EventId);
            Assert.Equal(code, error.Code);
            Assert.Equal(msg, error.Message);
            Assert.NotNull(error.Exception);
            Assert.Null(error.InnerException);
            Assert.NotNull(error.StackTrace);
            Assert.NotNull(error.SourceMethod);
            Assert.True(error.SourceLineNumber >= 0);
            Assert.NotNull(error.SourceFileName);
            Assert.Null(error.ObjectManipulated);
            Assert.Equal(WorkType.Event, error.WorkType);
            Assert.True(error.IsValid);
            Assert.Empty(error.Validate());
        }

        [Fact]
        public void SystemError_With_Code_And_Message_And_ObjectManipulated_And_Exception_Created_Successfully()
        {
            string code = "1", msg = "Error Message";
            object obj = new Fakes.DefaultCommand();
            Exception exception = null;

            try
            {
                string ex = null;
                ex.ToString();
            }
            catch (Exception e)
            {
                exception = e;
            }

            var error = new SystemError(code, msg, obj, exception);

            Assert.NotEqual(default, error.EventId);
            Assert.Equal(code, error.Code);
            Assert.Equal(msg, error.Message);
            Assert.NotNull(error.Exception);
            Assert.Null(error.InnerException);
            Assert.NotNull(error.StackTrace);
            Assert.NotNull(error.SourceMethod);
            Assert.True(error.SourceLineNumber >= 0);
            Assert.NotNull(error.SourceFileName);
            Assert.Equal(obj, error.ObjectManipulated);
            Assert.Equal(WorkType.Event, error.WorkType);
            Assert.True(error.IsValid);
            Assert.Empty(error.Validate());
        }

        [Fact]
        public void SystemError_ToString_Contains_Notification_Details()
        {
            string msg = "Log Message";
            var log = new SystemError(msg);

            Assert.Contains(msg, log.ToString());
            Assert.Contains(log.SourceMethod, log.ToString());
            Assert.Contains(log.SourceLineNumber.ToString(), log.ToString());
            Assert.Contains(log.SourceFileName, log.ToString());
        }
    }
}