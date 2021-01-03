using System.Security.Cryptography;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sibusten.Philomena.Api.Models;

namespace Sibusten.Philomena.Api.Tests
{
    [TestClass]
    public class ImageTests : TestRoot
    {
        private const double _durationIfNotAnimated = 0.04;

        private const string _firstImageJson = "{\"image\":{\"mime_type\":\"image/jpeg\",\"spoilered\":false,\"name\":\"0__derpy-hooves_j-period--jonah-jameson_ponified\",\"first_seen_at\":\"2012-01-02T03:10:59\",\"updated_at\":\"2019-12-20T10:07:20\",\"tag_ids\":[445,18877,20246,21896,22633,23102,24899,26775,27141,29280,30084,30840,31285,31344,32938,33802,33855,33983,34427,35386,35781,37063,37140,37319,37536,38172,38185,40482,41830,42127,42666,42939,43863,61921,82531,182100,190054,321220],\"format\":\"jpg\",\"animated\":false,\"representations\":{\"full\":\"https://derpicdn.net/img/view/2012/1/2/0.jpg\",\"large\":\"https://derpicdn.net/img/2012/1/2/0/large.jpg\",\"medium\":\"https://derpicdn.net/img/2012/1/2/0/medium.jpg\",\"small\":\"https://derpicdn.net/img/2012/1/2/0/small.jpg\",\"tall\":\"https://derpicdn.net/img/2012/1/2/0/tall.jpg\",\"thumb\":\"https://derpicdn.net/img/2012/1/2/0/thumb.jpg\",\"thumb_small\":\"https://derpicdn.net/img/2012/1/2/0/thumb_small.jpg\",\"thumb_tiny\":\"https://derpicdn.net/img/2012/1/2/0/thumb_tiny.jpg\"},\"aspect_ratio\":1.1428571428571428,\"duplicate_of\":null,\"hidden_from_users\":false,\"tags\":[\"adventure in the comments\",\"artist needed\",\"bag\",\"building\",\"chair\",\"cigar\",\"derpy hooves\",\"eyes\",\"female\",\"grin\",\"hilarious in hindsight\",\"image macro\",\"it begins\",\"j. jonah jameson\",\"letter\",\"mail\",\"male\",\"mare\",\"meme\",\"muffin\",\"necktie\",\"paper\",\"parody\",\"pegasus\",\"phone\",\"ponified\",\"pony\",\"safe\",\"sitting\",\"smoking\",\"spider-man\",\"stallion\",\"swinging\",\"gritted teeth\",\"featured image\",\"smiling\",\"song in the comments\",\"derpibooru legacy\"],\"comment_count\":2932,\"created_at\":\"2012-01-02T03:10:59\",\"description\":\"Tags locked because people apparently need ten different ways to say this is the first image and five different ways to say things about comments.\",\"orig_sha512_hash\":\"85a930032f2d668417c5c6a8fb7d999ecdc94e3d136e42110cebda365214370f92510c3eb79310acec732e7af185ae78a623da71df24bb5c4ef4aefc186aa110\",\"duration\":0.04,\"size\":134173,\"intensities\":{\"ne\":36.42730257666667,\"nw\":23.869670415714285,\"se\":43.22466028428571,\"sw\":32.33743947190476},\"upvotes\":3775,\"thumbnails_generated\":true,\"uploader\":\"Carcer\",\"downvotes\":80,\"score\":3695,\"wilson_score\":0.972458254851847,\"processed\":true,\"sha512_hash\":\"85a930032f2d668417c5c6a8fb7d999ecdc94e3d136e42110cebda365214370f92510c3eb79310acec732e7af185ae78a623da71df24bb5c4ef4aefc186aa110\",\"id\":0,\"view_url\":\"https://derpicdn.net/img/view/2012/1/2/0__artist+needed_safe_derpy+hooves_adventure+in+the+comments_bag_building_chair_cigar_derpibooru+legacy_eyes_featured+image_female_grin_gritted+teeth_h.jpg\",\"faves\":2344,\"tag_count\":38,\"height\":700,\"width\":800,\"uploader_id\":211190,\"source_url\":\"\",\"deletion_reason\":null},\"interactions\":[]}";

        private void ValidateSha512Hash(string hash)
        {
            Assert.IsNotNull(hash);

            Assert.AreEqual(128, hash.Length, "SHA512 hash should be 128 hex characters long (64 bytes, 512 bits)");
            foreach (char c in hash)
            {
                bool isHexCharacter = c
                    is >= '0' and <= '9'
                    or >= 'a' and <= 'f'
                    or >= 'A' and <= 'F';

                Assert.IsTrue(isHexCharacter, "The hex string should contain only hex characters");
            }
        }

        [TestMethod]
        public async Task Test_GetImage_FirstImage()
        {
            _httpTest.RespondWith(_firstImageJson);

            // Get the image response
            ImageResponseModel imageResponse = await _api.GetImage(0);
            Assert.IsNotNull(imageResponse);

            // Ensure interactions are present, but empty
            Assert.IsNotNull(imageResponse.Interactions);
            Assert.IsFalse(imageResponse.Interactions.Any(), "Image should have no interactions without an api key");

            // Ensure the image is present and has the correct ID
            Assert.IsNotNull(imageResponse.Image);
            ImageModel image = imageResponse.Image;
            Assert.IsNotNull(image.Id);
            Assert.AreEqual(0, image.Id);

            // Ensure image is not marked as animated
            Assert.IsNotNull(image.IsAnimated);
            Assert.IsFalse(image.IsAnimated.Value, "Image #0 is not animated");
            Assert.IsNotNull(image.Duration);
            Assert.AreEqual(_durationIfNotAnimated, image.Duration);

            // Ensure image is visible and not marked as duplicate or deleted
            Assert.IsNotNull(image.IsHiddenFromUsers);
            Assert.IsFalse(image.IsHiddenFromUsers.Value, "Image #0 is not hidden");
            Assert.IsNull(image.DuplicateOf);
            Assert.IsNull(image.DeletionReason);

            // Ensure the image is processed
            Assert.IsNotNull(image.Processed);
            Assert.IsTrue(image.Processed.Value, "Image #0 should be processed");
            Assert.IsNotNull(image.ThumbnailsGenerated);
            Assert.IsTrue(image.ThumbnailsGenerated.Value, "Image #0 should have thumbnails generated");

            // Verify the image hashes
            ValidateSha512Hash(image.OrigSha512Hash);
            ValidateSha512Hash(image.Sha512Hash);

            // Ensure other image properties are present
            Assert.IsNotNull(image.AspectRatio);
            Assert.IsNotNull(image.CommentCount);
            Assert.IsNotNull(image.CreatedAt);
            Assert.IsNotNull(image.Downvotes);
            Assert.IsNotNull(image.Faves);
            Assert.IsNotNull(image.FirstSeenAt);
            Assert.IsNotNull(image.Format);
            Assert.IsNotNull(image.Height);
            Assert.IsNotNull(image.IsSpoilered);
            Assert.IsNotNull(image.MimeType);
            Assert.IsNotNull(image.Score);
            Assert.IsNotNull(image.Size);
            Assert.IsNotNull(image.UpdatedAt);
            Assert.IsNotNull(image.Uploader);
            Assert.IsNotNull(image.UploaderId);
            Assert.IsNotNull(image.Upvotes);
            Assert.IsNotNull(image.ViewUrl);
            Assert.IsNotNull(image.Width);
            Assert.IsNotNull(image.WilsonScore);

            // Ensure the image is tagged
            Assert.IsNotNull(image.TagCount);
            Assert.IsTrue(image.TagCount > 0, "The image should have tags");
            Assert.IsNotNull(image.TagIds);
            Assert.AreEqual(image.TagCount, image.TagIds.Count);
            Assert.IsNotNull(image.Tags);
            Assert.AreEqual(image.TagCount, image.Tags.Count);

            // Ensure all representations are present
            Assert.IsNotNull(image.Representations);
            RepresentationsModel representations = image.Representations;
            Assert.IsNotNull(representations.Full);
            Assert.IsNotNull(representations.Large);
            Assert.IsNotNull(representations.Medium);
            Assert.IsNotNull(representations.Small);
            Assert.IsNotNull(representations.Tall);
            Assert.IsNotNull(representations.Thumb);
            Assert.IsNotNull(representations.ThumbSmall);
            Assert.IsNotNull(representations.ThumbTiny);
        }
    }
}
