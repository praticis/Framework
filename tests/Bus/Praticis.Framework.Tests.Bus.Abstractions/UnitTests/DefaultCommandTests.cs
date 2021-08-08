
using System;

using Praticis.Framework.Bus.Abstractions.Enums;
using Praticis.Framework.Tests.Bus.Abstractions.Fakes;

using Xunit;

namespace Praticis.Framework.Tests.Bus.Abstractions.UnitTests
{
    public class DefaultCommandTests
    {
        [Fact]
        public void DefaultCommand_Created_Successfully()
        {
            var cmd = new DefaultCommand();

            Assert.NotEqual(default, cmd.CommandId);
            Assert.Equal(cmd.CommandId, cmd.ObtainsWorkId());
            Assert.NotEqual(default, cmd.Time);
            Assert.Equal(typeof(DefaultCommand), cmd.ResourceType);
            Assert.Equal(typeof(DefaultCommand).Name, cmd.CommandName);
            Assert.Equal(ExecutionMode.WaitToClose, cmd.ExecutionMode);
            Assert.Equal(NotificationType.Default, cmd.NotificationType);
            Assert.Equal(WorkType.Command, cmd.WorkType);
            Assert.Equal(cmd.CommandName, cmd.ObtainsWorkName());
            Assert.True(cmd.IsValid);
        }

        [Theory]
        [InlineData(ExecutionMode.WaitToClose)]
        [InlineData(ExecutionMode.Enqueue)]
        public void DefaultCommand_Change_Execution_Mode(ExecutionMode executionMode)
        {
            var cmd = new DefaultCommand();
            cmd.ChangeExecutionMode(executionMode);

            Assert.Equal(executionMode, cmd.ExecutionMode);
        }

        [Fact]
        public void DefaultCommand_Change_CommandId_When_ChangeWorkId_Is_Changed()
        {
            Guid newCommandId = Guid.NewGuid();

            var cmd = new DefaultCommand();

            cmd.ChangeWorkId(newCommandId);

            Assert.Equal(newCommandId, cmd.CommandId);
            Assert.Equal(newCommandId, cmd.ObtainsWorkId());
        }
    }
}