using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Social.Common.BL;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Social.BL
{
    public class PhotoStorage : IPhotosStorage
    {
        private async Task<string> UploadPhoto(byte[] photo)
        {
            using (var s3Client = new AmazonS3Client(RegionEndpoint.EUWest1))
            {
                var request = new PutObjectRequest
                {
                    BucketName = "odedbucket",
                    CannedACL = S3CannedACL.PublicRead,
                    Key = Guid.NewGuid().ToString()
                };
                using (var ms = new MemoryStream(photo))
                {
                    request.InputStream = ms;
                    try
                    {
                        await s3Client.PutObjectAsync(request);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                return "https://s3-eu-west-1.amazonaws.com/odedbucket/" + request.Key;
            }
        }

        public void UploadPhoto(byte[] photo, out string photoURL)
        {
            photoURL = UploadPhoto(photo).Result;
        }
    }
}
