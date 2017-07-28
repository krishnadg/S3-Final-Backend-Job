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

        //Does S3 job, returns dictionary of teamname, storage value - to be sent to json constructor for further readability on front end
        public Dictionary<string, long> DoS3Job()
        {

            //Gather all prefixes that are needed to find all team names
            SearchPrefixGenerator searchPrefixGen = new SearchPrefixGenerator(client, bucket);
            List<string> bucketPrefixList = searchPrefixGen.GetListOfPrefixes(bucketPrefix);
            Console.WriteLine("\nNumber of unique days teams' logs are being stored/number of initial requests being made to find all teams: " + bucketPrefixList.Count);

            //Gather team names in S3 Bucket
            TeamNameGenerator teamNameGen = new TeamNameGenerator(client, bucket);
            Dictionary<string, List<string>> teamNames = teamNameGen.GetListOfTeamNames(bucketPrefixList);

            //Generate necessary requests to be made to S3Bucket
            ObjectRequestGenerator objRequestGen = new ObjectRequestGenerator(bucket);
            List<ListObjectsRequest> objRequests = objRequestGen.GetObjectRequestList(teamNames);
            Console.WriteLine( objRequests.Count + " \nListObjectRequests Generated...\n");
            
            //Make said requests and store the corresponding list responses
            ObjectResponseGenerator objResponseGen = new ObjectResponseGenerator(client);
            Console.WriteLine("\nMaking Requests\n");

            List<ListObjectsResponse> objResponses = objResponseGen.GetObjectResponseList(objRequests);
            Console.WriteLine("\n\nJOB TOTAL NUMBER OF LIST OBJECTS REQUESTS: " + objResponses.Count );
            
            //Parse responses into readable storage container to be sent to frontend workspace
            ObjectResponseParser objResponseParser = new ObjectResponseParser(bucketPrefix);
            Dictionary<string, long> teamStorageData = objResponseParser.GetDataStructure(objResponses);
            Console.WriteLine("\nResponses Parsed\n");


            JsonFileConstructor jsonConstructor = new JsonFileConstructor();
            jsonConstructor.AddJsonFileToS3(client, teamStorageData);
            Console.WriteLine("\nJson uploaded to s3...");
            //Print Data to Console
            objResponseParser.PrintData();
            
            return teamStorageData;
        }


    }
}