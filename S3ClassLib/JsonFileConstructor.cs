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
    public class JsonFileConstructor
    {

        FinalJsonRoot rootObj;

        List<TeamData> teamData = new List<TeamData>();

        MetaData _meta;       

        //Could add in new constructor here to alter meta/teamdata field names
        public JsonFileConstructor()
        {
          

            _meta = new MetaData
            {
                period = "all_time",
                unit = "bytes",
                name = "S3"

            };

            rootObj = new FinalJsonRoot()
            {
                meta = _meta
            };
        }


        //Get Json and put it in S3 Storage...
        public void AddJsonFileToS3(AmazonS3Client client, Dictionary<string, long> teamsStorage)
        {


            ConvertDictToJsonObject(teamsStorage);

            string jsonString = JsonConvert.SerializeObject(rootObj);

            
            var jsonFileKeyPut = "leaderboard/s3.json";


            var currentDateTime = DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year;
            var jsonFileKeyWithDate = "leaderboard/s3-history/" + currentDateTime.ToString() + ".json";


            //Put both files, one with date, one as most recent to be used by hub leaderboard
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
        }

        //Sort dictionary and convert into array of TeamData for json root object's results field
        private void ConvertDictToJsonObject(Dictionary<string, long> teamsStorage)
        {
            var sortedTeamStorage = from entry in teamsStorage orderby entry.Value descending select entry;

            foreach (KeyValuePair<string, long> teamAndStorage in sortedTeamStorage)
            {
                var singleTeamData = new TeamData
                {
                    team = DetermineRootTeamName(teamAndStorage.Key),
                    value = teamAndStorage.Value,
                    sub_team = teamAndStorage.Key
                };
                teamData.Add(singleTeamData);
            }

            //Add team data array/list to json root obj for serialization
            rootObj.results = teamData.ToArray();
            
        }

        //Determine the root team name if there is one, otherwise team and sub_team values will be the same
        private string DetermineRootTeamName(string fullBucketName)
        {
            string rootTeam = fullBucketName;
            if (fullBucketName.Contains("-"))
            {
                var index = fullBucketName.IndexOf("-");
                rootTeam = fullBucketName.Substring(0, index);
            }

            return rootTeam;
        }


    }
}
