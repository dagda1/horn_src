using System;
using System.Configuration;

namespace Horn.Services.Core.Config
{
    public class HornConfig : ConfigurationSection
    {
        private static readonly HornConfig settings;

        [ConfigurationProperty("buildfrequency", IsRequired = true)]
        public int BuildFrequency
        {
            get { return (int)this["buildfrequency"]; }
            set { this["buildfrequency"] = value; }
        }

        [ConfigurationProperty("dropdirectory", IsRequired = true)]
        public string DropDirectory
        {
            get { return (string)this["dropdirectory"]; }
            set { this["dropdirectory"] = value; }
        }

        [ConfigurationProperty("hornrootdirectory", IsRequired = true)]
        public string HornRootDirectory
        {
            get { return (string)this["hornrootdirectory"]; }
            set { this["hornrootdirectory"] = value; }
        }

        [ConfigurationProperty("horntempdirectory", IsRequired = true)]
        public string HornTempDirectory
        {
            get { return (string)this["horntempdirectory"]; }
            set { this["horntempdirectory"] = value; }
        }

        public static HornConfig Settings
        {
            get
            {
                if (settings == null)
                    return null;

                return settings;
            }
        }

        static HornConfig()
        {
            settings = ConfigurationManager.GetSection("horn") as HornConfig;
        }
    }
}