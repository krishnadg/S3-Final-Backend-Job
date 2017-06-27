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
        protected List<ListObjectsResponse> listResponsesArg;
        protected Dictionary<string, long> expected;
        protected ObjectResponseParser sut;

        protected ObjResponseTestsBase()
        {
            // Do "global" initialization here; Called before every test method.

            client = new AWSTestClient();
            expected = new Dictionary<string, long>();
            listResponsesArg = new List<ListObjectsResponse>();
        }

        public void Dispose()
        {
            // Do "global" teardown here; Called after every test method.
        }
    } 


    namespace S3Tests
    {


    public class ObjectResponseParserTest : ObjResponseTestsBase
    {
    
        [Fact]
        public void ParseListObjectresponses_3ListResponses_Return3Entries()
        {
             //ARRANGE
            sut = new ObjectResponseParser();
            
            listResponsesArg.Add(client.GetListResponse()); //Static List Response from AWSClient.cs
            listResponsesArg.Add(client.GetListResponse());
            
            //ACT


            //ASSERT
        }
        

        [Fact]
        public void TestParseOnListResponseSize3()
        {
            

        }
        
    }
}
}