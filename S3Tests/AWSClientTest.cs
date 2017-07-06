using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using System.Collections.Generic;
namespace S3Tests
{
    public class AWSClientTest
    {
        
        static AmazonS3Client client;
        
        public AWSClientTest()
        {
            var creds = new BasicAWSCredentials("Foo", "Bar");
            var config = new AmazonS3Config();
            config.ServiceURL = "http://localhost:4569";

            client = new AmazonS3Client(creds, config);
            CreateBucket3Teams1FileEach("S3FinalTest1");
        }

        public AWSClientTest(AWSCredentials creds, AmazonS3Config config)
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
        public void CreateEmptyBucket(string bucketName)
        {
            client.PutBucketAsync(bucketName);

        }

        public void CreateBucket1Team1File(string bucketName)
        {
            client.PutBucketAsync(bucketName);
            InitializeFilesInBucket1Team(bucketName);
        }

         private void InitializeFilesInBucket1Team(string bucketName)
        {
            PutSingleFileInBucket(bucketName, "S3Bucket/12_04_13/Team1/smallsizefile");
            
        }


        public void CreateBucket3Teams1FileEach(string bucketName)
        {
            client.PutBucketAsync(bucketName);
            InitializeFilesInBucket1(bucketName);

        }

        

        private void InitializeFilesInBucket1(string bucketName)
        {
            PutSingleFileInBucket(bucketName, "S3Bucket/12_04_13/Team1/smallsizefile", "Content = stuff im saying blah blah");
            PutSingleFileInBucket(bucketName, "S3Bucket/12_04_13/Team2/mediumsizefile");
            PutSingleFileInBucket(bucketName, "S3Bucket/12_04_13/Team3/largesizefile");

        

        }

         public void CreateBucket2Teams3FilesEach(string bucketName)
        {
            client.PutBucketAsync(bucketName);
            InitializeFilesInBucket2(bucketName);

        }

        private void InitializeFilesInBucket2(string bucketName)
        {
            PutSingleFileInBucket(bucketName, "S3Bucket/12_04_13/Team1/smallsizefile");
            PutSingleFileInBucket(bucketName, "S3Bucket/12_04_13/Team1/smallsizefile2");
            PutSingleFileInBucket(bucketName, "S3Bucket/12_04_13/Team1/smallsizefile3");

            PutSingleFileInBucket(bucketName, "S3Bucket/12_04_13/Team2/largesizefile");
            PutSingleFileInBucket(bucketName, "S3Bucket/12_04_13/Team2/largesizefile2");
            PutSingleFileInBucket(bucketName, "S3Bucket/12_04_13/Team2/largesizefile3");

        

        }

         public void CreateBucket4TeamsMultipleFilesEach(string bucketName)
        {
            client.PutBucketAsync(bucketName);
            InitializeFilesInBucket3(bucketName);

        }

        private void InitializeFilesInBucket3(string bucketName)
        {
            PutSingleFileInBucket(bucketName, "S3Bucket/12_04_13/Team1/smallsizefile");
            PutSingleFileInBucket(bucketName, "S3Bucket/12_04_13/Team1/smallsizefile2");
            PutSingleFileInBucket(bucketName, "S3Bucket/12_04_13/Team1/smallsizefile3");

            PutSingleFileInBucket(bucketName, "S3Bucket/12_04_13/Team2/mediumsizefile");
            PutSingleFileInBucket(bucketName, "S3Bucket/12_04_13/Team2/largesizefile");
            PutSingleFileInBucket(bucketName, "S3Bucket/12_04_13/Team2/largesizefile2");
            PutSingleFileInBucket(bucketName, "S3Bucket/12_04_13/Team2/largesizefile3");

            PutSingleFileInBucket(bucketName, "S3Bucket/12_04_13/Team3/smallsizefile");

            PutSingleFileInBucket(bucketName, "S3Bucket/12_04_13/Team4/largesizefile2");
            PutSingleFileInBucket(bucketName, "S3Bucket/12_04_13/Team4/largesizefile3");
            

        }

         private void PutSingleFileInBucket(string bucketName, string keyName)
        {
            PutObjectRequest putRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = keyName,
                 
                 
            };
            
            var response = client.PutObjectAsync(putRequest);
        }

        private void PutSingleFileInBucket(string bucketName, string keyName, string content)
        {
            PutObjectRequest putRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = keyName,
                ContentBody = content
                 
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


        //ListObjectsRequests
        public ListObjectsRequest GetFakeListRequestTeam1(string bucketName)
        {
            return new ListObjectsRequest{
                BucketName = bucketName,
                Prefix = "S3Bucket/12_04_13/Team1/",
                
                };
        }

        public ListObjectsRequest GetFakeListRequestTeam2(string bucketName)
        {
            return new ListObjectsRequest{
                BucketName = bucketName,
                Prefix = "S3Bucket/12_04_13/Team2/",
                };
            
        }
        
        public ListObjectsRequest GetFakeListRequestTeam3(string bucketName)
        {
            return new ListObjectsRequest{
                BucketName = bucketName,
                Prefix = "S3Bucket/12_04_13/Team3/",
                };
        }
 
        public ListObjectsRequest GetFakeListRequestTeam4(string bucketName)
        {
            return new ListObjectsRequest{
                BucketName = bucketName,
                Prefix = "S3Bucket/12_04_13/Team4/",
                }  ;

        }
                
                 




        
        //ListObjectsResponse related methods

        /*Return ListObjectsResponse of static S3Objects */
        public ListObjectsResponse GetFakeListResponseTeam1()
        {

            ListObjectsResponse listResponse = new ListObjectsResponse();

            var list = AddFakeObjectsToListResponse(new List<S3Object>());
            listResponse.S3Objects = list;
            listResponse.Prefix = "S3Bucket/12_04_13/Team1/";

            return listResponse;
        }

        public ListObjectsResponse GetFakeListResponseTeam1_3Files()
        {

            ListObjectsResponse listResponse = new ListObjectsResponse();

            var list = AddFakeObjectsToListResponse1(new List<S3Object>());
            listResponse.S3Objects = list;
            listResponse.Prefix = "S3Bucket/12_04_13/Team1/";

            return listResponse;
        }

        public ListObjectsResponse GetFakeListResponseTeam2()
        {

            ListObjectsResponse listResponse = new ListObjectsResponse();
            

            var list = AddFakeObjectsToListResponse2(new List<S3Object>());
            listResponse.S3Objects = list;
            listResponse.Prefix = "S3Bucket/12_04_13/Team2/";

            return listResponse;
        }

        public ListObjectsResponse GetFakeListResponseTeam3()
        {

            ListObjectsResponse listResponse = new ListObjectsResponse();
            
            var list = AddFakeObjectsToListResponse3(new List<S3Object>());
            listResponse.S3Objects = list;
            listResponse.Prefix = "S3Bucket/12_04_13/Team3/";
            return listResponse;
        }

        public ListObjectsResponse GetFakeListResponseTeam4()
        {

            ListObjectsResponse listResponse = new ListObjectsResponse();
            
            var list = AddFakeObjectsToListResponse4(new List<S3Object>());
            listResponse.S3Objects = list;
            listResponse.Prefix = "S3Bucket/12_04_13/Team4/";
            return listResponse;
        }

        /*Create static instances of S3Objects and add them to returned list */
        public List<S3Object> AddFakeObjectsToListResponse(List<S3Object> objects)
        {
            var s3Object1 = new S3Object();
            s3Object1.Key = "S3Bucket/12_04_13/Team1/smallsizefile";
            s3Object1.Size = 194;
            
            objects.Add(s3Object1);
            

            return objects;
        }

        /*Create static instances of S3Objects and add them to returned list */
        public List<S3Object> AddFakeObjectsToListResponse1(List<S3Object> objects)
        {
            var s3Object1 = new S3Object();
            s3Object1.Key = "S3Bucket/12_04_13/Team1/smallsizefile";
            s3Object1.Size = 200;
            var s3Object2 = new S3Object();
            s3Object2.Key = "S3Bucket/12_04_13/Team1/smallsizefile2";
            s3Object2.Size = 300;
            var s3Object3 = new S3Object();
            s3Object3.Key = "S3Bucket/12_04_13/Team1/smallsizefile3";
            s3Object3.Size = 400;
            
            objects.Add(s3Object1);
            objects.Add(s3Object2);
            objects.Add(s3Object3);
            

            return objects;
        }

        /*Create static instances of S3Objects and add them to returned list */
        public List<S3Object> AddFakeObjectsToListResponse2(List<S3Object> objects)
        {
            var s3Object1 = new S3Object();
            s3Object1.Key = "S3Bucket/12_04_13/Team2/mediumsizefile";
            s3Object1.Size = 52;
            var s3Object2 = new S3Object();
            s3Object2.Key = "S3Bucket/12_04_13/Team2/largesizefile";
            s3Object2.Size = 90;
            var s3Object3 = new S3Object();
            s3Object3.Key = "S3Bucket/12_04_13/Team2/largesizefile2";
            s3Object3.Size = 431;
            var s3Object4 = new S3Object();
            s3Object4.Key = "S3Bucket/12_04_13/Team2/largesizefile3";
            s3Object4.Size = 431;


            objects.Add(s3Object2);
            objects.Add(s3Object3);
            objects.Add(s3Object4);
            objects.Add(s3Object1);


            return objects;
        }

        /*Create static instances of S3Objects and add them to returned list */
          public List<S3Object> AddFakeObjectsToListResponse3(List<S3Object> objects)
        {
            var s3Object1 = new S3Object();
            s3Object1.Key = "S3Bucket/12_04_13/Team3/smallsizefile";
            s3Object1.Size = 100024;
           

            objects.Add(s3Object1);
            

            return objects;
        }

        

        /*Create static instances of S3Objects and add them to returned list */
          public List<S3Object> AddFakeObjectsToListResponse4(List<S3Object> objects)
        {
            var s3Object1 = new S3Object();
            s3Object1.Key = "S3Bucket/12_04_13/Team4/largesizefile2";
            s3Object1.Size = 100024;
            var s3Object2 = new S3Object();
            s3Object2.Key = "S3Bucket/12_04_13/Team4/largesizefile3";
            s3Object2.Size = 100024;

            objects.Add(s3Object1);
            objects.Add(s3Object2);

            return objects;
        }

    }
}