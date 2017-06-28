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

        public void DataStructuresAreSame(Dictionary<string, long> expected, Dictionary<string, long> result)
        {
            var countMatches = expected.Count == result.Count;
            Assert.True(countMatches, String.Format("expected Dict count {0}, got Dict count {1}", expected.Count, result.Count ));


           
            foreach (KeyValuePair<string, long> expectedTeamData in expected)
            {
                var teamNameMatches = result.ContainsKey(expectedTeamData.Key);
                Assert.True(teamNameMatches, String.Format("expected contains team name {0}, got contains {1}", expectedTeamData.Key, result.ContainsKey(expectedTeamData.Key) ));

                long resultTeamStorage;
                
                var teamDataFound = result.TryGetValue(expectedTeamData.Key, out resultTeamStorage);
                var teamDataMatches = expectedTeamData.Value == resultTeamStorage;
                Assert.True(teamDataMatches, String.Format("expected team storage size {0}, got size {1}", expectedTeamData.Value, resultTeamStorage ));

            }
        }
    
        [Fact]
        public void ParseListObjectresponses_3ListResponses_Return3Entries()
        {
             //ARRANGE
            sut = new ObjectResponseParser();
            
            listResponsesArg.Add(client.GetListResponse()); //Static List Response from AWSClient.cs
            listResponsesArg.Add(client.GetListResponse2());
            listResponsesArg.Add(client.GetListResponse3());


            expected.Add("Team1", 1116);
            expected.Add("Team2", 573);
            expected.Add("Team3", 228878);

            
            //ACT

            var result = sut.GetDataStructure(listResponsesArg);

            //ASSERT
            //Assert.Equal(expected, result);
            DataStructuresAreSame(expected, result);
        }
        

        [Fact]
        public void TestParseOnListResponseSize3()
        {
            

        }
        
    }
}
}