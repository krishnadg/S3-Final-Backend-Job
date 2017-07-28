
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using System.Collections.Generic;

namespace S3ClassLib
{
    /*Class to get the List<ListObjectResponse> by making requests using List<ListObjectRequest> to client's S3 Bucket */
    public class ObjectResponseGenerator 
    {
        
        AmazonS3Client client;

        List<ListObjectsResponse> objResponses = new List<ListObjectsResponse>();
        public ObjectResponseGenerator()
        {
            
        }

        public ObjectResponseGenerator(AmazonS3Client _client)
        {
            client = _client;
        }

        private void ProcessListIntoResponses(List<ListObjectsRequest> listObjRequests)
        {
            ListObjectsResponse listResponse;

            int count = 0;
            foreach (ListObjectsRequest listRequest in listObjRequests)
            {
                do
                {
                    Console.WriteLine("\nProcessing Request # " + count);

                    // Get listResponse for up to 1000 files after marker
                    listResponse = client.ListObjectsAsync(listRequest).GetAwaiter().GetResult();

                    // Added response to objresponses list
                    objResponses.Add(listResponse);
                    
                    // Set the marker property, continue getting list responses if more than 1000 files exist
                    listRequest.Marker = listResponse.NextMarker;
                    Console.WriteLine("Finished Processing Request # " + count);

                    count++;

                } while (listResponse.IsTruncated);

            }
        }

        public List<ListObjectsResponse> GetObjectResponseList(List<ListObjectsRequest> listObjRequests)
        {
            ProcessListIntoResponses(listObjRequests);
            

            return objResponses;
        }
    }
}