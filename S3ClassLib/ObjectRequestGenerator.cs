using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using System.Collections.Generic;

namespace S3ClassLib
{
    /*Class used to generate list of ListObjectRequests based on list of team names obtained previously*/
    public class ObjectRequestGenerator 
    {
        
        AmazonS3Client client;
        List<ListObjectsRequest> objRequests = new List<ListObjectsRequest>();
        string bucket;
        public ObjectRequestGenerator()
        {
            
        }

        public ObjectRequestGenerator(string bucketName)
        {
            bucket = bucketName;
        }

         public ObjectRequestGenerator(AmazonS3Client _client, string bucketName)
        {
            client = _client;
            bucket = bucketName;
        }

        //Process list of requests and then make them..
        private void ProcessListIntoRequests(List<string> teamNames)
        {
            ListObjectsRequest listRequest;
            
            
            foreach (string team in teamNames)
            {
                listRequest = new ListObjectsRequest
                {
                    BucketName = bucket,
                    Prefix = "S3Bucket/",
                    Delimiter = team + "/"
                };

                objRequests.Add(listRequest);
            }



        }

        public List<ListObjectsRequest> GetObjectRequestList(List<string> teamNames)
        {
            ProcessListIntoRequests(teamNames);

            return objRequests;
        }

    }
}