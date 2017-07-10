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
        
        List<ListObjectsRequest> objRequests = new List<ListObjectsRequest>();
        string bucket;
        public ObjectRequestGenerator()
        {
            
        }

        public ObjectRequestGenerator(string bucketName)
        {
            bucket = bucketName;
        }

        

        //Process list of requests and then make them..
        private void ProcessListIntoRequests(Dictionary<string, List<string>> teamNamesPrefixes) //(Dictionary<string, List<string>>)
        {
            int teamsAnalyzed = 0;
            //Go through each pair of team name, list of prefixes, generate all corresponding requests 
            foreach (KeyValuePair<string, List<string>> teamNameWithListPrefixes in teamNamesPrefixes)
            {
               GetAllRequestsForSingleTeam(teamNameWithListPrefixes);
               teamsAnalyzed ++;

               
            }
            
            
         
            
        }

        private void GetAllRequestsForSingleTeam(KeyValuePair<string, List<string>> teamNameWithListPrefixes)
        {
            ListObjectsRequest listRequest;

            string teamName = teamNameWithListPrefixes.Key;


            
            foreach(string teamPrefix in teamNameWithListPrefixes.Value)
            {
                listRequest = new ListObjectsRequest
                {
                    BucketName = bucket,
                    Prefix = teamPrefix + teamName + "/",
                };

                objRequests.Add(listRequest);

            }
            


        }

        public List<ListObjectsRequest> GetObjectRequestList(Dictionary<string, List<string>> teamNamesPrefixes)
        {
            ProcessListIntoRequests(teamNamesPrefixes);

            return objRequests;
        }

    }
}