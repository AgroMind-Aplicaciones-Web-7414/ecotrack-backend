using Cortex.Mediator.Notifications;
using EcotrackPlatform.API.Shared.Domain.Model.Events;

namespace EcotrackPlatform.Shared.Application.Internal.EventHandlers;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IEvent
{
    
}