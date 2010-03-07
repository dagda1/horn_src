using System.Configuration;

namespace Horn.Core.Config
{
    public class HornConfig : ConfigurationSection
    {
        private static readonly HornConfig settings;

        [ConfigurationProperty("hornrootdirectory", IsRequired = true)]
        public string HornRootDirectory
        {
            get { return (string)this["hornrootdirectory"]; }
            set { this["hornrootdirectory"] = value; }
        }

        [ConfigurationProperty("packagetreeuri", IsRequired = true)]
        public string PackageTreeUri
        {
            get { return (string)this["packagetreeuri"]; }
            set { this["packagetreeuri"] = value; }
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