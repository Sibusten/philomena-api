using System;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sibusten.Philomena.Api;
using Sibusten.Philomena.Api.Models;

namespace Philomena.Api.Tests
{
    [TestClass]
    public class TagTests
    {
        private const string _domain = "https://derpibooru.org";
        private IPhilomenaApi _api;
        private HttpTest _httpTest;

        private const string _tagJson = @"{""tag"":{""aliased_tag"":null,""aliases"":[],""category"":""rating"",""description"":""Pieces of official MLP content, without edits and only reasonable animation loops, are *always* safe.\r\n*Cannot be combined with any other rating.*"",""dnp_entries"":[],""id"":40482,""images"":1624463,""implied_by_tags"":[],""implied_tags"":[],""name"":""safe"",""name_in_namespace"":""safe"",""namespace"":null,""short_description"":""Entirely safe for work and children."",""slug"":""safe"",""spoiler_image_uri"":""https://derpicdn.net/media/2012/07/01/16_01_52_242_safe.png""}}";

        [TestInitialize]
        public void InitializeTest()
        {
            _api = new PhilomenaApi(_domain);
            _httpTest = new HttpTest();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _httpTest.Dispose();
        }

        /// <summary>
        /// Tests getting a tag that exists
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetTag_Exists()
        {
            _httpTest.RespondWith(_tagJson);

            TagModel tag = await _api.GetTagAsync("safe");

            Assert.AreEqual("safe", tag.Slug);
        }

        /// <summary>
        /// Tests getting the safe tag and ensuring all properties are correct
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetTag_Safe()
        {
            _httpTest.RespondWith(_tagJson);

            TagModel tag = await _api.GetTagAsync("safe");

            Assert.IsNull(tag.AliasedTag);
            Assert.IsTrue(tag.Aliases.Count == 0, "Expected tag to have no aliases");
            Assert.AreEqual("rating", tag.Category);
            Assert.IsNotNull(tag.Description);
            Assert.IsTrue(tag.DnpEntries.Count == 0, "Expected tag to have no DNP entries");
            Assert.AreEqual(40482, tag.Id);
            Assert.IsNotNull(tag.Images);
            Assert.IsTrue(tag.ImpliedByTags.Count == 0, "Expected tag to be implied by no other tags");
            Assert.IsTrue(tag.ImpliedTags.Count == 0, "Expected tag to imply no other tags");
            Assert.IsTrue(tag.Images > 0, "Expected image count to be positive");
            Assert.AreEqual("safe", tag.Name);
            Assert.AreEqual("safe", tag.NameInNamespace);
            Assert.IsNull(tag.Namespace);
            Assert.IsNotNull(tag.ShortDescription);
            Assert.AreEqual("safe", tag.Slug);
            Assert.IsNotNull(tag.SpoilerImageUri);
        }

        /// <summary>
        /// Tests getting a tag that does not exist
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetTag_DoesNotExist()
        {
            _httpTest.RespondWith("", 404);

            FlurlHttpException ex = await Assert.ThrowsExceptionAsync<FlurlHttpException>(async () => await _api.GetTagAsync("notarealtag"));

            Assert.AreEqual(404, ex.StatusCode, "Expected a 404 status code");
        }
    }
}
