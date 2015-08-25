using System;
using System.Collections.Generic;
using System.Configuration;

namespace DynamicSection
{
    /// <summary>
    /// Represents a interface to allow plugins to reload configuration properties based on any desired storage
    /// </summary>
    public interface IConfigurationReloadPlugin
    {
        /// <summary>
        /// Event used to set property values at 'DynamicConfigurationSection' by plugin
        /// </summary>
        event EventHandler<Tuple<string, object>> PropertyLoaded;

        /// <summary>
        /// Method used to start retreive process to get latest properties values from plugin storage
        /// </summary>
        /// <param name="configurationSection">Base configuration section, provided in order to be able to retreive properties</param>
        void GetLastestPropertiesValues(ConfigurationSection configurationSection);
    }
}