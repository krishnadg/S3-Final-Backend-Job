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
        string bucketPrefix;

        public S3Job(AmazonS3Client _client, string bucketName, string _bucketPrefix)
        {
            client = _client;
            bucket = bucketName;
            bucketPrefix = _bucketPrefix;
        }

        public string SetBucket(string newBucketName)
        {
            string oldBucket = bucket;
            bucket = newBucketName;

            return oldBucket;
        }

        public Dictionary<string, long> DoS3Job()
        {

            //Gather all prefixes that are needed to find all team names
            SearchPrefixGenerator searchPrefixGen = new SearchPrefixGenerator(client, bucket);
            List<string> bucketPrefixList = searchPrefixGen.GetListOfPrefixes(bucketPrefix);

            //Gather team names in S3 Bucket
            TeamNameGenerator teamNameGen = new TeamNameGenerator(client, bucket);
            Dictionary<string, List<string>> teamNames = teamNameGen.GetListOfTeamNames(bucketPrefixList);

            //Generate necessary requests to be made to S3Bucket
            ObjectRequestGenerator objRequestGen = new ObjectRequestGenerator(bucket);
            List<ListObjectsRequest> objRequests = objRequestGen.GetObjectRequestList(teamNames);
            Console.WriteLine( objRequests.Count + " ListObjectRequests Generated...");

            
            //Make said requests and store the corresponding list responses
            ObjectResponseGenerator objResponseGen = new ObjectResponseGenerator(client);
            Console.WriteLine("Making Requests");

            List<ListObjectsResponse> objResponses = objResponseGen.GetObjectResponseList(objRequests);
            Console.WriteLine(objResponses.Count + "Responses Obtained");
            
            //Parse responses into readable storage container to be sent to frontend workspace
            ObjectResponseParser objResponseParser = new ObjectResponseParser(bucketPrefix);
            Dictionary<string, long> teamStorageData = objResponseParser.GetDataStructure(objResponses);
            Console.WriteLine("Responses Parsed");
            //Print Data to Console
            objResponseParser.PrintData();
            
            return teamStorageData;
            

            //return new Dictionary<string, long>();

        }


    }
}