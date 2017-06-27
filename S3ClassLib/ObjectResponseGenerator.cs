
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
        string bucket;
        public ObjectResponseGenerator()
        {
            
        }

        public ObjectResponseGenerator(AmazonS3Client _client)
        {
            client = _client;
        }

        public void ProcessListIntoResponses(List<ListObjectsRequest> listObjRequests)
        {
            ListObjectsResponse listResponse;

            foreach (ListObjectsRequest listRequest in listObjRequests)
            {
                listResponse = client.ListObjectsAsync(listRequest).GetAwaiter().GetResult();
                objResponses.Add(listResponse);
            }
        }

        public List<ListObjectsResponse> GetObjectResponseList(List<ListObjectsRequest> listObjRequests)
        {
            ProcessListIntoResponses(listObjRequests);
            return objResponses;
        }
    }
}