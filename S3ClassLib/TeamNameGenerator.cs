using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using System.Collections.Generic;

namespace S3ClassLib
{
    /*Class used to sort through entire bucket to gather/store all unique team names in a list*/
    public class TeamNameGenerator 
    {
        
        AmazonS3Client client;
        List<string> teamNames = new List<string>();

        List<ListObjectsRequest> objRequests = new List<ListObjectsRequest>();
        string bucket;
        public TeamNameGenerator()
        {
            
        }

        public TeamNameGenerator(string bucketName)
        {
            bucket = bucketName;
        }

         public TeamNameGenerator(AmazonS3Client _client, string bucketName)
        {
            client = _client;
            bucket = bucketName;
        }


        /*Methods for adding all unique team names to list */


        //Search through entire bucket and parse files into all team names
        public void GatherTeamNames()
        {
            //Create request to return ALL teams and their files in bucket(hopefully)
            ListObjectsRequest listRequest = new ListObjectsRequest
            {
                Prefix = "S3Bucket/",
                Delimiter = "/",
                BucketName = bucket
            };
            ProcessListRequestIntoTeams(listRequest);
        }

        private void ProcessListRequestIntoTeams(ListObjectsRequest listRequest)
        {
            ListObjectsResponse listResponse;
            
            do
            {
                // Get listResponse for up to 1000 files after marker
                listResponse = client.ListObjectsAsync(listRequest).GetAwaiter().GetResult();
                //Exit if null, no files in bucket
                if (listResponse is null)
                {
                    return;
                }
                //Add in each unique team name
                foreach (string commonPrefix in listResponse.CommonPrefixes)
                {
                    //commonPrefix is formatted into entire path ("S3Bucket/Team1/"), so truncate by subtracting the prefix length and final '/' character
                    string formattedTeamName = commonPrefix.Substring(listResponse.Prefix.Length, commonPrefix.Length - listResponse.Prefix.Length - 1);

                    //Check if it already exists, if not add it
                    if (!teamNames.Contains(formattedTeamName))
                        teamNames.Add(formattedTeamName);
                }
 
            // Set the marker property
             listRequest.Marker = listResponse.NextMarker;
            } while (listResponse.IsTruncated);
        }

        public List<string> GetListOfTeamNames()
        {
            GatherTeamNames();
            return teamNames;
        }

    }

}