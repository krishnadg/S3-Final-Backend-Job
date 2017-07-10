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
    public class SearchPrefixGenerator 
    {
        
        AmazonS3Client client;
        List<string> preTeamPrefixes = new List<string>();

        

        string bucket;
        public SearchPrefixGenerator()
        {
            
        }

        public SearchPrefixGenerator(string bucketName)
        {
            bucket = bucketName;
        }

        public SearchPrefixGenerator(AmazonS3Client _client, string bucketName)
        {
            client = _client;
            bucket = bucketName;
        }


        /*Methods for adding all unique team names to list */


        //Search through entire bucket and parse files into all pre-teamname prefixes
        private void GatherPreTeamPrefixes(string bucketPrefix)
        {
            //Create request to return ALL files with date attached 
            ListObjectsRequest listRequest = new ListObjectsRequest
            {
                Prefix = bucketPrefix + "/",
                Delimiter = "/",
                BucketName = bucket
            };
            ProcessListRequestIntoPrefixes(listRequest);
        }



        //Adds on the 
        private void ProcessListRequestIntoPrefixes(ListObjectsRequest listRequest)
        {
            ListObjectsResponse listResponse;
            int numFiles = 0;
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
                    //If it doesn't contain this prefix, add it to our list
                    if (!preTeamPrefixes.Contains(commonPrefix))
                        preTeamPrefixes.Add(commonPrefix);
                }
 
            // Set the marker property
            listRequest.Marker = listResponse.NextMarker;
            numFiles += listResponse.S3Objects.Count;
            } while (listResponse.IsTruncated);
            Console.WriteLine("There are this many files... " + numFiles);
        }

        public List<string> GetListOfPrefixes(string bucketPrefix)
        {
            GatherPreTeamPrefixes(bucketPrefix);
            return preTeamPrefixes;
        }

    }

}