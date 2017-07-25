using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
namespace S3ClassLib
{
    public class ObjectResponseParser
    {
        //Class for parsing ListObjectResponses data metrics into storage ----
        // k,v = (list team name, total storage)
        Dictionary<string, long> teamsStorage = new Dictionary<string, long>();
        long totalBucketStorage;
        string bucketPrefix;
        public ObjectResponseParser(string _bucketPrefix)
        {
            bucketPrefix = _bucketPrefix;
        }

        private void ParseListObjectResponses(List<ListObjectsResponse> listObjResponses)
        {
            //Change this value based on the name of the bucket stored in, ex "TestBucket/YY_MM_DD/teamname/files", lengthOfPrefixUpToTeamName = 20
            int sizeOfDatePrefix = 10; 
            int lengthOfPrefixUpToTeamName = bucketPrefix.Length + sizeOfDatePrefix ;

            foreach(ListObjectsResponse listResponse in listObjResponses)
            {
                //Team name = Entire prefix (ex. "s3bucket/12_09_12/team1/") minus bucket name ("s3bucket/12_09_12/") leaving team name ("team1/"), minus 1 for '/' leaves "team1"
                string teamName = listResponse.Prefix.Substring(lengthOfPrefixUpToTeamName, listResponse.Prefix.Length - lengthOfPrefixUpToTeamName - 1);
                
                ParseObjectResponse(listResponse, teamName);
            }

        }


        private void ParseObjectResponse(ListObjectsResponse listResponse, string teamName)
        {
            Contract.Requires(listResponse != null);            
            
            foreach (S3Object obj in listResponse.S3Objects)
            {
                ParseSingleObject(obj, teamName);
            }
        }

        private void ParseSingleObject(S3Object obj, string teamName)
        {
                Contract.Requires(obj != null);

                
                long objSize = obj.Size;
                totalBucketStorage += objSize;
                //Add to previously calculated size (value) for specified team (key)
                if (teamsStorage.ContainsKey(teamName))
                {
                    long currentStorage = teamsStorage[teamName];
                    long updatedStorage = currentStorage + objSize;
                    teamsStorage[teamName] = updatedStorage;
                }
                //Add new team (key) with current object's file size (value)
                else
                {
                    teamsStorage.Add(teamName, objSize);
                }
        }


        //Parse the list of object responses, return the data structure
        public Dictionary<string, long> GetDataStructure(List<ListObjectsResponse> listObjResponses)
        {
            ParseListObjectResponses(listObjResponses);
            return teamsStorage;
        }

        //Get Json and put it in S3 Storage...
        public void AddJsonFileToS3(AmazonS3Client client)
        {

            teamsStorage.Add("Total-Storage", totalBucketStorage);

            var sortedTeamStorage = from entry in teamsStorage orderby entry.Value descending select entry;
            string jsonString = JsonConvert.SerializeObject(sortedTeamStorage);

            var currentDateTime = DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year;
            
            var jsonFileKeyPut = "leaderboard/s3.json";


            var jsonFileKeyWithDate = "leaderboard/s3-history/" + currentDateTime.ToString() + ".json";

            PutObjectRequest putJsonRequest = new PutObjectRequest
            {
                BucketName = "datalens-hub",
                Key = jsonFileKeyPut,
                ContentBody = jsonString,
                
            };
            
            PutObjectRequest putJsonRequestWithDate = new PutObjectRequest
            {
                BucketName = "datalens-hub",
                Key = jsonFileKeyWithDate,
                ContentBody = jsonString,
                
            };

            
            
            PutObjectResponse putJsonResponse = client.PutObjectAsync(putJsonRequest).GetAwaiter().GetResult();

            PutObjectResponse putJsonResponse2 = client.PutObjectAsync(putJsonRequestWithDate).GetAwaiter().GetResult();

            
        }


        //For console printing/test purposes only, not meant for deployment
        public void PrintData()
        {
            Console.WriteLine("Total S3 Bucket Storage\n" + totalBucketStorage);

            foreach (KeyValuePair<string, long> teamInfo in teamsStorage)
            {
                Console.WriteLine("Key = {0}, Value = {1}", teamInfo.Key, teamInfo.Value);
            }
        
        }
    }   
}