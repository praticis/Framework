
using System;

using Praticis.Framework.Bus.Abstractions.Enums;
using Praticis.Framework.Tests.Bus.Abstractions.Fakes;

using Xunit;

namespace Praticis.Framework.Tests.Bus.Abstractions.UnitTests
{
    public class ResponseCommandTests
    {
        [Fact]
        public void ResponseCommand_Created_Successfully()
        {
            var cmd = new ResponseCommand();

            Assert.NotEqual(default, cmd.CommandId);
            Assert.Equal(cmd.CommandId, cmd.ObtainsWorkId());
            Assert.NotEqual(default, cmd.Time);
            Assert.Equal(typeof(ResponseCommand), cmd.ResourceType);
            Assert.Equal(typeof(ResponseCommand).Name, cmd.CommandName);
            Assert.Equal(ExecutionMode.WaitToClose, cmd.ExecutionMode);
            Assert.Equal(NotificationType.Default, cmd.NotificationType);
            Assert.Equal(WorkType.Command, cmd.WorkType);
            Assert.Equal(cmd.CommandName, cmd.ObtainsWorkName());
            Assert.True(cmd.IsValid);
        }

        [Theory]
        [InlineData(ExecutionMode.WaitToClose)]
        [InlineData(ExecutionMode.Enqueue)]
        public void ResponseCommand_Change_Execution_Mode_To_Enqueue(ExecutionMode executionMode)
        {
            var cmd = new ResponseCommand();
            cmd.ChangeExecutionMode(executionMode);

            Assert.Equal(executionMode, cmd.ExecutionMode);
        }

        [Fact]
        public void ResponseCommand_Change_CommandId_When_ChangeWorkId()
        {
            Guid newCommandId = Guid.NewGuid();

            var cmd = new ResponseCommand();

            cmd.ChangeWorkId(newCommandId);

            Assert.Equal(newCommandId, cmd.CommandId);
            Assert.Equal(newCommandId, cmd.ObtainsWorkId());
        }
    }
}