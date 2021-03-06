﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bifrost.Testing.Fakes.Events;
using Machine.Specifications;
using Bifrost.Events;

namespace Bifrost.Specs.Events.for_EventSubscriptionRepository
{
    public class when_getting_by_id : given.an_event_subscription_repository
    {
        static Guid subscription_id = Guid.NewGuid();
        static EventSubscription    subscription;

        Establish context = () => entity_context_mock.Setup(e=>e.Entities).Returns((new[] { 
            new EventSubscription 
            { 
                EventType = typeof(SimpleEvent),
                Owner = typeof(Subscribers),
                Id = subscription_id 
            }
        }).AsQueryable());

        Because of = () => subscription = repository.Get(subscription_id);

        It should_return_the_subscripion = () => subscription.Id.ShouldEqual(subscription_id);
    }
}
