using System.Collections.Generic;
using Autofac;

namespace Moonlit.Mvc.Maintenance
{
    public class ModuleConfiguration
    {
        private readonly IEnumerable<IModuleConfiguration> _moduleConfigurations;

        public ModuleConfiguration(IEnumerable<IModuleConfiguration> moduleConfigurations)
        {
            _moduleConfigurations = moduleConfigurations;
        }

        public void Configure(IContainer container)
        {
            foreach (var moduleConfiguration in _moduleConfigurations)
            {
                moduleConfiguration.Configure(container);
            }
        }
    }
}