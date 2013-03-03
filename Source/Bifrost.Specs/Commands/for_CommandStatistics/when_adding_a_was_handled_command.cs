﻿using Bifrost.Commands;
using Bifrost.Statistics;
using Machine.Specifications;
using System.Collections.Generic;

namespace Bifrost.Specs.Commands.for_CommandStatistics
{
    public class when_adding_a_was_handled_command : given.a_command_statistics_with_registered_plugins
    {
        static Command command = new Command();

        Because of = () => command_statistics.WasHandled(command);

        It should_add_the_command_to_the_store = () =>
        {
            statistics_store
                .Verify(store =>
                    store.Add(Moq.It.Is<IStatistic>(stat => stat.Categories.Contains(
                        new KeyValuePair<string, string>("CommandStatistics","WasHandled")))), Moq.Times.Once());
        };

        It should_be_effected_by_a_registered_plugin = () =>
        {
            statistics_store
                .Verify(store =>
                    store.Add(Moq.It.Is<IStatistic>(stat => stat.Categories.Contains(
                        new KeyValuePair<string, string>("DummyStatisticsPluginContext", "I touched a was handled statistic")))), Moq.Times.Once());
        };
    }
}