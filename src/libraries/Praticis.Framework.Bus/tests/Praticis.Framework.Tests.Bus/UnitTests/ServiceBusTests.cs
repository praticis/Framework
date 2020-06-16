
using System.Threading.Tasks;

using MediatR;
using Moq;
using Xunit;

using Praticis.Framework.Bus;
using Praticis.Framework.Bus.Abstractions;
using Praticis.Framework.Tests.Bus.Fakes;
using Praticis.Framework.Bus.Abstractions.Events;
using Praticis.Framework.Bus.Abstractions.Commands;
using Praticis.Framework.Bus.Abstractions.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Praticis.Framework.Tests.Bus.UnitTests
{
    public class ServiceBusTests
    {
        private Mock<IMediator> _mediatorMock { get; set; }
        private Mock<INotificationStore> _notificationsMock { get; set; }

        public ServiceBusTests()
        {
            this._mediatorMock = new Mock<IMediator>();
            this._notificationsMock = new Mock<INotificationStore>();
        }

        [Fact]
        public async Task ServiceBus_When_Publish_WaitClose_Event_Then_Publish_Event()
        {
            var @event = new WaitToCloseEvent();

            var bus = new ServiceBus(this._mediatorMock.Object, this._notificationsMock.Object);

            this._mediatorMock.Setup(m => m.Publish(@event, default))
                .Returns(Task.CompletedTask);

            this._notificationsMock.Setup(n => n.HasNotifications(true, false, true))
                .Returns(false);

            var result = await bus.PublishEvent(@event);

            this._mediatorMock.Verify(m => m.Publish(It.Is<IEvent>(e => e.EventId == @event.EventId), default), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public async Task ServiceBus_When_Publish_Enqueue_Event_Then_Enqueue_And_Not_Publish_Event()
        {
            var @event = new EnqueueEvent();
            var enqueueWorkEvent = new EnqueueWorkEvent(@event);

            var bus = new ServiceBus(this._mediatorMock.Object, this._notificationsMock.Object);

            this._mediatorMock.Setup(m => m.Publish(@event, default))
                .Returns(Task.CompletedTask);

            this._mediatorMock.Setup(m => m.Publish(enqueueWorkEvent, default))
                .Returns(Task.CompletedTask);
            
            this._notificationsMock.Setup(n => n.HasNotifications(true, false, true))
                .Returns(false);

            var result = await bus.PublishEvent(@event);

            this._mediatorMock.Verify(m => m.Publish(It.Is<IEvent>(e => e.EventId == @event.EventId), default), Times.Never);
            this._mediatorMock.Verify(m => m.Publish(It.Is<EnqueueWorkEvent>(e => e.Work.ObtainsWorkId() == @event.EventId), default), Times.Once);
            
            Assert.True(result);
        }

        [Fact]
        public async Task ServiceBus_When_Send_WaitClose_Command_Then_Send_Command()
        {
            var cmd = new WaitToCloseCommand();

            var bus = new ServiceBus(this._mediatorMock.Object, this._notificationsMock.Object);

            this._mediatorMock.Setup(m => m.Send(cmd, default))
                .Returns(Task.FromResult(true));

            this._notificationsMock.Setup(n => n.HasNotifications(true, false, true))
                .Returns(false);

            var result = await bus.SendCommand(cmd);

            this._mediatorMock.Verify(m => m.Send(It.Is<ICommand>(c => c.CommandId == cmd.CommandId), default), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public async Task ServiceBus_When_Send_Enqueue_Command_Then_Enqueue_And_Not_Send_Command()
        {
            var cmd = new EnqueueCommand();
            var enqueueWorkEvent = new EnqueueWorkEvent(cmd);

            var bus = new ServiceBus(this._mediatorMock.Object, this._notificationsMock.Object);

            this._mediatorMock.Setup(m => m.Send(cmd, default))
                .Returns(Task.FromResult(true));

            this._mediatorMock.Setup(m => m.Publish(enqueueWorkEvent, default))
                .Returns(Task.CompletedTask);

            this._notificationsMock.Setup(n => n.HasNotifications(true, false, true))
                .Returns(false);

            var result = await bus.SendCommand(cmd);

            this._mediatorMock.Verify(m => m.Send(It.Is<ICommand>(c => c.CommandId == cmd.CommandId), default), Times.Never);
            this._mediatorMock.Verify(m => m.Publish(It.Is<EnqueueWorkEvent>(e => e.Work.ObtainsWorkId() == cmd.CommandId), default), Times.Once);

            Assert.True(result);
        }

        [Fact]
        public async Task ServiceBus_When_Send_WaitClose_ResponseCommand_Then_Send_Command()
        {
            string testResult = "pong";

            var cmd = new WaitToCloseResponseCommand();

            var bus = new ServiceBus(this._mediatorMock.Object, this._notificationsMock.Object);

            this._mediatorMock.Setup(m => m.Send(cmd, default))
                .Returns(Task.FromResult(testResult));

            this._mediatorMock.Setup(m => m.Publish(new EnqueueWorkEvent(cmd), default))
                .Returns(Task.CompletedTask);

            this._notificationsMock.Setup(n => n.HasNotifications(true, false, true))
                .Returns(false);

            var result = await bus.SendCommand(cmd);

            this._mediatorMock.Verify(m => m.Send(It.Is<ICommand<string>>(c => c.CommandId == cmd.CommandId), default), Times.Once);
            this._mediatorMock.Verify(m => m.Publish(It.IsAny<EnqueueWorkEvent>(), default), Times.Never);
            Assert.Equal(testResult, result);
        }

        [Fact]
        public async Task ServiceBus_When_Enqueue_Command_Then_Change_Execution_Mode_To_Enqueue_And_Publish_EnqueueWorkEvent()
        {
            var cmd = new WaitToCloseCommand();

            var bus = new ServiceBus(this._mediatorMock.Object, this._notificationsMock.Object);

            this._mediatorMock.Setup(m => m.Publish(new EnqueueWorkEvent(cmd), default))
                .Returns(Task.CompletedTask);

            this._notificationsMock.Setup(n => n.HasNotifications(true, false, true))
                .Returns(false);

            var result = await bus.EnqueueWork(cmd);

            this._mediatorMock.Verify(m => m.Publish(It.Is<EnqueueWorkEvent>(e => e.Work.ObtainsWorkId() == cmd.CommandId), default), Times.Once);

            Assert.True(result);
            Assert.Equal(ExecutionMode.Enqueue, cmd.ExecutionMode);
        }

        [Fact]
        public async Task ServiceBus_When_Enqueue_Event_Then_Change_Execution_Mode_To_Enqueue_And_Publish_EnqueueWorkEvent()
        {
            var @event = new WaitToCloseEvent();

            var bus = new ServiceBus(this._mediatorMock.Object, this._notificationsMock.Object);

            this._mediatorMock.Setup(m => m.Publish(new EnqueueWorkEvent(@event), default))
                .Returns(Task.CompletedTask);

            this._notificationsMock.Setup(n => n.HasNotifications(true, false, true))
                .Returns(false);

            var result = await bus.EnqueueWork(@event);

            this._mediatorMock.Verify(m => m.Publish(It.Is<EnqueueWorkEvent>(e => e.Work.ObtainsWorkId() == @event.EventId), default), Times.Once);

            Assert.True(result);
            Assert.Equal(ExecutionMode.Enqueue, @event.ExecutionMode);
        }

        [Fact]
        public async Task ServiceBus_When_Enqueue_Works_Then_Change_Execution_Mode_To_Enqueue_And_Publish_EnqueueWorksEvent()
        {
            var works = new List<IWork>();

            works.Add(new EnqueueCommand());
            works.Add(new EnqueueEvent());
            works.Add(new EnqueueResponseCommand());
            works.Add(new WaitToCloseCommand());
            works.Add(new WaitToCloseEvent());
            works.Add(new WaitToCloseResponseCommand());

            var bus = new ServiceBus(this._mediatorMock.Object, this._notificationsMock.Object);

            this._mediatorMock.Setup(m => m.Publish(new EnqueueWorksEvent(works), default))
                .Returns(Task.CompletedTask);

            this._notificationsMock.Setup(n => n.HasNotifications(true, false, true))
                .Returns(false);

            var result = await bus.EnqueueWork(works);

            foreach (var work in works)
                this._mediatorMock.Verify(m => m.Publish(It.Is<EnqueueWorksEvent>(e => e.Works.Any(w => w.ObtainsWorkId() == work.ObtainsWorkId())), default), Times.Once);

            Assert.True(result);
            Assert.True(works.All(w => w.ExecutionMode == ExecutionMode.Enqueue));
        }
    }
}