using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace KickAss_Music_Player.BusinessLogic.Services.ApplicationVersion.Tests
{
    [TestClass()]
    public class ApplicationVersionServiceTests
    {
        private readonly IApplicationVersionService _applicationVersionService;
        [OneTimeSetUp]
        public void ClassInit()
        {

        }

        [TearDown]
        public void ClassCleanUp()
        {

        }

        [TestMethod()]
        public void GetApplicationVersionTest()
        {
            Assert.Fail();
        }
    }
}