using System;
using System.Configuration;
using System.Timers;

namespace DynamicSection
{
    /// <summary>
    /// Define a configuration section capable of running a refresh plugin instance, allowing refresh configuration every minute without 
    /// restart application
    /// </summary>
    public class DynamicConfigurationSection : ConfigurationSection
    {
        private OnClockMinuteTimer timer = null;
        private IConfigurationReloadPlugin configReloadable = null;
        private DynamicConfigurationSection section = null;

        private static int index = 0;
        private object lockOject = new object();

        /// <summary>
        /// This property wrap '<code>(DynamicConfigurationSection)ConfigurationManager.GetSection(base.SectionInformation.Name)</code>' necessary to acess SectionInstance
        /// </summary>
        public DynamicConfigurationSection Section
        {
            get
            {
                if (section != null)
                    return section;
                else
                {
                    //This lock mechanism is used to avoid two or more simultaneous calls to set 'section' field
                    //This will not impact performance because once 'section' field is set, it started to be returned above, befor this code.
                    lock (lockOject)
                    {
                        if (section == null)
                            section = (DynamicConfigurationSection)ConfigurationManager.GetSection(base.SectionInformation.Name);
                    }
                    return section;
                }
            }
            set { section = value; }
        }

        /// <summary>
        /// Get or set the type that should be used as plugin to during configuration retreive process
        /// </summary>
        [ConfigurationProperty("refreshPluginType", IsRequired = false)]
        public string RefreshPluginType
        {
            get
            {
                return this["refreshPluginType"].ToString();
            }
            set
            {
                this["refreshPluginType"] = value;
            }
        }

        /// <summary>
        /// Create a 'DynamicConfigurationSection' instance and start 'OnClockMinute' monitoring timer
        /// </summary>
        public DynamicConfigurationSection()
        {
            System.Threading.Interlocked.Increment(ref index);
            if (index == 1)//this trick is used to avoid create more than one monitoring timer instance
            {
                this.timer = new OnClockMinuteTimer();
                this.timer.Start();
                this.timer.Elapsed += this.Timer_Elapsed;
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.IsConfigReloadableSet())
            {
                configReloadable.GetLastestPropertiesValues(this.Section);
            }
        }

        private bool IsConfigReloadableSet()
        {
            if (this.configReloadable != null)
                return true;
            else
            {
                if (!string.IsNullOrWhiteSpace(this.Section.RefreshPluginType))
                {
                    if (this.Section != null)
                    {
                        try
                        {
                            var parts = this.Section.RefreshPluginType.Split(new char[] { ',' });
                            string assemblyName = parts[1].Trim();
                            string typeName = parts[0].Trim();
                            this.configReloadable = (IConfigurationReloadPlugin)Activator.CreateInstance(assemblyName, typeName).Unwrap();
                            this.configReloadable.PropertyLoaded += this.ConfigReloadable_PropertyLoaded;
                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
        }

        private void ConfigReloadable_PropertyLoaded(object sender, Tuple<string, object> property)
        {
            var configurationProperty = new ConfigurationProperty(property.Item1, typeof(string), null);
            this.Section.SetPropertyValue(configurationProperty, property.Item2, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Return 'true'</returns>
        public override bool IsReadOnly()
        {
            return false;
        }
    }
}