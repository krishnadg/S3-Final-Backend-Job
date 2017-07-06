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

namespace S3Tests
{


    public abstract class S3JobTestsBase : IDisposable
    {
       protected S3Job s3JobDoer;
       
       protected AmazonS3Client client;
       protected string bucket;
       protected string bucketPrefix;


       protected Dictionary<string, long> expectedDataStructure;
        protected S3JobTestsBase()
        {
            expectedDataStructure = new Dictionary<string, long>();
        }

        public void Dispose()
        {
            // Do "global" teardown here; Called after every test method.
        }
    } 


   

        public class S3JobTest : S3JobTestsBase
        {

             /*Test Helper method */
        public void AreSameDictionaries(Dictionary<string, long> expected, Dictionary<string, long> result)
        {  
            var countMatches = expected.Count == result.Count;
            Assert.True(countMatches, String.Format("expected count {0}, got count {1}", expected.Count, result.Count));

            
            foreach (KeyValuePair<string, long> pair in expected)
            {
                string teamName = pair.Key;
                bool hasTeamName = result.ContainsKey(teamName);
                Assert.True(hasTeamName, String.Format("expected team name {0}, got didn't have", teamName));

                bool teamSizeMatches = expected[teamName] == result[teamName];
                Assert.True(teamSizeMatches, String.Format("expected team size {0}, got team size {1}", expected[teamName], result[teamName]));

                

            }

        }

             // Uncomment to test when... actual client instantiated and bucket declared
            [Fact]
            [Trait("Category", "Integration")]
            public void S3Jobtest_FakeClientFakeBucket_Return3TeamsData()
            {

                //ARRANGE

                bucket = "S3CompleteJobTester3T";
                bucketPrefix = "S3Bucket";
                var testClient = new AWSClientTest();
                testClient.CreateBucket3Teams1FileEach("S3CompleteJobTester3T");
                client = testClient.GetClient();

                s3JobDoer = new S3Job(client, bucket, bucketPrefix);

                //Actual known file sizes must be input here
                long team1Size = 208;
                long team2Size = 0;
                long team3Size = 0;

                expectedDataStructure.Add("Team1", team1Size);
                expectedDataStructure.Add("Team2", team2Size);
                expectedDataStructure.Add("Team3", team3Size);


                //ACT

                var result = s3JobDoer.DoS3Job();

                //ASSERT
                AreSameDictionaries(expectedDataStructure, result);
            }
        }
}


