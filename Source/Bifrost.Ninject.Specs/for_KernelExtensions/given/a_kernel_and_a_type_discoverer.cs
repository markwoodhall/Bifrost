﻿using Bifrost.Execution;
using Machine.Specifications;
using Moq;
using Ninject;
using Ninject.Activation;

namespace Bifrost.Ninject.Specs.for_KernelExtensions.given
{
    public class a_kernel_and_a_type_discoverer
    {
        protected static Mock<ITypeDiscoverer> type_discoverer_mock;
        protected static Mock<IKernel> kernel_mock;

        Establish context = () =>
        {
            type_discoverer_mock = new Mock<ITypeDiscoverer>();
            kernel_mock = new Mock<IKernel>();
            kernel_mock.Setup(k => k.Resolve(Moq.It.IsAny<IRequest>())).Returns(new[] { type_discoverer_mock.Object });
        };

    }
}
