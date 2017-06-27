using System;
using Xunit;
using S3ClassLib;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System.Threading.Tasks;

namespace S3Tests
{
    public class UnitTest1
    {

        
        [Fact]
        public void TestListBucketNames()
        {
            var testConfig = new AmazonS3Config();
            testConfig.ServiceURL = "http://localhost:4569";

            var testCreds = new BasicAWSCredentials("foo", "bar");

            var thingWeAreTesting = new AWSSetup(testCreds, testConfig);

            var fakeResponse = new ListObjectsResponse();
            fakeResponse.S3Objects.Add(S3Object());
            
            
        }
        
    }
}
