using System.Configuration;

namespace Horn.Console.Config
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