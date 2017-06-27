using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using System.Collections.Generic;
namespace S3ClassLib
{
    public class AWSTestClient
    {
        
        static AmazonS3Client client;
        
        public AWSTestClient()
        {
            var creds = new BasicAWSCredentials("Foo", "Bar");
            var config = new AmazonS3Config();
            config.ServiceURL = "http://localhost:4569";

            client = new AmazonS3Client(creds, config);

        }

        public AWSTestClient(AWSCredentials creds, AmazonS3Config config)
        {
            config.ServiceURL = "http://localhost:4569";
            client = new AmazonS3Client(creds, config);
 
        }

        public AmazonS3Client GetClient()
        {
            return client;
        }




        //Sample S3Bucket initialization, instantiation, accessor, and teardown

        public S3Bucket GetBucket()
        {
            var response = client.ListBucketsAsync().GetAwaiter().GetResult();
            if (response.Buckets != null)
            {
                return response.Buckets[0];
            }
            else 
            {
                return new S3Bucket();
            }
        }

        //EMPTY FOR TEST
        public void CreateBucket0(string bucketName)
        {
            client.PutBucketAsync(bucketName);

        }


        public void CreateBucket1(string bucketName)
        {
            client.PutBucketAsync(bucketName);
            InitializeFilesInBucket1(bucketName);

        }

        private void InitializeFilesInBucket1(string bucketName)
        {
            PutSingleFileInBucket(bucketName, "S3Bucket/Team1/smallsizefile");
            PutSingleFileInBucket(bucketName, "S3Bucket/Team2/mediumsizefile");
            PutSingleFileInBucket(bucketName, "S3Bucket/Team3/largesizefile");

        

        }

         public void CreateBucket2(string bucketName)
        {
            client.PutBucketAsync(bucketName);
            InitializeFilesInBucket2(bucketName);

        }

        private void InitializeFilesInBucket2(string bucketName)
        {
            PutSingleFileInBucket(bucketName, "S3Bucket/Team1/smallsizefile");
            PutSingleFileInBucket(bucketName, "S3Bucket/Team1/smallsizefile2");
            PutSingleFileInBucket(bucketName, "S3Bucket/Team1/smallsizefile3");

            PutSingleFileInBucket(bucketName, "S3Bucket/Team2/mediumsizefile");
            PutSingleFileInBucket(bucketName, "S3Bucket/Team2/largesizefile");
            PutSingleFileInBucket(bucketName, "S3Bucket/Team2/largesizefile2");

        

        }

         public void CreateBucket3(string bucketName)
        {
            client.PutBucketAsync(bucketName);
            InitializeFilesInBucket3(bucketName);

        }

        private void InitializeFilesInBucket3(string bucketName)
        {
            PutSingleFileInBucket(bucketName, "S3Bucket/Team1/smallsizefile");
            PutSingleFileInBucket(bucketName, "S3Bucket/Team1/smallsizefile2");
            PutSingleFileInBucket(bucketName, "S3Bucket/Team1/smallsizefile3");

            PutSingleFileInBucket(bucketName, "S3Bucket/Team2/mediumsizefile");
            PutSingleFileInBucket(bucketName, "S3Bucket/Team2/largesizefile");
            PutSingleFileInBucket(bucketName, "S3Bucket/Team2/largesizefile2");
            PutSingleFileInBucket(bucketName, "S3Bucket/Team2/largesizefile3");

            PutSingleFileInBucket(bucketName, "S3Bucket/Team3/smallsizefile");

            PutSingleFileInBucket(bucketName, "S3Bucket/Team4/largesizefile2");
            PutSingleFileInBucket(bucketName, "S3Bucket/Team4/largesizefile3");
            

        }

         private void PutSingleFileInBucket(string bucketName, string fileName)
        {
            PutObjectRequest putRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = fileName,
            };
            
            var response = client.PutObjectAsync(putRequest);
        }

        //Delete bucket from fake_s3, not sure if it's working though...
        public void RemoveBucketFromS3(string bucketName)
        {
            var deleteRequest = new DeleteBucketRequest
            {
                BucketName = bucketName,
                UseClientRegion = true
            };
            client.DeleteBucketAsync(deleteRequest);
        }



        //ListObjectsRequest related methods

        /*Return ListObjectsResponse of static prefix/delimeter
        
        Note: Eventually add string prefix and delimeter as formal parameters here
        */
/*        public ListObjectsRequest GetListRequest(string bucketName)
        {
            ListObjectsRequest listRequest = new ListObjectsRequest();
            listRequest.Prefix = "S3Bucket/";
            listRequest.Delimiter = "/";
            listRequest.BucketName = bucketName;
            
            return listRequest;
        }
*/





        //ListObjectsResponse related methods

        /*Return ListObjectsResponse of static S3Objects */
        public ListObjectsResponse GetListResponse()
        {

            ListObjectsResponse listResponse = new ListObjectsResponse();
            listResponse.Prefix = "S3Bucket/Team1";
            listResponse.Delimiter = "Team1/";

            var list = AddObjectsToListResponse(new List<S3Object>());
            listResponse.S3Objects = list;
            return listResponse;
        }

        public ListObjectsResponse GetListResponse2()
        {

            ListObjectsResponse listResponse = new ListObjectsResponse();
            listResponse.Prefix = "S3Bucket/";
            listResponse.Delimiter = "/";

            var list = AddObjectsToListResponse2(new List<S3Object>());
            listResponse.S3Objects = list;
            return listResponse;
        }

        public ListObjectsResponse GetListResponse3()
        {

            ListObjectsResponse listResponse = new ListObjectsResponse();
            listResponse.Prefix = "S3Bucket/";
            listResponse.Delimiter = "/";

            var list = AddObjectsToListResponse3(new List<S3Object>());
            listResponse.S3Objects = list;
            return listResponse;
        }

        /*Create static instances of S3Objects and add them to returned list */
        public List<S3Object> AddObjectsToListResponse(List<S3Object> objects)
        {
            var s3Object1 = new S3Object();
            s3Object1.Key = "S3Bucket/Team1/file1";
            s3Object1.Size = 52;
            var s3Object2 = new S3Object();
            s3Object2.Key = "S3Bucket/Team1/file2";
            s3Object2.Size = 90;
            var s3Object3 = new S3Object();
            s3Object3.Key = "S3Bucket/Team1/file3";
            s3Object3.Size = 431;

            objects.Add(s3Object1);
            objects.Add(s3Object2);
            objects.Add(s3Object3);

            return objects;
        }

        /*Create static instances of S3Objects and add them to returned list */
        public List<S3Object> AddObjectsToListResponse2(List<S3Object> objects)
        {
            var s3Object1 = new S3Object();
            s3Object1.Key = "S3Bucket/Team1/file1";
            s3Object1.Size = 52;
            var s3Object2 = new S3Object();
            s3Object2.Key = "S3Bucket/Team2/file1";
            s3Object2.Size = 90;
            var s3Object3 = new S3Object();
            s3Object3.Key = "S3Bucket/Team2/file2";
            s3Object3.Size = 431;

            objects.Add(s3Object1);
            objects.Add(s3Object2);
            objects.Add(s3Object3);

            return objects;
        }

        /*Create static instances of S3Objects and add them to returned list */
          public List<S3Object> AddObjectsToListResponse3(List<S3Object> objects)
        {
            var s3Object1 = new S3Object();
            s3Object1.Key = "S3Bucket/Team1/file1";
            s3Object1.Size = 52;
            var s3Object2 = new S3Object();
            s3Object2.Key = "S3Bucket/Team2/file1";
            s3Object2.Size = 90;
            var s3Object3 = new S3Object();
            s3Object3.Key = "S3Bucket/Team3/file1";
            s3Object3.Size = 431;

            objects.Add(s3Object1);
            objects.Add(s3Object2);
            objects.Add(s3Object3);

            return objects;
        }

    }
}