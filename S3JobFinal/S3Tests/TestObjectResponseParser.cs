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


    public abstract class ObjResponseTestsBase : IDisposable
    {
        protected AWSTestClient client;
        protected Dictionary<string, long> expected;
        protected ObjectResponseParser objParser;

        protected ObjResponseTestsBase()
        {
            // Do "global" initialization here; Called before every test method.

            client = new AWSTestClient();
            expected = new Dictionary<string, long>();
            objParser = new ObjectResponseParser();
        }

        public void Dispose()
        {
            // Do "global" teardown here; Called after every test method.
        }
    } 


    namespace S3Tests
    {


    public class TestObjectResponseParser : ObjResponseTestsBase
    {
    
      /*   [Fact]
        public void TestParseListResponseOnEmptyListResponse()
        {
            var testResponse = new ListObjectsResponse();
            objParser.ParseObjectResponse(testResponse);
            var sut = objParser.GetDataStructure();
            
            Assert.Equal(expected, sut );
        }
        

        [Fact]
        public void TestParseOnListResponseSize3()
        {
            var testResponse = client.GetListResponse();
            objParser.ParseObjectResponse(testResponse);

            var sut = objParser.GetDataStructure();
            
            expected.Add("S3Bucket/Team1/file1", 52);
            expected.Add("S3Bucket/Team1/file2", 90);
            expected.Add("S3Bucket/Team1/file3", 431);

            Assert.Equal(expected, sut);

        }
        */
    }
}
}