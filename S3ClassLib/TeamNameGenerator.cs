using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using System.Collections.Generic;

namespace S3ClassLib
{
    /*Class used to sort through all files with given list of common prefixes to gather/store all unique team names and their respective common prefixes in a dictionary*/
    public class TeamNameGenerator 
    {
        
        AmazonS3Client client;
        //List<string> teamNames = new List<string>();

        Dictionary<string, List<string>> teamNamesAndPrefixes = new Dictionary<string, List<string>>();

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
        public void GatherTeamNames(List<string> bucketPrefixesList)
        {

            foreach (string bucketPrefix in bucketPrefixesList)
            {
                 ListObjectsRequest listRequest = new ListObjectsRequest
                {
                    Prefix = bucketPrefix,
                    Delimiter = "/",
                    BucketName = bucket
                };
                ProcessListRequestIntoTeams(listRequest);
            }

            //Create request to return ALL files with date still attached
           
        }



        //Adds on the 
        private void ProcessListRequestIntoTeams(ListObjectsRequest listRequest)
        {
            ListObjectsResponse listResponse;
            
            do
            {
                // Get listResponse for up to 1000 files after marker
                listResponse = client.ListObjectsAsync(listRequest).GetAwaiter().GetResult();
                //Exit if null, no files in bucket
                if (listResponse.CommonPrefixes == null || listResponse.CommonPrefixes.Count == 0)
                {
                    return;
                }
                //Add in each unique team name
                foreach (string commonPrefix in listResponse.CommonPrefixes)
                {
                    //commonPrefix is formatted into entire path ("S3Bucket/17_04_12/Team1/"), so truncate by subtracting the prefix length and final '/' character leaving "Team1
                    string formattedTeamName = commonPrefix.Substring(listResponse.Prefix.Length, commonPrefix.Length - listResponse.Prefix.Length - 1);


                    //Check if this team name is already in our dictionary; if so add the team name and its common prefix 
                    if (!teamNamesAndPrefixes.ContainsKey(formattedTeamName))
                    {
                        teamNamesAndPrefixes.Add(formattedTeamName, new List<string>{listResponse.Prefix});
                    }
                    //If not, get the list and add this new common prefix (for this team name) to it
                    else
                    {
                        //Get list, add new common prefix if it't not already in there
                        var currentTeamNamePrefixList = teamNamesAndPrefixes[formattedTeamName];
                        if (!currentTeamNamePrefixList.Contains(listResponse.Prefix))
                            currentTeamNamePrefixList.Add(listResponse.Prefix);

                        //Add it back in to our Dict
                        teamNamesAndPrefixes.Add(formattedTeamName, currentTeamNamePrefixList);
                        
                    }

                }
 
            // Set the marker property
             listRequest.Marker = listResponse.NextMarker;
            } while (listResponse.IsTruncated);
        }

        public Dictionary<string, List<string>> GetListOfTeamNames(List<string> bucketPrefixesList)
        {
            GatherTeamNames(bucketPrefixesList);
            return teamNamesAndPrefixes;
        }

    }

}