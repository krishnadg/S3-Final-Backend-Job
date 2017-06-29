using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using System.Collections.Generic;

namespace S3ClassLib
{
    public class ObjectResponseParser
    {
        //Class for parsing ListObjectResponses data metrics into storage ----
        // k,v = (list team name, total storage)
        Dictionary<string, long> teamsStorage = new Dictionary<string, long>();
        public ObjectResponseParser()
        {
            
        }

        private void ParseListObjectResponses(List<ListObjectsResponse> listObjResponses)
        {
            //Change this value based on the name of the bucket stored in, ex "TestBucket/teamname/files", lengthOfBucketname = 11
            int lengthOfBucketname = 9;

            foreach(ListObjectsResponse listResponse in listObjResponses)
            {
                //Team name = Entire prefix (ex. "s3bucket/team1/") minus bucket name ("s3bucket/") leaving team name ("team1/"), minus 1 for '/' leaves "team1"
                string teamName = listResponse.Prefix.Substring(lengthOfBucketname, listResponse.Prefix.Length - lengthOfBucketname - 1);
                
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


        //For console printing/test purposes only, not meant for deployment
        public void PrintData()
        {
            foreach (KeyValuePair<string, long> teamInfo in teamsStorage)
            {
                Console.WriteLine("Key = {0}, Value = {1}", teamInfo.Key, teamInfo.Value);
            }
        
        }
    }   
}