using System;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System.Threading.Tasks;
namespace S3ClassLib
{
    public class AWSSetup 
    {
        static AmazonS3Client client;
        public AWSSetup(AWSCredentials creds, AmazonS3Config config)
        {
          //  config.ServiceURL = "http://localhost:4569";

            client = new AmazonS3Client(creds, config);
 
        }
        public  Task<ListBucketsResponse> GetTaskListBuckets()
        {
            InitializeBucketsAndFiles();            
            var list =   client.ListBucketsAsync();
            
            return list;
        }

        public ListBucketsResponse GetListBuckets()
        {
            var list = GetTaskListBuckets().GetAwaiter().GetResult();
            
            return list;

        }
      

        private void InitializeBucketsAndFiles()
        {
            CreateBucket( "TestBucket 1");
            CreateBucket( "TestBucket 2");
            CreateBucket( "TestBucket 3");

        }

       

        private void CreateBucket(string bucketName)
        {
            client.PutBucketAsync(bucketName);
            InitializeFilesInBucket(bucketName);
        }

          private void InitializeFilesInBucket( string bucketName)
        {
            PutFileInBucket( bucketName, "File 1");
            PutFileInBucket( bucketName, "File 2");
            PutFileInBucket( bucketName, "File 3");

        }

        private void PutFileInBucket( string bucketName, string fileName)
        {
            PutObjectRequest putRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = fileName,
                ContentBody = "stuff blah blah"
                
                
            };
            
            var response = client.PutObjectAsync(putRequest);
        }

        public void PrintListBucketsAndObjectFiles()
        {

            var response =  GetListBuckets();
            var list = response.Buckets;

             foreach (Amazon.S3.Model.S3Bucket bucket in list)
            {
                Console.WriteLine("Bucket Name: " + bucket.BucketName);
                Console.WriteLine("Files info for this Bucket:");
                PrintBucketObjectFiles(bucket.BucketName);
            }
        }

        private void PrintBucketObjectFiles(string bucketName)
        {
            ListObjectsRequest listRequest = new ListObjectsRequest
            {
                BucketName = bucketName,
            };

             
            
            // Get a list of objects
            ListObjectsResponse listResponse = client.ListObjectsAsync(listRequest).GetAwaiter().GetResult();

            foreach (S3Object obj in listResponse.S3Objects)
            {
               Console.WriteLine("Object - " + obj.Key);
               Console.WriteLine(" Size - " + obj.Size);
               Console.WriteLine(); 
               
          //     Console.WriteLine(" LastModified - " + obj.LastModified);
          //     Console.WriteLine(" Storage class - " + obj.StorageClass);
            }
        }

       
    }
}