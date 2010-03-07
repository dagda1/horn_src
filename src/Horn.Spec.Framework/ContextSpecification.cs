using log4net;
using log4net.Config;
using NUnit.Framework;

namespace Horn.Spec.Framework
{
    [TestFixture]
    public abstract class ContextSpecification
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(ContextSpecification));

        protected abstract void because();

        protected abstract void establish_context();

        [SetUp]
        public void setup()
        {
            establish_context();
            because();
        }

        [TearDown]
        public void teardown()
        {
            after_each_specification();
        }

        protected virtual void after_each_specification()
        {
        }

        [TestFixtureTearDown]
        public virtual void after_each_Spec()
        {
        }
        [TestFixtureSetUp]
        public virtual void before_each_spec()
        {
            XmlConfigurator.Configure();
        }
    }
}