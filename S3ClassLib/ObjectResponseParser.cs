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
        //Storage for parsing ----
        // k,v = (list team name, total storage)
        Dictionary<string, long> teamsStorage = new Dictionary<string, long>();
        public ObjectResponseParser()
        {
            
        }


        public void ParseAllObjectResponses(List<ListObjectsResponse> listObjResponses)
        {
            foreach (ListObjectsResponse resp in listObjResponses)
            {
                ParseSingleObjectResponse(resp);
            }
        }


        public void ParseSingleObjectResponse(ListObjectsResponse listResponse)
        {
            Contract.Requires(listResponse != null);

            foreach (string str in listResponse.CommonPrefixes)
            {
                Console.WriteLine("CommonPrefix: " + str);

            }
            foreach (S3Object obj in listResponse.S3Objects)
            {
                ParseS3Object(obj);
            }
        }

        private void ParseS3Object(S3Object obj)
        {
                Contract.Requires(obj != null);

                string teamName = obj.Key;
                
                long objSize = obj.Size;
                Console.WriteLine("Key: " + teamName);

                //Add to previously calculated size (value) for specified team (key)
                if (teamsStorage.ContainsKey(teamName))
                {
                    long currentStorage = teamsStorage[teamName];
                    long updatedStorage = currentStorage + objSize;
                    teamsStorage[teamName] = updatedStorage;
                }
                //Add new team (key) with objects file size (value)
                else
                {
                    teamsStorage.Add(teamName, objSize);
                }
        }


        public Dictionary<string, long> GetDataStructure()
        {
            return teamsStorage;
        }



        public void PrintData()
        {
            foreach (KeyValuePair<string, long> kvp in teamsStorage)
            {
            //textBox3.Text += ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            }
        
        }
    }   
}