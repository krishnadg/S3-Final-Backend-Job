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


    public abstract class SearchPrefixGenTestsBase : IDisposable
    {   
        protected AWSClientTest client;
        protected List<string> expectedPrefixes;

        protected SearchPrefixGenerator sut;
        protected SearchPrefixGenTestsBase()
        {
            // Do "global" initialization here; Called before every test method.
            client = new AWSClientTest();
            expectedPrefixes = new List<string>();

        }

        public void Dispose()
        {
            // Do "global" teardown here; Called after every test method.

        }
    } 



    public class SearchPrefixGeneratorTest : SearchPrefixGenTestsBase
    {
        [Fact]
        [Trait("Category", "Integration")]
        public void GetListOfPrefixes_0Teams0Files_ReturnEmpty()
        {
            //ARRANGE
            client.CreateEmptyBucket("S3SearchPrefixGenTestBucket0P");
            sut = new SearchPrefixGenerator(client.GetClient(), "S3SearchPrefixGenTestBucket0P");
            string bucketPrefix = "S3Bucket"; //For example, file path is S3Bucket/Team1/Files
            //ACT
            var result = sut.GetListOfPrefixes(bucketPrefix);

            //ASSERT
            Assert.Equal(expectedPrefixes, result);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void GetListOfPrefixes_1Teams4Files_Return1Prefixes()
        {
            //ARRANGE
            client.CreateBucket1Team1File("S3SearchPrefixGenTestBucket1P");
            sut = new SearchPrefixGenerator(client.GetClient(), "S3SearchPrefixGenTestBucket1P");
            string bucketPrefix = "S3Bucket"; //For example, file path is S3Bucket/Team1/Files

            expectedPrefixes.Add("S3Bucket/12_04_13/");
            //ACT
            var result = sut.GetListOfPrefixes(bucketPrefix);

            //ASSERT
            Assert.Equal(expectedPrefixes, result);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void GetListOfPrefixes_3Teams1File_Return1Prefixes()
        {
            //ARRANGE
            client.CreateBucket3Teams1FileEach("S3SearchPrefixGenTestBucket3P");
            sut = new SearchPrefixGenerator(client.GetClient(), "S3SearchPrefixGenTestBucket3P");
            string bucketPrefix = "S3Bucket"; //For example, file path is S3Bucket/Team1/Files

            expectedPrefixes.Add("S3Bucket/12_04_13/");
            //ACT
            var result = sut.GetListOfPrefixes(bucketPrefix);

            //ASSERT
            Assert.Equal(expectedPrefixes, result);
        }


    }
}
    