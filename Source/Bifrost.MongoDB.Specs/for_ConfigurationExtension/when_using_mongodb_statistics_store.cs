﻿using Bifrost.Configuration;
using Machine.Specifications;

namespace Bifrost.MongoDB.Specs.for_ConfigurationExtension
{
    public class when_using_mongodb_statistics_store : given.a_statistics_configuration_and_container
    {
        Because of = () => { statistics_configuration.UsingMongoDB(); };

        It should_configure_the_store_to_a_mongodb_implementation = () =>
        {
            statistics_configuration.StoreType.FullName.ShouldBeTheSameAs(typeof(MongoDB.Statistics.StatisticsStore).FullName);
        };
    }
}
