using NUnit.Framework;

namespace SM.ProducMgmt.Business.Test
{
    [SetUpFixture]
    public class GlobalSetupFixture
    {
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
        }
    }
}
