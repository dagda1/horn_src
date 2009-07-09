using log4net;
using log4net.Config;
using Rhino.Mocks;

public abstract class Specification : TestBase
{
    protected static readonly ILog log = LogManager.GetLogger(typeof (Specification));

    protected static T CreateStub<T>() where T : class
    {
        return MockRepository.GenerateStub<T>();
    }

    protected static T CreateStub<T>(params object[] param) where T : class
    {
        return MockRepository.GenerateStub<T>(param);
    }

    static Specification()
    {
        XmlConfigurator.Configure();
    }
}