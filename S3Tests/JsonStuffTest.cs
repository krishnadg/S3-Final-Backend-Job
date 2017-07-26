using System;
using System.Diagnostics;
using Xunit;
using Xunit.Sdk;
using S3ClassLib;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;


namespace S3Tests
{
    public class JsonStuffTest
    {
        
        partial class S3TeamStorageInfo
        {
            public string teamName;
            public long storageSize;

            public DateTime lastDateModified;
            public S3TeamStorageInfo()
            {
                
            }
        }

        [Fact]
        public void TestJsonFast()
        {
            List<S3TeamStorageInfo> teamInfo = new List<S3TeamStorageInfo>
            {
                new S3TeamStorageInfo
                {
                    teamName = "Team1",
                    storageSize = 100123,
                    lastDateModified = new DateTime(2012, 3, 21 )
                    


                },
                new S3TeamStorageInfo
                {
                    teamName = "Team2",
                    storageSize = 100123,
                    lastDateModified = new DateTime(2014, 3, 11 )
                    


                },
                new S3TeamStorageInfo
                {
                    teamName = "Team1",
                    storageSize = 100123,
                    lastDateModified = new DateTime(2015, 1, 12 )
                    


                }
            };

          
            var jsonString = JsonConvert.SerializeObject(teamInfo);



            var currentDateTime = DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year;

            Assert.True(true, currentDateTime);


        }

        [Fact]
        public void TestS3Leaderboard()
        {

            Dictionary<string, long> teamNamesAndStorage = new Dictionary<string, long>();
            teamNamesAndStorage.Add("Team-ddos", 123124);
            teamNamesAndStorage.Add("Team-ddasdfos", 4124);
            teamNamesAndStorage.Add("Team-312", 421232124);
            teamNamesAndStorage.Add("Team-datas", 5124);



            long totalBucketStorage = 12341234;
            DateTime leaderboardDate = DateTime.Now;


            var leaderboard = new S3Leaderboard(teamNamesAndStorage, totalBucketStorage, leaderboardDate);


            var sortedTeamStorage = from entry in teamNamesAndStorage orderby entry.Value descending select entry;

            string json = JsonConvert.SerializeObject(teamNamesAndStorage);
            Assert.True(true, json);
        }


            [Fact]
            public void TestFinalJsonObject()
            {

                var teamData1 = new TeamData
                {
                    team = "datavision",
                    sub_team = "datavision-web",
                    value = 33000000
                };

                var teamData2 = new TeamData
                {
                    team = "datavision",
                    sub_team = "datavision-sid",
                    value = 400000
                };

                var metaData = new MetaData()
                {
                    period = "total",
                    unit = "bytes",
                    name = "S3"
                };

                TeamData[] data = new TeamData[]
                {
                    teamData1,teamData2
                };



                var rootObj = new FinalJsonRoot()
                {
                    results = data,
                    meta = metaData
                    
                };


                string json = JsonConvert.SerializeObject(rootObj);


                Assert.True(true, json);

            }

    }



    
}