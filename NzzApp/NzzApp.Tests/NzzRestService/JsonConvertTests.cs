using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Newtonsoft.Json;
using NzzApp.Services.Responses.Articles;

namespace NzzApp.Tests.NzzRestService
{
    [TestClass]
    public class JsonConvertTests
    {
        [TestMethod]
        public void TestCorruptBox()
        {
            var converted = JsonConvert.DeserializeObject<FullArticleResponse>(JsonData.BoxCorrupt);
        }

        [TestMethod]
        public void TestDefaultBox()
        {
            var converted = JsonConvert.DeserializeObject<FullArticleResponse>(JsonData.BoxDefault);
        }
    }
}
