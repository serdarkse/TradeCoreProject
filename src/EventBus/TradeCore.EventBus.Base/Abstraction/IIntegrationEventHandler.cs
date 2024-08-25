using TradeCore.EventBus.Base.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeCore.EventBus.Base.Abstraction
{
    //IIntegrationEventHandler interfacesi yaratıldı. 
    //içerisine dinamik bir tip alacak. tipin IntegrationEvent olma zorunluluğu vardır. =>where TIntegrationEvent:IntegrationEvent
    public interface IIntegrationEventHandler<TIntegrationEvent> : IntegrationEventHandler where TIntegrationEvent:IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }

    public interface IntegrationEventHandler
    {

    }
}
