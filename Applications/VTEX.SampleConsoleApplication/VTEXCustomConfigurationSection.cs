﻿using System.Configuration;
using VTEX.Configuration.DynamicSection;

namespace VTEX.Configuration.DynamicSection.SampleConsoleApplication
{
    /// <summary>
    /// Define a custom configuration section with two properties.
    /// </summary>
    public class VTEXCustomConfigurationSection : DynamicConfigurationSection
    {
        /// <summary>
        /// Get or set changeble test property
        /// </summary>
        [ConfigurationProperty("testValue", IsRequired = false)]
        public string TestValue
        {
            get
            {
                return this["testValue"].ToString();
            }
            set
            {
                this["testValue"] = value;
            }
        }

        /// <summary>
        /// Get or set fixed test property
        /// </summary>
        [ConfigurationProperty("otherValue", IsRequired = false)]
        public string OtherValue
        {
            get
            {
                return this["otherValue"].ToString();
            }
            set
            {
                this["otherValue"] = value;
            }
        }
    }
}