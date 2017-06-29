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
            // Uncomment to test when... actual client instantiated and bucket declared
            // [Fact]
            public void S3Jobtest_FakeClientFakeBucket_Return3TeamsData()
            {

                //ARRANGE
                bucket = "S3TestBucket1";
                client = new AWSClientTest().GetClient();
                s3JobDoer = new S3Job(client, bucket);

                //Actual known file sizes must be input here
                long team1Size = 0;
                long team2Size = 0;
                long team3Size = 0;

                expectedDataStructure.Add("Team1", team1Size);
                expectedDataStructure.Add("Team2", team2Size);
                expectedDataStructure.Add("Team3", team3Size);


                //ACT

                var result = s3JobDoer.DoS3Job();

                //ASSERT
                Assert.Equal(expectedDataStructure, result);
            }
        }
}


