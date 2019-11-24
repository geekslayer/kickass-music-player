using KickAss_Music_Player.BusinessLogic.Services.ApplicationVersion;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;

namespace KickAss_Music_Player.BusinessLogicTests.Services.ApplicationVersion
{
    [TestFixture]
    public class ApplicationVersionServiceTests
    {
        private IApplicationVersionService _applicationVersionService;
        private ServiceProvider serviceProvider { get; set; }

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddTransient<IApplicationVersionService, ApplicationVersionService>();
            serviceProvider = services.BuildServiceProvider();

            _applicationVersionService = serviceProvider.GetService<IApplicationVersionService>();
        }

        [TearDown]
        public void ClassCleanUp()
        {
            _applicationVersionService = null;
        }

        [TestCase("0.1.0", true)]
        [TestCase("0.1.1", false)]
        [TestCase("0.2.0", false)]
        [TestCase("0.3.0", false)]
        [TestCase("0.4.0", false)]
        [TestCase("0.4.2", false)]
        public void GetApplicationVersionTest(string versionValue, bool shouldMatch)
        {
            var versionString = _applicationVersionService.GetApplicationVersion().Result;

            Assert.AreEqual(shouldMatch, Convert.ToString(versionString.Data) == versionValue);
        }
    }
}