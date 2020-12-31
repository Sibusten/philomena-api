using Flurl.Http.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sibusten.Philomena.Api.Tests
{
    public abstract class TestRoot
    {
        protected const string _domain = "https://derpibooru.org";
        protected IPhilomenaApi _api;
        protected HttpTest _httpTest;

        [TestInitialize]
        public virtual void InitializeTest()
        {
            _api = new PhilomenaApi(_domain);
            _httpTest = new HttpTest();
        }

        [TestCleanup]
        public virtual void CleanupTest()
        {
            _httpTest.Dispose();
        }
    }
}
