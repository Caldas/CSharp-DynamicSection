using System;
using System.Configuration;
using VTEX.Configuration.DynamicSection;

namespace VTEX.Configuration.DynamicSection.SampleConsoleApplication
{
    /// <summary>
    /// This is a test propose demonstration plugin.
    /// 
    /// Knowing SampleConsoleApplication custom configuration samples fields I just hard coded to change one field value, but of course this could
    /// be also used to really get data from a defined storage.
    /// 
    /// Please take a look into the solutions and checkout others real world plugins.
    /// </summary>
    public class TestConfigurationReloadPlugin : IConfigurationReloadPlugin
    {
        public event EventHandler<Tuple<string, object>> PropertyLoaded = delegate { };

        public void GetLastestPropertiesValues(ConfigurationSection configurationSection)
        {
            foreach (PropertyInformation item in configurationSection.ElementInformation.Properties)
            {
                var value = item.Value;
                if (item.Name.Equals("testValue"))
                    value = "!!!! TEST WORKS !!";

                this.PropertyLoaded(this, new Tuple<string, object>(item.Name, value));
            }
        }
    }
}