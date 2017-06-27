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


    public abstract class ObjRequestTestsBase : IDisposable
    {   
        protected AWSTestClient client;
//        protected List<string> teamNames; // dont need this because team names will be statically created for tests, only need to verify the object requests
        protected List<ListObjectsRequest> expectedObjectsRequests;

        protected ObjectRequestGenerator objRequestGen;

        protected ObjRequestTestsBase()
        {
            // Do "global" initialization here; Called before every test method.
            client = new AWSTestClient();
            //teamNames = new List<string>();  
            expectedObjectsRequests = new List<ListObjectsRequest>();
            objRequestGen = new ObjectRequestGenerator(client.GetClient(), "S3TestBucket");

        }

        public void Dispose()
        {
            // Do "global" teardown here; Called after every test method.
          //  client.RemoveBucketFromS3("S3TestBucket");
        }
    } 



    public class TestObjectResponseParser : ObjRequestTestsBase
    {
    
      /*  [Fact]
        public void TestObjectRequest()
        {
            Assert.True(true);
        }
        

        [Fact]
        public void TestTeamNameGeneration3Teams3Files()
        {
           

            
            Assert.True(true);
        }
        */
    }
}
