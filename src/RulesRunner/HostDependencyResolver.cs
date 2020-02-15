using Microsoft.Extensions.DependencyInjection;
using NRules.Extensibility;
using System;

namespace RulesRunner {
    public class HostDependencyResolver : IDependencyResolver {
        private readonly IServiceProvider _container;

        public HostDependencyResolver(IServiceProvider serviceProvider) {
            _container = serviceProvider;
        }

        public object Resolve(IResolutionContext context, Type serviceType) {
            return _container.GetRequiredService(serviceType);
        }
    }
}