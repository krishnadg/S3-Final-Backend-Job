using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;


namespace S3ClassLib
{

    // Connectes all steps through the entire backend process of collecting team names of file owners and mapping their total respective storage usage in a given S3 bucket

    public class S3Job
    {
       
        AmazonS3Client client;
        string bucket;

        public S3Job(AmazonS3Client _client, string bucketName)
        {
            client = _client;
            bucket = bucketName;
        }

        public string SetBucket(string newBucketName)
        {
            string oldBucket = bucket;
            bucket = newBucketName;

            return oldBucket;
        }

        public Dictionary<string, long> DoS3Job()
        {
            
             //Gather team names in S3 Bucket
             TeamNameGenerator teamNameGen = new TeamNameGenerator(client, bucket);
             List<string> teamNames = teamNameGen.GetListOfTeamNames();

             //Generate necessary requests to be made to S3Bucket
             ObjectRequestGenerator objRequestGen = new ObjectRequestGenerator(bucket);
             List<ListObjectsRequest> objRequests = objRequestGen.GetObjectRequestList(teamNames);

             //Make said requests and store the corresponding list responses
             ObjectResponseGenerator objResponseGen = new ObjectResponseGenerator(client);
             List<ListObjectsResponse> objResponses = objResponseGen.GetObjectResponseList(objRequests);

             //Parse responses into readable storage container to be sent to frontend workspace
            ObjectResponseParser objResponseParser = new ObjectResponseParser();
            Dictionary<string, long> teamStorageData = objResponseParser.GetDataStructure(objResponses);

            //Print Data to Console
            objResponseParser.PrintData();
            
            return teamStorageData;

        }


    }
}