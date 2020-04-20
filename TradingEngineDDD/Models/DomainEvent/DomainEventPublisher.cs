using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradingEngineDDD.Models.DomainEvent
{
    public interface IDomainEventPublisher
    {
        void Publish(DomainEvent domainEvent);
    }

    public class DomainEventPublisher: IDomainEventPublisher
    {
        public void Publish(DomainEvent domainEvent)
        {
            //Publish here using 3rd party
        }
    }
}