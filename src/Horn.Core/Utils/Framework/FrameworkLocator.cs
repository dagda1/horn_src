namespace Horn.Core.Utils.Framework
{
    public sealed class FrameworkLocator
    {

        private static readonly FrameworkLocator instance = new FrameworkLocator();


        public static FrameworkLocator Instance
        {
            get
            {
                return instance;
            }
        }



        public Framework this[FrameworkVersion version]
        {
            get
            {
                return new Framework(version);
            }
        }



    }
}